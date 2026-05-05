using System.Data;
using System.Diagnostics.Contracts;
using dal;
namespace CustomerManager;
public class customerManager {
   
    string login {get; set;} = "";
    string holder {get; set;} = "";
    int accountNumber {get; set;} = 0;
    int pin {get; set;} = 0;
    double balance {get; set;} = 0.0;
    string status {get; set;} = "Active";

    public customerManager(int user) {
        //Use dal to get other info
        getInfo(user);

    }
    
    public void getInfo(int user) {
        DataTable dt = Dal.searchID(user);

        foreach (DataRow row in dt.Rows) {
            login = row["login"].ToString();
            holder = row["holder"].ToString();
            status = Convert.ToBoolean(row["isActive"]) ? "Active" : "Disabled";
            pin = Convert.ToInt32(row["pin"]);
            balance = Convert.ToDouble(row["balance"]);
            accountNumber = user;

            login = (login != null) ? login : "";
            holder = (holder != null) ? holder : "";
        }        
    }

    public void withdraw() {
        Console.WriteLine("Enter the withdraw amount");
        string? amtS = Console.ReadLine();

        if (!Double.TryParse(amtS, out double amt)) {
            throw new FormatException("Withdrawal must be a number. Please try again.");
        }

        if (balance < amt) {
            Console.WriteLine("Insufficient Funds. Please Try Again.");
            withdraw();
            return;
        }
        
        Dal.updateAccountBalance(accountNumber,balance-amt);    
        balance = Dal.getAccountBalance(accountNumber);
    
        //Run api to update amount
        Console.WriteLine("Cash Successfully Withdrawn");
        Console.WriteLine("Account #" + accountNumber);
        Console.WriteLine("Date: " + DateTime.Today.ToString("MM/dd/yyyy"));
        Console.WriteLine("Withdrawn: " + amt);
        Console.WriteLine("Balance: " + balance);        
    }

    public void deposit() {
        Console.WriteLine("Enter the cash amount to deposit:");
        string? amtS = Console.ReadLine();

        if (!Double.TryParse(amtS, out double amt)) {
            throw new FormatException("Deposit must be a number. Please try again.");
        }
        
        Dal.updateAccountBalance(accountNumber,balance+amt);    
        balance = Dal.getAccountBalance(accountNumber);
    
        //Run api to update amount
        Console.WriteLine("Cash Successfully Deposited");
        Console.WriteLine("Account #" + accountNumber);
        Console.WriteLine("Date: " + DateTime.Today.ToString("MM/dd/yyyy"));
        Console.WriteLine("Withdrawn: " + amt);
        Console.WriteLine("Balance: " + balance);  
    }

    public void display() {
        Console.WriteLine("Account #" + accountNumber);
        Console.WriteLine("Date: " + DateTime.Today.ToString("MM/dd/yyyy"));
        Console.WriteLine("Balance: " + balance);
    }

    public void menu() {
        string? val = "";
        while (val != "5") {
            Console.WriteLine("1----Withdraw Cash");
            Console.WriteLine("3----Deposit Cash");
            Console.WriteLine("4----Display Balance");
            Console.WriteLine("5----Exit");
            val = Console.ReadLine();

            switch (val) {
                case "1": withdraw(); break;
                case "3": deposit(); break;
                case "4": display(); break;
                case "5": break;
                default: Console.WriteLine("Invalid Input, Please Try Again"); break;
            }
        }
    }

}