using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Project;

class Program
{
    static void Main(string[] args)
    {
        Directory.CreateDirectory("Accounts");

        WelcomeMenu();
    }

    public static string Green(string color)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(color);
        Console.ResetColor();
        return color;
    }

    public static string Red(string color)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(color);
        Console.ResetColor();
        return color;
    }

    public static string Yellow(string color)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(color);
        Console.ResetColor();
        return color;
    }

    public static void ClearOutputs()
    {
        Yellow("Press Any Key To Continue");
        Console.ReadKey(true);
        Console.Clear();
    }

    public static string NameTrim(string name)
    {
        name = name.Trim();
        string[] splitName = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        name = String.Join(' ', splitName);
        name = name.ToLower();
        return name;
    }

    public static string HidePin()
    {
        string pin = "";
        bool isRunning = true;

        while (isRunning)
        {

            if (pin.Length == 4 && pin.All(char.IsDigit))
            {
                isRunning = false;
                return pin;
            }

            ConsoleKeyInfo key = Console.ReadKey(true);
            char c = key.KeyChar;

            if (key.Key == ConsoleKey.Backspace && pin.Length > 0)
            {
                pin = pin.Substring(0, pin.Length - 1);
                Console.Write("\b \b");
            }
            else if (char.IsDigit(c))
            {
                pin += c;
                Console.Write("*");
            }
            else if (!char.IsDigit(c))
            {
                Console.WriteLine();
                Red("Enter Valid 4 Digits!");
                isRunning = false;
                ClearOutputs();
            }
        }
        return pin;
    }

    public static void WelcomeMenu()
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            Green("---WELCOME TO ATM---");
            Console.WriteLine("[1] Login Account.");
            Console.WriteLine("[2] Create Account.");
            Console.WriteLine("[3] Exit.");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    isRunning = false;
                    LoginAccount();
                    break;

                case "2":
                    isRunning = false;
                    CreateAccount();
                    break;

                case "3":
                    isRunning = false;
                    break;

                default:
                    Red("Wrong Input!");
                    ClearOutputs();
                    break;
            }
        }
    }

    public static void LoginAccount()
    {
        Console.Clear();
        bool isRunning = true;

        while (isRunning)
        {
            string accountName;
            string accountPin = "";
            int attempts = 3;

            Green("---Login Account---");
            Yellow("   [Attempts " + attempts + "]");
            Console.WriteLine();
            Console.Write("Enter Account Name: ");
            accountName = NameTrim(Console.ReadLine());

            string accountFolder = Path.Combine("Accounts", accountName);
            if (Directory.Exists(accountFolder))
            {
                string transactionHistoryPath = Path.Combine(accountFolder, "transaction_history.txt");
                string accountBalancePath = Path.Combine(accountFolder, "balance.txt");
                string accountPinPath = Path.Combine(accountFolder, "pin.txt");
                string loginPin = File.ReadAllText(accountPinPath);

                while (attempts > 0)
                {
                    if (attempts > 0)
                    {
                        Console.Clear();
                        Green("---Login Account---");
                        Yellow("   [Attempts " + attempts + "]");
                        Console.WriteLine();
                        Console.Write("Enter Account Name: " + accountName);
                        Console.WriteLine();
                        Console.Write("Enter Account Pin (4-Digits): ");
                        accountPin = HidePin();
                    }

                    if (accountPin.Length != 4)
                    {
                        attempts--;

                        if (attempts <= 0)
                        {
                            Red("Attempts exceeded!");
                            Console.WriteLine();
                            Yellow("Loading...");
                            Thread.Sleep(2000);
                            isRunning = false;
                            WelcomeMenu();
                        }
                    }
                    else if (accountPin.Length == 4 && loginPin != accountPin)
                    {
                        Console.WriteLine();
                        Yellow("Verifying your information... Please wait.");
                        Thread.Sleep(3000);
                        attempts--;
                        Console.WriteLine();
                        Red("Incorrect Pin!");
                        ClearOutputs();

                        if (attempts <= 0)
                        {
                            Red("Attempts exceeded!");
                            Console.WriteLine();
                            Yellow("Loading...");
                            Thread.Sleep(2000);
                            isRunning = false;
                            WelcomeMenu();
                        }
                    }
                    else
                    {
                        attempts = 0;
                        isRunning = false;
                        Console.WriteLine();
                        Yellow("Verifying your information... Please wait.");
                        Thread.Sleep(3000);
                        Green("Access Granted!");
                        Thread.Sleep(1500);
                        MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                    }
                }
            }
            else
            {
                Red("This Account Is Not Exists!");
                Console.WriteLine();
                Yellow("Create Account? (Y/N)");
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    Yellow("Loading...");
                    Thread.Sleep(1000);
                    CreateAccount();
                    break;
                }
                else
                {
                    Console.Clear();
                }
            }
        }
    }

    public static void CreateAccount()
    {
        Console.Clear();
        bool isRunning = true;

        while (isRunning)
        {

            string accountName;
            string accountPin = "";
            int attempts = 3;

            Green("---Create Account---");
            Yellow("    [Attempts " + attempts + "]");
            Console.WriteLine();
            Console.Write("Create Account Name: ");
            accountName = NameTrim(Console.ReadLine());

            string accountFolder = Path.Combine("Accounts", accountName);
            if (!Directory.Exists(accountFolder))
            {
                while (attempts > 0)
                {
                    if (attempts > 0)
                    {
                        Console.Clear();
                        Green("---Create Account---");
                        Yellow("    [Attempts " + attempts + "]");
                        Console.WriteLine();
                        Console.Write("Create Account Name: " + accountName);
                        Console.WriteLine();
                        Console.Write("Create Account Pin (4-Digits): ");
                        accountPin = HidePin();
                    }

                    if (accountPin.Length != 4)
                    {
                        attempts--;

                        if (attempts <= 0)
                        {
                            Red("Attempts exceeded!");
                            Console.WriteLine();
                            Yellow("Loading...");
                            Thread.Sleep(2000);
                            isRunning = false;
                            WelcomeMenu();
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        Yellow("Creating account... Please wait.");
                        Thread.Sleep(3000);
                        attempts = 0;
                        isRunning = false;
                        Directory.CreateDirectory(accountFolder);
                        string accountPinPath = Path.Combine(accountFolder, "pin.txt");
                        string transactionHistoryPath = Path.Combine(accountFolder, "transaction_history.txt");
                        string accountBalancePath = Path.Combine(accountFolder, "balance.txt");

                        File.WriteAllText(accountPinPath, accountPin);
                        File.WriteAllText(transactionHistoryPath, "");
                        File.WriteAllText(accountBalancePath, "500");
                        Green("Account Created!");
                        Thread.Sleep(1500);
                        MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                    }
                }
            }
            else
            {
                Red("This Account Already Exists!");
                ClearOutputs();
            }
        }
    }

    public static void MainMenu(string accountName, string transactionHistoryPath, string accountBalancePath, string accountPinPath)
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            Green("---MENU---");
            Console.WriteLine("[1] Check Balance.");
            Console.WriteLine("[2] Deposit Balance.");
            Console.WriteLine("[3] Withdraw Balance.");
            Console.WriteLine("[4] Transfer Balance.");
            Console.WriteLine("[5] Change Account PIN.");
            Console.WriteLine("[6] Transaction History.");
            Console.WriteLine("[7] Clear Transaction History.");
            Console.WriteLine("[8] Logout.");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    isRunning = false;
                    CheckBalance(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                    break;

                case "2":
                    isRunning = false;
                    Deposit(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                    break;

                case "3":
                    isRunning = false;
                    Withdraw(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                    break;

                case "4":
                    isRunning = false;
                    TransferBalance(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                    break;

                case "5":
                    isRunning = false;
                    Green("Knock Knock");
                    break;

                case "6":
                    isRunning = false;
                    Green("Heyy");
                    break;

                case "7":
                    isRunning = false;
                    Green("Bruhh");
                    break;

                case "8":
                    isRunning = false;
                    Yellow("Logging out...");
                    Thread.Sleep(2000);
                    WelcomeMenu();
                    break;

                default:
                    Red("Wrong Input!");
                    ClearOutputs();
                    break;
            }
        }
    }

    public static void CheckBalance(string accountName, string transactionHistoryPath, string accountBalancePath, string accountPinPath)
    {
        string balance = File.ReadAllText(accountBalancePath);

        Console.Clear();
        Green("---Balance Inquiry---");
        Console.WriteLine("Account Holder: " + accountName);
        Console.Write("Account Balance: $");
        Green(balance);
        Console.Write("Account Status: ");
        Green("Active");
        ClearOutputs();
        MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
    }

    public static void Deposit(string accountName, string transactionHistoryPath, string accountBalancePath, string accountPinPath)
    {
        string time = DateTime.Now.ToString("HH:mm");
        string balanceFile = File.ReadAllText(accountBalancePath);
        if (int.TryParse(balanceFile, out int balance))
        {
            bool isRunning = true;
            while (isRunning)
            {
                string accountPin = File.ReadAllText(accountPinPath);
                string confirmPin = "";
                int attempts = 3;

                Console.Clear();
                Green("---Deposit Balance---");
                Yellow("     [Attempts " + attempts + "]");
                Console.WriteLine();
                Console.Write("Enter an amount for deposit: $");
                if (int.TryParse(Console.ReadLine(), out int depositAmount))
                {
                    if (depositAmount < 5)
                    {
                        Console.WriteLine();
                        Red("The Minimum Deposit Amount is 5$!");
                        ClearOutputs();
                    }
                    else
                    {
                        while (attempts > 0)
                        {
                            if (attempts > 0)
                            {
                                Console.Clear();
                                Green("---Deposit Balance---");
                                Yellow("     [Attempts " + attempts + "]");
                                Console.WriteLine();
                                Console.Write("Enter an amount for deposit: $" + depositAmount);
                                Console.WriteLine();
                                Console.Write("Confirm your PIN for this deposit: ");
                                confirmPin = HidePin();
                            }

                            if (confirmPin.Length != 4)
                            {
                                attempts--;

                                if (attempts <= 0)
                                {
                                    Red("Attempts exceeded!");
                                    Console.WriteLine();
                                    Yellow("Loading...");
                                    Thread.Sleep(2000);
                                    isRunning = false;
                                    MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                                }
                            }
                            else if (confirmPin.Length == 4 && confirmPin != accountPin)
                            {
                                Console.WriteLine();
                                Yellow("Verifying your deposit... Please wait.");
                                Thread.Sleep(3000);
                                attempts--;
                                Console.WriteLine();
                                Red("Incorrect Pin!");
                                ClearOutputs();

                                if (attempts <= 0)
                                {
                                    Red("Attempts exceeded!");
                                    Console.WriteLine();
                                    Yellow("Loading...");
                                    Thread.Sleep(2000);
                                    isRunning = false;
                                    MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                                }
                            }
                            else
                            {
                                attempts = 0;
                                isRunning = false;
                                Console.WriteLine();
                                Yellow("Verifying your deposit... Please wait.");
                                Thread.Sleep(3000);
                                Green("Deposit Successful!");
                                balance += depositAmount;
                                string finalBalance = balance.ToString();
                                File.WriteAllText(accountBalancePath, finalBalance);
                                File.AppendAllText(transactionHistoryPath, "[Deposit] Deposited $" + depositAmount + " at <" + time + "> | Updated Balance: $" + balance + Environment.NewLine);
                                Thread.Sleep(1500);
                                MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine();
                    Red("Enter Valid Digits!");
                    ClearOutputs();
                }
            }
        }
        else
        {
            Red("Something Went Wrong!");
            ClearOutputs();
            MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
        }
    }

    public static void Withdraw(string accountName, string transactionHistoryPath, string accountBalancePath, string accountPinPath)
    {
        string time = DateTime.Now.ToString("HH:mm");

        string balanceFile = File.ReadAllText(accountBalancePath);
        if (int.TryParse(balanceFile, out int balance))
        {
            bool isRunning = true;
            while (isRunning)
            {
                string accountPin = File.ReadAllText(accountPinPath);
                string confirmPin = "";
                int attempts = 3;

                Console.Clear();
                Green("---Withdraw Balance---");
                Yellow("    [Attempts " + attempts + "]");
                Console.WriteLine();
                Console.Write("Enter an amount for withdrawal: $");
                if (int.TryParse(Console.ReadLine(), out int withdrawAmount))
                {
                    if (withdrawAmount < 5)
                    {
                        Console.WriteLine();
                        Red("The Minimum Withdrawal Amount is 5$!");
                        attempts--;
                        ClearOutputs();
                    }
                    else if (withdrawAmount > balance)
                    {
                        Console.WriteLine();
                        Red("Insufficient Balance For This Transaction!");
                        attempts--;
                        ClearOutputs();
                    }
                    else
                    {
                        while (attempts > 0)
                        {
                            if (attempts > 0)
                            {
                                if (withdrawAmount > 5 && withdrawAmount <= balance)
                                {
                                    Console.Clear();
                                    Green("---Withdraw Balance---");
                                    Yellow("    [Attempts " + attempts + "]");
                                    Console.WriteLine();
                                    Console.Write("Enter an amount for withdrawal: $" + withdrawAmount);
                                    Console.WriteLine();
                                    Console.Write("Confirm your PIN for this withdrawal: ");
                                    confirmPin = HidePin();
                                }

                                if (confirmPin.Length != 4)
                                {
                                    attempts--;

                                    if (attempts <= 0)
                                    {
                                        Red("Attempts exceeded!");
                                        Console.WriteLine();
                                        Yellow("Loading...");
                                        Thread.Sleep(2000);
                                        isRunning = false;
                                        MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                                    }
                                }
                                else if (confirmPin.Length == 4 && confirmPin != accountPin)
                                {
                                    Console.WriteLine();
                                    Yellow("Verifying your withdrawal... Please wait.");
                                    Thread.Sleep(3000);
                                    attempts--;
                                    Console.WriteLine();
                                    Red("Incorrect Pin!");
                                    ClearOutputs();

                                    if (attempts <= 0)
                                    {
                                        Red("Attempts exceeded!");
                                        Console.WriteLine();
                                        Yellow("Loading...");
                                        Thread.Sleep(2000);
                                        isRunning = false;
                                        MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                                    }
                                }
                                else
                                {
                                    attempts = 0;
                                    isRunning = false;
                                    Console.WriteLine();
                                    Yellow("Verifying your withdrawal... Please wait.");
                                    Thread.Sleep(3000);
                                    Green("Withdraw Successful!");
                                    balance -= withdrawAmount;
                                    string finalBalance = balance.ToString();
                                    File.WriteAllText(accountBalancePath, finalBalance);
                                    File.AppendAllText(transactionHistoryPath, "[Withdraw] Withdrew $" + withdrawAmount + " at <" + time + "> | Updated Balance: $" + balance + Environment.NewLine);
                                    Thread.Sleep(1500);
                                    MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                                }
                            }
                            else
                            {
                                Red("Attempts exceeded!");
                                Console.WriteLine();
                                Yellow("Loading...");
                                Thread.Sleep(2000);
                                isRunning = false;
                                MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                            }
                        }
                    }

                }
                else
                {
                    Console.WriteLine();
                    Red("Enter Valid Digits!");
                    ClearOutputs();
                }
            }
        }
        else
        {
            Red("Something Went Wrong!");
            ClearOutputs();
            MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
        }
    }

    public static void TransferBalance(string accountName, string transactionHistoryPath, string accountBalancePath, string accountPinPath)
    {
        string time = DateTime.Now.ToString("HH:mm");

        string balanceFile = File.ReadAllText(accountBalancePath);
        if (int.TryParse(balanceFile, out int balance))
        {
            bool isRunning = true;
            while (isRunning)
            {
                string accountPin = File.ReadAllText(accountPinPath);
                string confirmPin = "";
                int attempts = 3;

                Console.Clear();
                Green("---Transfer Balance---");
                Yellow("     [Attempts " + attempts + "]");
                Console.WriteLine();
                Console.Write("Enter reciever's account name: ");
                string recieverName = NameTrim(Console.ReadLine());

                string recieverNamePath = Path.Combine("Accounts", recieverName);
                if (recieverName == accountName)
                {
                    Red("Sorry! You Cannot Transfer Balance into Your Own Account.");
                    ClearOutputs();
                }
                else if (recieverName == "")
                {
                    Red("Enter An Valid Name!");
                    ClearOutputs();
                }
                else if (Directory.Exists(recieverNamePath))
                {
                    Yellow("Finding the reciever's account...");
                    Console.WriteLine();
                    Thread.Sleep(3000);
                    Green("Account Found!");
                    Thread.Sleep(1000);
                    string recieverBalancePath = Path.Combine("Accounts", recieverName, "balance.txt");
                    if (int.TryParse(File.ReadAllText(recieverBalancePath), out int recieverBalance))
                    {
                        while (attempts > 0)
                        {
                            if (attempts > 0)
                            {
                                Console.Clear();
                                Green("---Transfer Balance---");
                                Yellow("     [Attempts " + attempts + "]");
                                Console.WriteLine();
                                Console.Write("Enter reciever's account name: " + recieverName);
                                Console.WriteLine();
                                Console.Write("Enter an amount to transfer: $");

                                if (int.TryParse(Console.ReadLine(), out int transferAmount))
                                {
                                    if (transferAmount < 10)
                                    {
                                        Console.WriteLine();
                                        Red("The Minimum Transfer Amount is 10$!");
                                        ClearOutputs();
                                    }
                                    else if (transferAmount > balance)
                                    {
                                        Console.WriteLine();
                                        Red("Insufficient Balance For This Transaction!");
                                        attempts--;
                                        ClearOutputs();
                                    }
                                    else
                                    {
                                        Console.WriteLine();
                                        Console.Write("Confirm your PIN for this transfer: ");
                                        confirmPin = HidePin();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine();
                                    Red("Enter Valid Digits!");
                                    ClearOutputs();
                                }
                                if (confirmPin.Length != 4)
                                {
                                    attempts--;

                                    if (attempts <= 0)
                                    {
                                        Red("Attempts exceeded!");
                                        Console.WriteLine();
                                        Yellow("Loading...");
                                        Thread.Sleep(2000);
                                        isRunning = false;
                                        MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                                    }
                                }
                                else if (confirmPin.Length == 4 && confirmPin != accountPin)
                                {
                                    Console.WriteLine();
                                    Yellow("Verifying your information...");
                                    Thread.Sleep(3000);
                                    attempts--;
                                    Console.WriteLine();
                                    Red("Incorrect Pin!");
                                    ClearOutputs();

                                    if (attempts <= 0)
                                    {
                                        Red("Attempts exceeded!");
                                        Console.WriteLine();
                                        Yellow("Loading...");
                                        Thread.Sleep(2000);
                                        isRunning = false;
                                        MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                                    }
                                }
                                else
                                {
                                    string recieverHistoryPath = Path.Combine("Accounts", recieverName, "transaction_history.txt");
                                    attempts = 0;
                                    isRunning = false;
                                    Console.WriteLine();
                                    Yellow("Verifying your information...");
                                    Thread.Sleep(3000);
                                    Green("Transfer Balance Successful!");
                                    balance -= transferAmount;
                                    recieverBalance += transferAmount;
                                    string finalBalance = balance.ToString();
                                    File.WriteAllText(accountBalancePath, finalBalance);
                                    File.AppendAllText(transactionHistoryPath, "[Transfer] Transfered $" + transferAmount + " at <" + time + "> to " + recieverName + " | Updated Balance: $" + balance + Environment.NewLine);
                                    File.AppendAllText(recieverHistoryPath, "[Recieve] Recieved $" + transferAmount + " at <" + time + "> from " + accountName + " | Updated Balance: $" + recieverBalance + Environment.NewLine);
                                    Thread.Sleep(1500);
                                    MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                                }
                            }
                            else
                            {
                                Console.WriteLine();
                                Red("Enter Valid Digits!");
                                ClearOutputs();

                            }
                        }
                    }
                    else
                    {
                        Red("Something Went Wrong!");
                        ClearOutputs();
                        MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
                    }
                }
                else
                {
                    Yellow("Finding the reciever's account...");
                    Console.WriteLine();
                    Thread.Sleep(3000);
                    Red("Sorry! Could'nt Find This Account.");
                    ClearOutputs();
                }
            }
        }
        else
        {
            Red("Something Went Wrong!");
            ClearOutputs();
            MainMenu(accountName, transactionHistoryPath, accountBalancePath, accountPinPath);
        }
    }
}