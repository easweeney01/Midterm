namespace login;

using System.Data;
using System.Data.Common;

public class LoginManager {

    public static int getLogin() {

        Console.WriteLine("Please Enter Login Name:");
        string? login = Console.ReadLine();

        Console.WriteLine("Enter Pin:");
        string? pinS = Console.ReadLine();


        try {
            int pin = Convert.ToInt32(pinS);            
        } catch {
            Console.WriteLine("Sorry, PINs are numbers-only. Please try again.");
            return -1;
        }

        return 0; //This number will correspond to the actual number that it should get
    }
}