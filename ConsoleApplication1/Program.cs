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
            //try {
                Console.Write("Please enter a starting balance for the Account: ");
                double starting = Convert.ToDouble(Console.ReadLine());
                Account account = new Account(Convert.ToDouble(starting.ToString("F")));
                MainMenu(account);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //    Console.ReadLine();
            //}
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
                Console.WriteLine("4. Buy/sell a stock");
                Console.WriteLine("5. Create Portfolio");
                Console.WriteLine("6. Delete Portfolio");
                Console.WriteLine("7. Simulate an event");
                Console.WriteLine("8. Exit");
                Console.Write("Please choose a number (1-7): ");
                Console.WriteLine();
                string answer = Console.ReadLine();
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
            Console.WriteLine("Would you like to:");
            Console.WriteLine("1. Add funds to the account");
            Console.WriteLine("2. Withdraw funds from the account");
            Console.Write("Please choose a number (1-2): ");
            Console.WriteLine();
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
            else
            {
                Console.WriteLine("Invalid Answer.");
            }
            MainMenu(account);
        }

        /// <summary>
        /// Asks what the user wants to do with the account balance and does said task
        /// </summary>
        /// <param name="account">user's account</param>
        static void AccountBalance(Account account)
        {
            string answer;
            Console.WriteLine("Would you like to:");
            Console.WriteLine("1. See the account balance");
            Console.WriteLine("2. see the cash positions balance");
            Console.WriteLine("3. See a Gain/Losses Report");
            Console.Write("Please choose a number (1-3): ");
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
                Console.Write("Please enter a start date: ");
                string start = Console.ReadLine();
                Console.Write("Please enter an end date: ");
                string end = Console.ReadLine();
                Console.WriteLine();
                account.GenerateReport(start, end);
            }
            else
            {
                Console.WriteLine("Invalid Answer.");
            }
        }

        /// <summary>
        /// asks what the user wants to do with the porfolio balance and does said task
        /// </summary>
        /// <param name="account">user's account</param>
        static void PortfolioBalance(Account account)
        {
            bool exit = false;
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
            while (!exit)
            {
                Console.WriteLine("Would you like to:");
                Console.WriteLine("1. See the portfolio balance");
                Console.WriteLine("2. see the cash and positions balance");
                Console.WriteLine("3. See a Gain/Losses Report (unavailable)");
                Console.WriteLine("4. Go to Main Menu");
                Console.Write("Please choose a number (1-4): ");
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
                //else if (answer == "3")
                //{
                //    Console.Write("Please enter a start date: ");
                //    string start = Console.ReadLine();
                //    Console.Write("Please enter an end date: ");
                //    string end = Console.ReadLine();
                //    p.GenerateReport(start, end);
                //}
                else if(answer == "4")
                {
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Invalid Answer.");
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
            if (account.portfolios.Count > 0)
            {
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
            Console.WriteLine("Please choose a volatility");
            Console.WriteLine("1. High(3% - 15%)");
            Console.WriteLine("2. Medium(2% - 8%");
            Console.WriteLine("3. Low(1% - 4%)");
            Console.Write("Please choose a number (1-3): ");
            answer = Console.ReadLine();
            Console.WriteLine();
            if (answer == "1")
            {
                foreach(Stock s in Stocks)
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
                    foreach(Portfolio p in account.portfolios)
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
                account.UpdateCash();
            }
            else
            {
                Console.WriteLine("Invalid Answer.");
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
            Console.Write("Please enter a Ticker File path: ");
            string file = Console.ReadLine();
            ReadFile(file);
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
            Console.WriteLine("Please choose:");
            Console.WriteLine("1. Buy stocks based on number");
            Console.WriteLine("2. Buy stocks based on the dollar");
            Console.WriteLine("3. Sell stocks");
            answer = Console.ReadLine();
            if(answer == "1")
            {
                bool valid = false;
                Stock s = null;
                while (!valid)
                {
                    Console.WriteLine("Enter the Ticker: ");
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
                    else if(amount < 1)
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
            }
            else if (answer == "2")
            {
                bool valid = false;
                Stock s = null;
                while (!valid)
                {
                    Console.WriteLine("Enter the Ticker: ");
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
            }
            else if(answer == "3")
            {
                bool valid = false;
                while (!valid)
                {
                    Console.Write("Please enter the ticker you want to sell: ");
                    answer = Console.ReadLine();

                }
            }
        }
        /// <summary>
        /// Reads a file and records the stocks at that time
        /// </summary>
        /// <param name="file">name of the file being read in</param>
        static void ReadFile(string file)
        {
            Stocks.Clear();
            StreamReader sr = new StreamReader(file);
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
