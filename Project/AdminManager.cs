namespace AdminManager;

using System.Data;
using System.Diagnostics.Contracts;
using dal;

public class adminManager {
	private readonly IDal _dal;

	///<summary>
	///Constructs the admin manager with a default dal wrapper.
	///</summary>
	public adminManager() {
		_dal = new DalWrapper();
	}

	///<summary>
	///Constructs the admin manager.
	///</summary>
	///<param name="dal">Dal Wrapper to carry out database functions.</param>
	public adminManager(IDal dal) {
		_dal = dal;
	}

	///<summary>
	///Prompts the user for data to create a new account, then runs createNewAccount to execute.
	///</summary>
	public void createNewAccountWrapper() {
		Console.Clear();

		try {
			Console.Write("Login:");
			string? l = Console.ReadLine(); if (l == null) { l = ""; }

			Console.Write("PIN:");
			int p = Convert.ToInt32(Console.ReadLine());
			if (p < 0 || p > 99999) { Console.WriteLine("PIN must be five numbers"); return; }
			Console.Write("Holder's name:");
			string? h = Console.ReadLine(); if (h == null) { h = ""; }

			Console.Write("Starting Balance:");
			double b = Convert.ToDouble(Console.ReadLine());

			string? a = "";
			while (a != "y" && a != "n") {
				Console.Write("Active? (Y/N):");
				a = Console.ReadLine(); if (a == null) { a = ""; }
				a = a.ToLower();
			}

			createNewAccount(h, l, p, b, a);
		}
		catch {
			Console.WriteLine("Account Creation Failed.");
		}
	}

	///<summary>
	///Uses the data entered to create a new account in the dal.
	///</summary>
	///<param name="h">Holder name</param>
	///<param name="l">Login username</param>
	///<param name="p">Pin number</param>
	///<param name="b">Balance</param>
	///<param name="a">Active status</param>
	public void createNewAccount(string h, string l, int p, double b, string a) {
		int id = _dal.createAccount(h, l, p, b, a == "y");
		Console.WriteLine("Account " + id + " Successfully Created.");
	}

	///<summary>
	///Prompts the user for an account's ID to delete, then runs deleteAccount to execute.
	///</summary>
	public void deleteAccountWrapper() {
		Console.Clear();
		Console.Write("Enter the account number to which you want to delete:");
		try {
			int num = Convert.ToInt32(Console.ReadLine());

			DataTable dt = _dal.searchID(num);

			if (dt.Rows.Count == 0) {
				Console.WriteLine("Account Not Found");
				return;
			}

			string? s = dt.Rows[0]["holder"].ToString();
			if (s == null) { s = ""; }

			Console.Write("You are about to delete the account of " + s + ".\nRepeat the account number to proceed.");
			int num2 = Convert.ToInt32(Console.ReadLine());

			deleteAccount(num, num2);
		}
		catch {
			Console.Clear();
			Console.WriteLine("Delete Account failed.");
			return;
		}
	}

	///<summary>
	///Uses the data entered to delete an account by id number in the dal, checking for matching inputs to ensure intent.
	///</summary>
	public bool deleteAccount(int num, int num2) {
		if (num == num2) {
			int del = _dal.deleteAccount(num);

			if (del == 0) { Console.WriteLine("Account not found."); return false; }

			Console.WriteLine("Account Deleted Successfully.");
			return true;
		}
		else {
			Console.WriteLine("No match.");
			return false;
		}
	}

