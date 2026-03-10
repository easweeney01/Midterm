using System;
using login;
using CustomerManager;
using System.Data;
using dal;
using Org.BouncyCastle.Pqc.Crypto.Cmce;
using System.ComponentModel;
using AdminManager;

public class Atm {
    

    public static void Main(string[] args) {
        int user = -1;
        bool isAdmin = false;
        bool exit = false;
        
        Console.Clear();
        while (user == -1) {
            user = LoginManager.getLogin();            
        }
        
        isAdmin = Dal.getAdmin(user);

        Console.Clear();
        
        if (isAdmin) {
            adminManager am = new adminManager();
            am.menu();
            Console.WriteLine("Thank you for using this ATM. Goodbye!");
        } else {
            customerManager cm = new customerManager(user);
            cm.menu();
            Console.WriteLine("Thank you for using this ATM. Goodbye!");
        }

    }    
}

