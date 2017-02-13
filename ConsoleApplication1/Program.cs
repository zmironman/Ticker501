using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        public static IList<Stock> Stocks = new List<Stock>();
        static double TRADEFEE = 9.99;
        static double TRANSFERFEE = 4.99;
        static int MAXPORTFOLIOS = 3;

        /// <summary>
        /// the initialization of the program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try {
                Console.Write("Please enter a starting balance for the Account: ");
                double starting = Convert.ToDouble(Console.ReadLine());
                Account account = new Account(Convert.ToDouble(starting.ToString("F")));
                Console.Write("Please enter a Ticker File path: ");
                string file = Console.ReadLine();
                ReadFile(new StreamReader(file));
                MainMenu(account);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Main menu for the application
        /// </summary>
        /// <param name="account">User's account</param>
        static void MainMenu(Account account)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("Would you like to:");
                Console.WriteLine("1. Go to account funds");
                Console.WriteLine("2. Go to account balance");
                Console.WriteLine("3. Go to Portfolio");
                Console.WriteLine("4. Buy/Sell a stock");
                Console.WriteLine("5. Create Portfolio");
                Console.WriteLine("6. Delete Portfolio");
                Console.WriteLine("7. Simulate an event");
                Console.WriteLine("8. Show stocks available");
                Console.WriteLine("9. Exit");
                Console.Write("Please choose a number (1-9): ");
                string answer = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine();
                if (answer == "1")
                {
                    AccountFunds(account);
                }
                else if (answer == "2")
                {
                    AccountBalance(account);
                }
                else if (answer == "3")
                {
                    PortfolioBalance(account);
                }
                else if(answer == "4")
                {
                    BuyStock(account);
                }
                else if (answer == "5")
                {
                    CreatePortfolio(account);
                }
                else if (answer == "6")
                {
                    DeletePortfolio(account);
                }
                else if(answer == "7")
                {
                    Simulate(account);
                }
                else if(answer == "8")
                {
                    ShowStocks();
                }
                else if(answer == "9")
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Invalid answer, please try again");
                }
            }
        }

        /// <summary>
        /// Asks what the user wants to do with the account Funds and does said task
        /// </summary>
        /// <param name="account">user's account</param>
        static void AccountFunds(Account account)
        {
            string answer;
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Would you like to:");
                Console.WriteLine("1. Add funds to the account");
                Console.WriteLine("2. Withdraw funds from the account");
                Console.WriteLine("3. Return to Main Menu");
                Console.Write("Please choose a number (1-3): ");
                answer = Console.ReadLine();
                Console.WriteLine();
                if (answer == "1")
                {
                    Console.Write("How much are you depositing: ");
                    account.AddFunds(Convert.ToDouble(Console.ReadLine()));
                    Console.WriteLine("Deposit Successful");
                }
                else if (answer == "2")
                {
                    Console.Write("How much are you withdrawing: ");
                    if (account.Withdraw(Convert.ToDouble(Console.ReadLine())))
                    {
                        Console.WriteLine("Withdraw successful");
                    }
                    else
                    {
                        Console.WriteLine("Invalid answer.");
                    }
                }
                else if(answer == "3")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid Answer.");
                }
            }
        }

        /// <summary>
        /// Asks what the user wants to do with the account balance and does said task
        /// </summary>
        /// <param name="account">user's account</param>
        static void AccountBalance(Account account)
        {
            string answer;
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Would you like to:");
                Console.WriteLine("1. See the account balance");
                Console.WriteLine("2. see the cash positions balance");
                Console.WriteLine("3. See a Gain/Losses Report");
                Console.WriteLine("4. Return to Main Menu");
                Console.Write("Please choose a number (1-4): ");
                Console.WriteLine();
                answer = Console.ReadLine();
                Console.WriteLine();
                if (answer == "1")
                {
                    account.CheckBal();
                }
                else if (answer == "2")
                {
                    account.CheckPosBal();
                }
                else if (answer == "3")
                {
                    Console.WriteLine();
                    account.GenerateReport();
                }
                else if (answer == "4")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid Answer.");
                }
            }
        }

        /// <summary>
        /// asks what the user wants to do with the porfolio balance and does said task
        /// </summary>
        /// <param name="account">user's account</param>
        static void PortfolioBalance(Account account)
        {
            bool exit = false;
            bool exit2 = false;
            if (account.portfolios.Count < 1)
            {
                Console.WriteLine("You dont have any portfolios");
                return;
            }
            while (!exit2)
            {
                string answer;
                foreach (Portfolio po in account.portfolios)
                {
                    Console.WriteLine(po.Name);
                }
                Console.Write("Please choose a portfolio(Case sensitive): ");
                answer = Console.ReadLine();
                Console.WriteLine();
                Portfolio p = account.GetPortfolio(answer);
                if (p.Name == "error")
                {
                    Console.WriteLine("Invalid Portfolio");
                    return;
                }
                exit = false;
                while (!exit)
                {
                    Console.WriteLine("Would you like to:");
                    Console.WriteLine("1. See the portfolio balance");
                    Console.WriteLine("2. see the cash and positions balance");
                    Console.WriteLine("3. See a Gain/Losses Report (unavailable)");
                    Console.WriteLine("4. Choose another portfolio");
                    Console.WriteLine("5. Return to Main Menu");
                    Console.Write("Please choose a number (1-5): ");
                    Console.WriteLine();
                    answer = Console.ReadLine();
                    Console.WriteLine();
                    if (answer == "1")
                    {
                        p.CheckBal(account.Total);
                    }
                    else if (answer == "2")
                    {
                        p.CheckPosBal();
                    }
                    else if (answer == "3")
                    {
                        p.GenerateReport();
                    }
                    else if (answer == "4")
                    {
                        exit = true;
                    }
                    else if (answer == "5")
                    {
                        exit2 = true;
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Answer.");
                    }
                }
            }
        }

        /// <summary>
        /// Creates a portfolio with the name given by user
        /// </summary>
        /// <param name="account">user's account</param>
        static void CreatePortfolio(Account account)
        {
            if(account.portfolios == null)
            {
                Console.Write("Please enter a name for the new portfolio: ");
                string answer = Console.ReadLine();
                Console.WriteLine();
                account.NewPortfolio(answer);
            }
            if(account.portfolios.Count < 3)
            {
                Console.Write("Please enter a name for the new portfolio: ");
                string answer = Console.ReadLine();
                Console.WriteLine();
                account.NewPortfolio(answer);
            }
            else
            {
                Console.WriteLine("You already have the maximum number of portfolios");
            }    
        }

        /// <summary>
        /// Deletes the portfolio with the name given by user
        /// </summary>
        /// <param name="account">user's account</param>
        static void DeletePortfolio(Account account)
        {
            Console.WriteLine();
            if (account.portfolios.Count > 0)
            { 
                foreach(Portfolio p in account.portfolios)
                {
                    Console.WriteLine(p.Name);
                }
                Console.Write("Please enter a name for the portfolio that is to be deleted: ");
                string answer = Console.ReadLine();
                Console.WriteLine();
                if (account.DelPortfolio(answer))
                {
                    Console.WriteLine("Portfolio Deleted, funds added to you account cash balance");
                }
                else
                {
                    Console.WriteLine("Invalid portfolio name");
                }
            }
            else
            {
                Console.WriteLine("You don't have any portfolios");
            }
        }
        
        /// <summary>
        /// Simulates the update of stock prices based on the users input
        /// </summary>
        /// <param name="account">User's account</param>
        static void Simulate(Account account)
        {
            string answer;
            Random r = new Random();
            int percentage;
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Please choose a volatility");
                Console.WriteLine("1. High(3% - 15%)");
                Console.WriteLine("2. Medium(2% - 8%");
                Console.WriteLine("3. Low(1% - 4%)");
                Console.WriteLine("4. Return to Main Menu");
                Console.Write("Please choose a number (1-4): ");
                answer = Console.ReadLine();
                Console.WriteLine();
                if (answer == "1")
                {
                    foreach (Stock s in Stocks)
                    {
                        percentage = r.Next(0, 30) - 15;
                        if (percentage > -3 && percentage < 0)
                            percentage -= 2;
                        else if (percentage > 0 && percentage < 3)
                            percentage += 2;
                        else if (percentage == 0)
                            percentage += 3;
                        s.Price += (s.Price * percentage / 100);
                        foreach (Portfolio p in account.portfolios)
                        {
                            p.UpdateStock(s, s.Price);
                        }
                    }

                }
                else if (answer == "2")
                {
                    foreach (Stock s in Stocks)
                    {
                        percentage = r.Next(0, 16) - 8;
                        if (percentage > -2 && percentage < 0)
                            percentage -= 1;
                        else if (percentage > 0 && percentage < 2)
                            percentage += 1;
                        else if (percentage == 0)
                            percentage += 2;
                        s.Price += (s.Price * percentage / 100);
                        foreach (Portfolio p in account.portfolios)
                        {
                            p.UpdateStock(s, s.Price);
                        }
                    }
                }
                else if (answer == "3")
                {
                    foreach (Stock s in Stocks)
                    {
                        percentage = r.Next(0, 8) - 4;
                        if (percentage == 0)
                            percentage += 1;
                        s.Price += (s.Price * percentage / 100);
                        foreach (Portfolio p in account.portfolios)
                        {
                            p.UpdateStock(s, s.Price);
                        }
                    }
                }
                else if(answer == "4")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid Answer.");
                }
            }
        }

        /// <summary>
        /// buys the stock the user asks for
        /// </summary>
        /// <param name="account">the account of the user</param>
        static void BuyStock(Account account)
        {
            if (account.portfolios.Count < 1)
            {
                Console.WriteLine("You dont have any portfolios");
                return;
            }
            string answer;
            foreach (Portfolio po in account.portfolios)
            {
                Console.WriteLine(po.Name);
            }
            Console.Write("Please choose a portfolio(Case sensitive): ");
            answer = Console.ReadLine();
            Console.WriteLine();
            Portfolio p = account.GetPortfolio(answer);
            if (p.Name == "error")
            {
                Console.WriteLine("Invalid Portfolio");
                return;
            }
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Would you like to: ");
                Console.WriteLine("1. Enter the number of stocks to purchase");
                Console.WriteLine("2. Enter the amount in dollars to purchase stock represented by ticker");
                Console.WriteLine("3. Sell a stock");
                Console.WriteLine("Return to Main Menu");
                Console.Write("Please pick a number (1-4): ");
                answer = Console.ReadLine();
                if (answer == "1")
                {
                    bool valid = false;
                    Stock s = null;
                    while (!valid)
                    {
                        Console.Write("Enter the Ticker: ");
                        string tick = Console.ReadLine();
                        foreach (Stock st in Stocks)
                        {
                            if (st.Ticker == tick)
                            {
                                s = st;
                                valid = true;
                            }
                        }
                        if (!valid)
                        {
                            Console.WriteLine("Invalid Ticker, please try again");
                        }
                    }
                    valid = false;
                    int amount = 0;
                    while (!valid)
                    {
                        Console.WriteLine("Enter the number of stocks: ");
                        amount = Convert.ToInt32(Console.ReadLine());
                        if (amount * s.Price > account.CashBalance)
                        {
                            valid = false;
                            Console.WriteLine("Insufficent funds");
                        }
                        else if (amount < 1)
                        {
                            valid = false;
                            Console.WriteLine("Invalid number");
                        }
                        else
                        {
                            valid = true;
                        }
                    }
                    p.BuyNumber(amount, s);
                    account.CashBalance -= (amount * s.Price) + TRADEFEE;
                    account.UpdateCash();
                    account.Trades += 1;
                }
                else if (answer == "2")
                {
                    bool valid = false;
                    Stock s = null;
                    while (!valid)
                    {
                        Console.Write("Enter the Ticker: ");
                        string tick = Console.ReadLine();
                        foreach (Stock st in Stocks)
                        {
                            if (st.Ticker == tick)
                            {
                                s = st;
                                valid = true;
                            }
                        }
                        if (!valid)
                        {
                            Console.WriteLine("Invalid Ticker, please try again");
                        }
                    }
                    double amount = 0;
                    double spent = 0;
                    while (!valid)
                    {
                        Console.WriteLine("Enter the amount to spend: ");
                        amount = Convert.ToInt32(Console.ReadLine());
                        if (amount > account.CashBalance)
                        {
                            valid = false;
                            Console.WriteLine("Insufficent funds");
                        }
                        else if (amount < s.Price)
                        {
                            valid = false;
                            Console.WriteLine("Stock price is more expensive");
                        }
                        else
                        {
                            valid = true;
                        }
                    }
                    p.BuyValue(amount, s, out spent);
                    account.CashBalance -= spent;
                    account.UpdateCash();
                    account.Trades += 1;
                }
                else if (answer == "3")
                {
                    if (p.Stocks.Count > 0)
                    {
                        Console.WriteLine("You have: ");
                        Stock prev = p.Stocks[0];
                        int count = 0;
                        int end = 0;
                        foreach (Stock s in p.Stocks)
                        {
                            if (s.Name == prev.Name)
                            {
                                count++;
                            }
                            else
                            {
                                Console.WriteLine(count + " stocks of " + prev.Name + "(" + prev.Ticker + ")");
                                count = 0;
                                prev = s;
                            }
                            end++;
                        }
                        Console.WriteLine(count + 1 + " stocks of " + prev.Name + "(" + prev.Ticker + ")");
                        Console.Write("Please enter a ticker to sell: ");
                        string tick = Console.ReadLine();
                        Console.Write("Please enter a number to sell: ");
                        int num = Convert.ToInt32(Console.ReadLine());
                        double value = 0;
                        Stock stock = p.getStockT(tick);
                        if (stock == null)
                        {
                            Console.WriteLine("You don't have any of that stock in this portfolio.");
                        }
                        else if (p.Sell(num, stock, out value))
                        {
                            Console.WriteLine("You sold " + num + " of your " + p.getStockT(tick).Name + " stocks for $" + value.ToString("f"));
                        }
                        else
                        {
                            Console.WriteLine("You sold all of your " + p.getStockT(tick).Name + " stocks for $" + value.ToString("f"));
                        }
                        account.CashBalance += value - 9.99;
                        account.Trades += 1;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("You don't have any stocks in this portfolio");
                        return;
                    }
                }
                else if(answer == "4")
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }

        static void ShowStocks()
        {
            foreach(Stock s in Stocks)
            {
                Console.WriteLine(s.Ticker + ": " + s.Name + " $" + s.Price.ToString("f"));
            }
        }


        /// <summary>
        /// Reads a file and records the stocks at that time
        /// </summary>
        /// <param name="file">name of the file being read in</param>
        static void ReadFile(StreamReader sr)
        {
            Stocks.Clear();
            StringBuilder sb = new StringBuilder();
            string tempTicker = null;
            string tempName = null;
            double tempValue = 0;
            string current;
            while (!sr.EndOfStream)
            {
                current = sr.ReadLine();
                string[] parts = current.Split('-');
                if (parts.Length > 1)
                {
                    tempTicker = parts[0];
                    tempName = parts[1];
                    string[] partss = parts[2].Split('$');
                    tempValue = Convert.ToDouble(partss[1]);
                    sb.Clear();
                    Stock temp = new Stock(tempTicker, tempName, tempValue);
                    Stocks.Add(temp);
                }
            }
            sr.Close();
        }
    }
}