	/// <summary>
	/// Prompts the user for an account number and then for an attribute to change about the account.
	/// </summary>
	public void updateAccount() {
		Console.Clear();
		Console.Write("Enter the Account Number:");
		int id = Convert.ToInt32(Console.ReadLine());

		bool done = false;

		while (!done) {

			try {
				DataTable dt = _dal.searchID(id);
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
					case "1": updateHolderWrapper(id, h, l, (int)p, s); break;
					case "2": updateLoginWrapper(id, h, l, (int)p, s); break;
					case "3": updatePinWrapper(id, h, l, (int)p, s); break;
					case "4": updateStatusWrapper(id, h, l, (int)p, s); break;
					case "5": Console.WriteLine("Exiting Update Mode."); done = true; break;
					default: Console.WriteLine("Invalid input. Try again."); break;
				}

				Console.Clear();
			}
			catch {
				Console.WriteLine("Account not found.");
				return;
			}
		}


	}

	/// <summary>
	///	Prompts the user to enter an account id number, then calls searchForAccount to get the information about the account.
	/// </summary>
	public void searchWrapper() {
		Console.Clear();
		bool done = false;

		while (!done) {
			Console.Write("Enter Account Number:");
			try {
				int val = Convert.ToInt32(Console.ReadLine());
				done = searchForAccount(val);
			}
			catch {
				Console.Clear();
				Console.WriteLine("Search ID must be a number. Please try again.\n");
			}

		}
	}

	/// <summary>
	/// Prints data about an account after getting its information from the dal.
	/// </summary>
	/// <param name="val">The account ID.</param>
	/// <returns>A boolean to denote success.</returns>
	public bool searchForAccount(int val) {
		DataTable dt = _dal.searchID(val);

		if (dt.Rows.Count == 0) {
			Console.WriteLine("No matching account found.\n");
			return false;
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

		return true;
	}

	/// <summary>
	/// Updates the holder name of the account using the dal.
	/// </summary>
	/// <param name="id">The ID of the account</param>
	/// <param name="newHolder">New holder name</param>
	/// <param name="login">Existing login name</param>
	/// <param name="pin">Existing pin password</param>
	/// <param name="status">Existing activity status</param>
	/// <returns>A boolean to denote success.</returns>
	public bool updateHolder(int id, string newHolder, string login, int pin, bool status) {
		if (newHolder == null) {
			Console.WriteLine("Invalid input, please try again."); return false;
		}


		int s = _dal.updateAccount(id, newHolder, login, pin, status);
		if (s == 0) { Console.WriteLine("ID Not Found"); return false; }

		return true;
	}

	/// <summary>
	/// Prompts the user for a new holder name for the selected account and executes with updateHolder.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="holder"></param>
	/// <param name="login"></param>
	/// <param name="pin"></param>
	/// <param name="status"></param>
	public void updateHolderWrapper(int id, string holder, string login, int pin, bool status) {
		bool done = false;

		while (!done) {
			Console.Write("Enter new holder name:");
			string? newHolder = Console.ReadLine();
			done = updateHolder(id, newHolder, login, pin, status);
		}
	}

	/// <summary>
	/// Updates the login username of the account using the dal.
	/// </summary>
	/// <param name="id">The ID of the account</param>
	/// <param name="holder">Existing holder name</param>
	/// <param name="login">New login username</param>
	/// <param name="pin">Existing pin password</param>
	/// <param name="status">Existing activity status</param>
	/// <returns>A boolean to denote success.</returns>
	public bool updateLogin(int id, string holder, string login, int pin, bool status) {
		if (login == null) {
			Console.WriteLine("Invalid input, please try again."); return false;
		}

		int s = _dal.updateAccount(id, holder, login, pin, status);
		if (s == 0) { Console.WriteLine("ID Not Found"); return false; }
		return true;
	}

	/// <summary>
	/// Prompts the user for a new login for the selected account and executes with updateLogin.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="holder"></param>
	/// <param name="login"></param>
	/// <param name="pin"></param>
	/// <param name="status"></param>
	public void updateLoginWrapper(int id, string holder, string login, int pin, bool status) {
		bool done = false;

		while (!done) {
			Console.Write("Enter new login name:");
			string? newLogin = Console.ReadLine();
			done = updateLogin(id, holder, newLogin, pin, status);
		}
	}

	/// <summary>
	/// Updates the PIN of the account using the dal.
	/// </summary>
	/// <param name="id">The ID of the account</param>
	/// <param name="holder">Existing holder name</param>
	/// <param name="login">Existing login username</param>
	/// <param name="pin">New pin password</param>
	/// <param name="status">Existing activity status</param>
	/// <returns>A boolean to denote success.</returns>
	public bool updatePIN(int id, string holder, string login, int pin, bool status) {
		if (pin < 0 || pin > 99999) { Console.WriteLine("Invalid PIN."); return false; }

		int s = _dal.updateAccount(id, holder, login, pin, status);
		if (s == 0) { Console.WriteLine("ID Not Found"); return false; }
		return true;
	}

	/// <summary>
	/// Prompts the user for a new pin for the selected account and executes with updatePIN.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="holder"></param>
	/// <param name="login"></param>
	/// <param name="pin"></param>
	/// <param name="status"></param>
	public void updatePinWrapper(int id, string holder, string login, int pin, bool status) {
		bool done = false;

		while (!done) {
			Console.Write("Enter new pin:");
			int newPin = Convert.ToInt32(Console.ReadLine());
			done = updatePIN(id, holder, login, newPin, status);
		}
	}

	/// <summary>
	/// Updates the activity status of the account using the dal.
	/// </summary>
	/// <param name="id">The ID of the account</param>
	/// <param name="holder">Existing holder name</param>
	/// <param name="login">Existing login username</param>
	/// <param name="pin">Existing pin password</param>
	/// <param name="status">Existing activity status</param>
	/// <param name="newStatus">The new status, represented by 'y' for active and 'n' for disabled.</param>
	/// <returns>A boolean to denote success.</returns>
	public bool updateStatus(int id, string holder, string login, int pin, bool status, string newStatus) {
		if (newStatus == null) {
			Console.WriteLine("Invalid input, please try again."); return false;
		}
		newStatus = newStatus.ToLower();

		if (newStatus != "n" && newStatus != "y") {
			Console.WriteLine("Invalid input, please try again."); return false;
		}

		int s = _dal.updateAccount(id, holder, login, pin, (newStatus == "y"));
		if (s == 0) { Console.WriteLine("ID Not Found"); return false; }

		return true;
	}

	/// <summary>
	/// Prompts the user for a new status for the selected account.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="holder"></param>
	/// <param name="login"></param>
	/// <param name="pin"></param>
	/// <param name="status"></param>
	public void updateStatusWrapper(int id, string holder, string login, int pin, bool status) {
		bool done = false;

		while (!done) {
			Console.Write("Is active? (Y/N):");
			string? newStatus = Console.ReadLine();
			done = updateStatus(id, holder, login, pin, status, newStatus);
		}
	}

	/// <summary>
	/// Shows and prompts the UI menu for the admin manager.
	/// </summary>
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
				case "1": createNewAccountWrapper(); break;
				case "2": deleteAccountWrapper(); break;
				case "3": updateAccount(); break;
				case "4": searchWrapper(); break;
				case "6": break;
				default: Console.WriteLine("Invalid Input, Please Try Again"); break;
			}
		}
	}

}
