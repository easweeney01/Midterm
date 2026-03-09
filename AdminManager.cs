namespace AdminManager;

public class adminManager {
    public adminManager() {}

    public void createNewAccount() {
        
    }

    public void deleteAccount() {
        
    }

    public void updateAccount() {
        
    }

    public int searchForAccount() {
        Console.WriteLine("Enter Account Number");
        try
        {
            int val = Convert.ToInt32(Console.ReadLine());    
        } catch
        {
            Console.WriteLine("Must be a number. Please try again");
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
                case "1": break;
                case "2": break;
                case "3": break;
                case "4": break;
            }
        }
    }
}