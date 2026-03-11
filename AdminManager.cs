namespace AdminManager;

using System.Data;
using System.Diagnostics.Contracts;
using dal;

public class adminManager {
    public adminManager() {}

    public void createNewAccount() {
        Console.Clear();

        try {
            Console.Write("Login:");
            string? l = Console.ReadLine(); if (l == null) {l = "";}

            Console.Write("PIN:");
            int p = Convert.ToInt32(Console.ReadLine()); 
            if (p < 0 || p > 99999) {Console.WriteLine("PIN must be five numbers"); return;}
            Console.Write("Holder's name:");
            string? h = Console.ReadLine(); if (h == null) {h = "";}

            Console.Write("Starting Balance:");
            double b = Convert.ToDouble(Console.ReadLine());

            string ?a = "";
            while (a != "y" && a != "n") {
                Console.Write("Active? (Y/N):");
                a = Console.ReadLine(); if (a == null) {a = "";}
                a = a.ToLower();    
            }      

            int id = Dal.createAccount(h,l,p,b,a == "y");
            Console.WriteLine("Account " + id + " Successfully Created.");   
        } catch {
            Console.WriteLine("Account Creation Failed."); 
        }

    }

    public void deleteAccount() {
        Console.Clear();
        Console.Write("Enter the account number to which you want to delete:");
        try {
            int num = Convert.ToInt32(Console.ReadLine());

            DataTable dt = Dal.searchID(num);

            if (dt.Rows.Count == 0) {
                Console.WriteLine("Account Not Found");
                return;
            }

            string? s = dt.Rows[0]["holder"].ToString();
            if (s == null) {s = "";}

            Console.Write("You are about to delete the account of " + s + ".\nRepeat the account number to proceed.");
            int num2 = Convert.ToInt32(Console.ReadLine());

            if (num == num2) {
                int del = Dal.deleteAccount(num);
                Console.WriteLine("Account Deleted Successfully.");
            } else {
                Console.WriteLine("No match.");
            }
        } catch {
            Console.Clear();
            Console.WriteLine("Delete Account failed.");
            return;
        }

    }

    public void updateAccount() {
        Console.Clear();
        Console.Write("Enter the Account Number:");
        int id = Convert.ToInt32(Console.ReadLine());

        bool done = false; 

        while (!done) {
            Console.Clear();
            try {
                DataTable dt = Dal.searchID(id);
                string? h = dt.Rows[0]["holder"].ToString();
                double? b = Convert.ToDouble(dt.Rows[0]["balance"]);
                string? l = dt.Rows[0]["login"].ToString();
                int p = Convert.ToInt32(dt.Rows[0]["pin"]);
                bool s = Convert.ToBoolean(dt.Rows[0]["isActive"]);

                l = (l != null) ? l : "";
                h = (h != null) ? h : "";
                //p = (p != null) ? p : 0;

                Console.WriteLine("Account #" + id);
                Console.WriteLine("Holder: " + h);
                Console.WriteLine("Balance: " + b);
                Console.WriteLine("Status: " + (s ? "Active" : "Disabled"));
                Console.WriteLine("Login: " + l);
                Console.WriteLine("Pin Code: " + p.ToString("D5"));
                Console.WriteLine("");

                Console.WriteLine("Select an attribute to edit:");
                Console.WriteLine("1-- Holder Name");
                Console.WriteLine("2-- Login");
                Console.WriteLine("3-- PIN");
                Console.WriteLine("4-- Status");
                Console.WriteLine("5-- Exit");
                string? choice = Console.ReadLine(); if (choice == null) choice = "";

                switch (choice) {
                    case "1": updateHolder(id,h,l,(int) p,s); break;
                    case "2": updateLogin(id,h,l,(int) p,s); break;
                    case "3": updatePIN(id,h,l,(int) p,s); break;
                    case "4": updateStatus(id,h,l,(int) p,s); break;
                    case "5": Console.WriteLine("Exiting Update Mode."); done = true; break;
                    default: Console.WriteLine("Invalid input. Try again."); break;
                }
            } catch {
                
            }
        }

        
    }

    public int searchForAccount() {
        Console.Clear();
        Console.Write("Enter Account Number:");
        try
        {
            int val = Convert.ToInt32(Console.ReadLine());  
            DataTable dt = Dal.searchID(val);

            if (dt.Rows.Count == 0) {
                Console.WriteLine("No matching account found.\n");
                return 0;
            }

            Console.WriteLine("The account information is:");

            foreach (DataRow row in dt.Rows) {

                    string? login = row["login"].ToString();
                    string? holder = row["holder"].ToString();
                    string status = Convert.ToBoolean(row["isActive"]) ? "Active" : "Disabled";
                    int pin = Convert.ToInt32(row["pin"]);
                    double balance = Convert.ToDouble(row["balance"]);
                    int accountNumber = val;

                    login = (login != null) ? login : "";
                    holder = (holder != null) ? holder : "";

                    Console.WriteLine("Account #" + val);
                    Console.WriteLine("Holder: " + holder);
                    Console.WriteLine("Balance: " + balance);
                    Console.WriteLine("Status: " + status);
                    Console.WriteLine("Login: " + login);
                    Console.WriteLine("Pin Code: " + pin.ToString("D5"));
                    Console.WriteLine("");


            
            }

        } catch {
            Console.Clear();
            Console.WriteLine("Search ID be a number. Please try again.\n");
            return searchForAccount();
        }
        
        return 0;
    }

    public bool updateHolder(int id, string holder, string login, int pin, bool status) {
        Console.Write("Enter new holder name:");
        string? newHolder = Console.ReadLine(); if (newHolder == null) {
            Console.WriteLine("Invalid input, please try again."); return false;
        }

        Dal.updateAccount(id,newHolder,login,pin,status);
        return true;
    }

    public bool updateLogin(int id, string holder, string login, int pin, bool status) {
        Console.Write("Enter new login:");
        string? newLogin = Console.ReadLine(); if (newLogin == null) {
            Console.WriteLine("Invalid input, please try again."); return false;
        }

        Dal.updateAccount(id,holder,newLogin,pin,status);
        return true;
    }

    public bool updatePIN(int id, string holder, string login, int pin, bool status) {
        Console.Write("Enter new pin:");
        int newPin = Convert.ToInt32(Console.ReadLine());

        if (pin < 0 || pin > 99999) {Console.WriteLine("Invalid PIN."); return false;}

        Dal.updateAccount(id,holder,login,newPin,status);
        return true;
    }
    
    public bool updateStatus(int id, string holder, string login, int pin, bool status) {
        Console.Write("Is active? (Y/N):");
        string? newStatus = Console.ReadLine(); 
        
        if (newStatus == null) {
            Console.WriteLine("Invalid input, please try again."); return false;
        } newStatus = newStatus.ToLower();
        
        if (newStatus != "n" && newStatus != "y") {
            Console.WriteLine("Invalid input, please try again."); return false;
        }

        Dal.updateAccount(id,holder,login,pin,(newStatus == "y"));
        return true;
    }

    public void menu() {
        string? val = "";
        while (val != "6") {
            Console.WriteLine("1----Create New Account");
            Console.WriteLine("2----Delete Existing Account");
            Console.WriteLine("3----Update Account Information");
            Console.WriteLine("4----Search for Account");
            Console.WriteLine("6----Exit");
            val = Console.ReadLine();

            switch (val) {
                case "1": createNewAccount(); break;
                case "2": deleteAccount(); break;
                case "3": updateAccount(); break;
                case "4": searchForAccount(); break;
                case "6": break;
                default: Console.WriteLine("Invalid Input, Please Try Again"); break;
            }
        }
    }

}