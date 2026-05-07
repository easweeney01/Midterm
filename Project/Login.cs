namespace login;

using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using dal;

public class LoginManager {
	private readonly IDal _dal;

	[ExcludeFromCodeCoverage]
	public LoginManager(IDal dal) {
		//Use dal to get other info
		_dal = dal;
	}

	[ExcludeFromCodeCoverage]
	public LoginManager() {
		_dal = new DalWrapper();
	}

	/// <summary>
	/// Logs the user in with a valid login name and password.
	/// </summary>
	/// <returns>An int representing user ID.</returns>
	[ExcludeFromCodeCoverage]
	public int getLogin() {
		Console.WriteLine("Please Enter Login Name:");
		string? login = Console.ReadLine();
		Console.WriteLine("Enter Pin:");
		string? pinS = Console.ReadLine();

		try {
			var log = tryLogin(login, pinS);
			if (log == -1) { Console.Clear(); Console.WriteLine("Sorry, please try again."); }
			return log;
		}
		catch (FormatException) {
			Console.Clear(); Console.WriteLine("Sorry, PINs are numbers-only. Please try again.");
			return -1;
		}
	}

	/// <summary>
	/// Does the business logic of logging in.
	/// </summary>
	/// <param name="login"></param>
	/// <param name="pinS">The string of the PIN</param>
	/// <returns></returns>
	public int tryLogin(string login, string pinS) {

		int pin = Convert.ToInt32(pinS);

		if (login != null) {
			int? ret = _dal.login(login, pin);
			if (ret == null) {
				return -1;
			}

			return ret.Value;
		}


		return -1;
	}
}
