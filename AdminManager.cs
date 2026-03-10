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

            string s = dt.Rows[0]["holder"].ToString();
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

        DataTable dt = Dal.searchID(id);

        //Print Standing Data
        
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
                string login = row["login"].ToString();
                string holder = row["holder"].ToString();
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
                Console.WriteLine("Pin Code: " + pin);
                Console.WriteLine("");
            }

        } catch {
            Console.Clear();
            Console.WriteLine("Search ID be a number. Please try again.\n");
            return searchForAccount();
        }
        

        return 0;
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
                case "3": break;
                case "4": searchForAccount(); break;
                case "6": break;
                default: Console.WriteLine("Invalid Input, Please Try Again"); break;
            }
        }
    }

}