using System;
using login;
using CustomerManager;
using System.Data;

public class Atm {
    

    public static void Main(string[] args) {
        int user = -1;
        bool isAdmin = false;
        bool exit = false;

        while (user == -1) {
            user = LoginManager.getLogin();            
        }

        if (isAdmin) {
            while (!exit) {
                
            }
            Console.WriteLine("Thank you for using this ATM. Goodbye!");
        } else {
            customerManager cm = new customerManager(user);
            cm.menu();
            Console.WriteLine("Thank you for using this ATM. Goodbye!");
        }

    }    
}

