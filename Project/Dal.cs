using System.Data;

namespace dal;

using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

[ExcludeFromCodeCoverage]
public class Dal {
	private const string connectionString = "server=host.docker.internal;port=3333;uid=root;pwd=a;database=midterm";

	/// <summary>
	/// Returns account balance from DAL.
	/// </summary>
	/// <param name="accountNum"></param>
	/// <returns>Returns account balance.</returns>
	public static double getAccountBalance(int accountNum) {
		var dt = new DataTable();

		using (var connection = new MySqlConnection(connectionString)) {
			connection.Open();

			var query = "select balance from AtmAccounts where accountNum = @accountNum;";

			var cmd = new MySqlCommand(query, connection);
			using (cmd) {
				cmd.Parameters.AddWithValue("@accountNum", accountNum);

				var res = cmd.ExecuteScalar();

				return Convert.ToDouble(res);
			}
		}
	}

	/// <summary>
	/// Updates account balance to DAL.
	/// </summary>
	/// <param name="accountNum"></param>
	/// <param name="balance"></param>
	/// <returns>1 for success; 0 for failure.</returns>
	public static int updateAccountBalance(int accountNum, double balance) {
		using (var connection = new MySqlConnection(connectionString)) {
			connection.Open();

			var update = @"UPDATE AtmAccounts SET balance = @balance WHERE accountNum = @accountNum";

			var cmd = new MySqlCommand(update, connection);
			using (cmd) {
				cmd.Parameters.AddWithValue("@accountNum", accountNum);
				cmd.Parameters.AddWithValue("@balance", balance);

				return cmd.ExecuteNonQuery();
			}

		}
	}

	/// <summary>
	/// Adds a new account to the DAL with the data provided.
	/// </summary>
	/// <param name="holder"></param>
	/// <param name="login"></param>
	/// <param name="pin"></param>
	/// <param name="balance"></param>
	/// <param name="isActive"></param>
	/// <returns>The ID of the new account.</returns>
	public static int createAccount(string holder, string login, int pin, double balance, bool isActive) {
		var dt = new DataTable();
		using (var connection = new MySqlConnection(connectionString)) {
			connection.Open();

			var insert = @"insert into AtmAccounts (holder, login, pin, balance, isActive) values (@holder,@login,@pin,@balance,@isActive)";

			var cmd = new MySqlCommand(insert, connection);
			using (cmd) {
				cmd.Parameters.AddWithValue("@holder", holder);
				cmd.Parameters.AddWithValue("@login", login);
				cmd.Parameters.AddWithValue("@pin", pin);
				cmd.Parameters.AddWithValue("@balance", balance);
				cmd.Parameters.AddWithValue("@isActive", isActive);

				cmd.ExecuteNonQuery();
			}
			return (int)cmd.LastInsertedId;
		}
	}

	/// <summary>
	/// Deletes the account of the account number.
	/// </summary>
	/// <param name="accountNum"></param>
	/// <returns>1 for success, 0 for failure.</returns>
	public static int deleteAccount(int accountNum) {
		using (var connection = new MySqlConnection(connectionString)) {
			connection.Open();

			var delete = @"delete from AtmAccounts where accountNum = @accountNum limit 1";

			var cmd = new MySqlCommand(delete, connection);
			using (cmd) {
				cmd.Parameters.AddWithValue("@accountNum", accountNum);

				return cmd.ExecuteNonQuery();
			}
		}
	}

	/// <summary>
	/// Updates account data.
	/// </summary>
	/// <param name="accountNum"></param>
	/// <param name="holder"></param>
	/// <param name="login"></param>
	/// <param name="pin"></param>
	/// <param name="isActive"></param>
	/// <returns></returns>
	public static int updateAccount(int accountNum, string holder, string login, int pin, bool isActive) {
		using (var connection = new MySqlConnection(connectionString)) {
			connection.Open();

			var update = @"UPDATE AtmAccounts SET holder = @holder, login = @login, pin = @pin, isActive = @isActive WHERE accountNum = @accountNum";

			var cmd = new MySqlCommand(update, connection);
			using (cmd) {
				cmd.Parameters.AddWithValue("@accountNum", accountNum);
				cmd.Parameters.AddWithValue("@holder", holder);
				cmd.Parameters.AddWithValue("@login", login);
				cmd.Parameters.AddWithValue("@pin", pin);
				cmd.Parameters.AddWithValue("@isActive", isActive);

				return cmd.ExecuteNonQuery();
			}

		}
	}

