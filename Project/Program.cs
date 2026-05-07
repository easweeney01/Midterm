using System;
using login;
using CustomerManager;
using System.Data;
using dal;
using Org.BouncyCastle.Pqc.Crypto.Cmce;
using System.ComponentModel;
using AdminManager;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Atm {

	[ExcludeFromCodeCoverage]
	public static void Main(string[] args) {
		int user = -1;
		bool isAdmin = false;

		Console.Clear();
		while (user == -1) {
			LoginManager lm = new LoginManager();
			user = lm.getLogin();
		}

		isAdmin = Dal.getAdmin(user);

		Console.Clear();

		if (isAdmin) {
			adminManager am = new adminManager(new DalWrapper());
			try {
				am.menu();
			}
			catch (Exception ex) {
				Console.WriteLine("Sorry, an error occurred.");
				Console.WriteLine(ex.Message + "\n");

				am.menu();
			}

			Console.WriteLine("Thank you for using this ATM. Goodbye!");
		}
		else {
			customerManager cm = new customerManager(user, new DalWrapper());

			try {
				cm.menu();
			}
			catch (Exception ex) {
				Console.WriteLine("Sorry, an error occurred.");
				Console.WriteLine(ex.Message + "\n");

				cm.menu();
			}

			Console.WriteLine("Thank you for using this ATM. Goodbye!");
		}

	}
}

