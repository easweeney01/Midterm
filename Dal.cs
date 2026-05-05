using System.Data;

namespace dal;

using MySql.Data.MySqlClient;
using System.Data;
using System.Runtime.InteropServices;

public class Dal {
    private const string connectionString = "server=host.docker.internal;port=3333;uid=root;pwd=a;database=midterm";

    public static double getAccountBalance(int accountNum) {
        var dt = new DataTable();

        using (var connection = new MySqlConnection(connectionString)) {
            connection.Open();

            var query = "select balance from AtmAccounts where accountNum = @accountNum;";

            var cmd = new MySqlCommand(query,connection);
            using (cmd) {
                cmd.Parameters.AddWithValue("@accountNum",accountNum);
        
                var res = cmd.ExecuteScalar();

                return Convert.ToDouble(res);
            }
        }     
    }

    public static int updateAccountBalance(int accountNum, double balance) {
        using (var connection = new MySqlConnection(connectionString)) {
            connection.Open();

            var update = @"UPDATE AtmAccounts SET balance = @balance WHERE accountNum = @accountNum";

            var cmd = new MySqlCommand(update,connection);
            using (cmd) {
                cmd.Parameters.AddWithValue("@accountNum",accountNum);
                cmd.Parameters.AddWithValue("@balance",balance);

                return cmd.ExecuteNonQuery();            
            }
            
        }
    }

    public static int createAccount(string holder,string login, int pin,double balance, bool isActive) {
        var dt = new DataTable();
        using (var connection = new MySqlConnection(connectionString)) {
            connection.Open();

            var insert = @"insert into AtmAccounts (holder, login, pin, balance, isActive) values (@holder,@login,@pin,@balance,@isActive)";

            var cmd = new MySqlCommand(insert,connection);
            using (cmd) {
                cmd.Parameters.AddWithValue("@holder",holder);
                cmd.Parameters.AddWithValue("@login",login);
                cmd.Parameters.AddWithValue("@pin",pin);
                cmd.Parameters.AddWithValue("@balance",balance);
                cmd.Parameters.AddWithValue("@isActive",isActive);
        
                cmd.ExecuteNonQuery();            
            }
            return (int) cmd.LastInsertedId;
        }
    }

    public static int deleteAccount(int accountNum) {
        using (var connection = new MySqlConnection(connectionString)) {
            connection.Open();

            var delete = @"delete from AtmAccounts where accountNum = @accountNum limit 1";

            var cmd = new MySqlCommand(delete,connection);
            using (cmd) {
                cmd.Parameters.AddWithValue("@accountNum",accountNum);
        
                return cmd.ExecuteNonQuery();
            }

        }
    }

    //TODO: UpdateAccount
    public static int updateAccount(int accountNum, string holder, string login, int pin, bool isActive) {
        using (var connection = new MySqlConnection(connectionString)) {
            connection.Open();

            var update = @"UPDATE AtmAccounts SET holder = @holder, login = @login, pin = @pin, isActive = @isActive WHERE accountNum = @accountNum";

            var cmd = new MySqlCommand(update,connection);
            using (cmd) {
                cmd.Parameters.AddWithValue("@accountNum",accountNum);
                cmd.Parameters.AddWithValue("@holder",holder);
                cmd.Parameters.AddWithValue("@login",login);
                cmd.Parameters.AddWithValue("@pin",pin);
                cmd.Parameters.AddWithValue("@isActive",isActive);

                return cmd.ExecuteNonQuery();            
            }
            
        }
    }

    public static DataTable searchID(int accountNum) {
        var dt = new DataTable();

        using (var connection = new MySqlConnection(connectionString)) {
            connection.Open();

            var query = "select * from AtmAccounts where accountNum = @accountNum;";

            var cmd = new MySqlCommand(query,connection);
            using (cmd) {
                cmd.Parameters.AddWithValue("@accountNum",accountNum);
        
                using (var da = new MySqlDataAdapter(cmd)) {
                    da.Fill(dt);
                }                
            }


        }

        return dt;
    }

    public static int? login(string login, int pin) {
        var dt = new DataTable();

        using (var connection = new MySqlConnection(connectionString)) {
            connection.Open();

            var query = "select accountNum from AtmAccounts where login = @login and pin = @pin;";

            var cmd = new MySqlCommand(query,connection);
            using (cmd) {
                cmd.Parameters.AddWithValue("@login",login);
                cmd.Parameters.AddWithValue("@pin",pin);
        
                var res = cmd.ExecuteScalar();

                return (res == null || res == DBNull.Value) ? -1 : Convert.ToInt32(res);       
            }
        }
    }

    public static bool getAdmin(int accNum) {
        var dt = new DataTable();

        using (var connection = new MySqlConnection(connectionString)) {
            connection.Open();

            var query = "select isAdmin from AtmAccounts where accountNum = @accountNum;";

            var cmd = new MySqlCommand(query,connection);
            using (cmd) {
                cmd.Parameters.AddWithValue("@accountNum",accNum);
        
                var res = cmd.ExecuteScalar();
                
                if (res == null || res == DBNull.Value) {return false;} else 
                { return Convert.ToInt32(res) == 1;};         
            }

        }
    }
}