	/// <summary>
	/// Searches for account data by ID.
	/// </summary>
	/// <param name="accountNum"></param>
	/// <returns>A datatable with the matching row of account information.</returns>
	public static DataTable searchID(int accountNum) {
		var dt = new DataTable();

		using (var connection = new MySqlConnection(connectionString)) {
			connection.Open();

			var query = "select * from AtmAccounts where accountNum = @accountNum;";

			var cmd = new MySqlCommand(query, connection);
			using (cmd) {
				cmd.Parameters.AddWithValue("@accountNum", accountNum);

				using (var da = new MySqlDataAdapter(cmd)) {
					da.Fill(dt);
				}
			}

		}

		return dt;
	}

	/// <summary>
	///	Finds the account by login and pin.
	/// </summary>
	/// <param name="login"></param>
	/// <param name="pin"></param>
	/// <returns>The account number, or -1 if not found.</returns>
	public static int? login(string login, int pin) {
		var dt = new DataTable();

		using (var connection = new MySqlConnection(connectionString)) {
			connection.Open();

			var query = "select accountNum from AtmAccounts where login = @login and pin = @pin;";

			var cmd = new MySqlCommand(query, connection);
			using (cmd) {
				cmd.Parameters.AddWithValue("@login", login);
				cmd.Parameters.AddWithValue("@pin", pin);

				var res = cmd.ExecuteScalar();

				return (res == null || res == DBNull.Value) ? -1 : Convert.ToInt32(res);
			}
		}
	}

	/// <summary>
	///	Checks to see if the account is an admin account. This is used to decide whether to run the customer or administrator manager upon logging in.
	/// </summary>
	/// <param name="accNum"></param>
	/// <returns>A boolean denoting whether or not the account is an admin.</returns>
	public static bool getAdmin(int accNum) {
		var dt = new DataTable();

		using (var connection = new MySqlConnection(connectionString)) {
			connection.Open();

			var query = "select isAdmin from AtmAccounts where accountNum = @accountNum;";

			var cmd = new MySqlCommand(query, connection);
			using (cmd) {
				cmd.Parameters.AddWithValue("@accountNum", accNum);

				var res = cmd.ExecuteScalar();

				if (res == null || res == DBNull.Value) { return false; }
				else { return Convert.ToInt32(res) == 1; }
				;
			}

		}
	}
}

public interface IDal {
	double getAccountBalance(int accountNum);
	int updateAccountBalance(int accountNum, double balance);
	int createAccount(string holder, string login, int pin, double balance, bool isActive);
	int deleteAccount(int accountNum);
	int updateAccount(int accountNum, string holder, string login, int pin, bool isActive);
	DataTable searchID(int accountNum);
	int? login(string login, int pin);
	bool getAdmin(int accNum);
}

public class DalWrapper : IDal {
	public double getAccountBalance(int accountNum) =>
		Dal.getAccountBalance(accountNum);

	public int updateAccountBalance(int accountNum, double balance) =>
		Dal.updateAccountBalance(accountNum, balance);

	public int createAccount(string holder, string login, int pin, double balance, bool isActive) =>
		Dal.createAccount(holder, login, pin, balance, isActive);

	public int deleteAccount(int accountNum) =>
		Dal.deleteAccount(accountNum);

	public int updateAccount(int accountNum, string holder, string login, int pin, bool isActive) =>
		Dal.updateAccount(accountNum, holder, login, pin, isActive);

	public DataTable searchID(int accountNum) =>
		Dal.searchID(accountNum);

	public int? login(string login, int pin) =>
		Dal.login(login, pin);

	public bool getAdmin(int accNum) =>
		Dal.getAdmin(accNum);
}
