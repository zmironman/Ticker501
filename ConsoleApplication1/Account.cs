using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Account
    {
        static double TRADEFEE = 9.99;
        static double TRANSFERFEE = 4.99;
        static int MAXPORTFOLIOS = 3;
        private double _cashBalance;
        private double _stockBalance;
        private double _totalBalance;
        private Portfolio current = new Portfolio(null);
        public IList<Portfolio> portfolios = new List<Portfolio>();

        /// <summary>
        /// constructor for any account object
        /// </summary>
        /// <param name="b">the initial balace</param>
        public Account(double b)
        {
            _totalBalance = b;
            _cashBalance = b;
            _stockBalance = 0;
        }

        /// <summary>
        /// getter for the total balance
        /// </summary>
        public double Total
        {
            get
            {
                return _totalBalance;
            }
        }

        /// <summary>
        /// getter for the cash balance
        /// </summary>
        public double CashBalance
        {
            get
            {
                return _cashBalance;
            }
            set
            {
                _cashBalance = value;
            }
        }

        /// <summary>
        /// getter for the stock balance
        /// </summary>
        public double StockBalance
        {
            get
            {
                return _stockBalance;
            }
        }

        /// <summary>
        /// adds money to the cash and total balances
        /// </summary>
        /// <param name="amount">amount of money to add</param>
        public void AddFunds(double amount)
        {
            _cashBalance += (amount - TRANSFERFEE);
            _totalBalance += (amount - TRANSFERFEE);
        }

        /// <summary>
        /// withdraws money from the account
        /// </summary>
        /// <param name="amount">the amoung to withdraw</param>
        /// <returns>wether the withdraw if valid or not</returns>
        public bool Withdraw(double amount)
        {
            if(_totalBalance - amount < 0)
            {
                return false;
            }
            else
            {
                _totalBalance -= (amount + TRANSFERFEE);
                if(_cashBalance - (amount + TRANSFERFEE) < 0)
                {
                    double over = (_cashBalance - (amount + TRANSFERFEE)) * -1;
                    _cashBalance = 0;
                    if (current.Name == null)
                    {
                        Console.WriteLine("You are withdrawing " + over + " more than your cash balance\nWhat stock would you like to sell(use format 'portfolio, ticker'): ");
                        string entry = Console.ReadLine();
                        string[] split = entry.Split(',');
                        current = GetPortfolio(split[0]);
                        Stock st = current.getStock(split[1]);
                        if (st == null)
                        {
                            return false;
                        }
                        double a;
                        if(current.Sell(Convert.ToInt16(over / st.Price), st, out a))
                        {
                            _cashBalance = a - over;
                        }
                    }
                    else
                    {
                        Console.WriteLine("You are withdrawing " + over + " more than your cash balance\nWhat stock would you like to sell(use the ticker value): ");
                        string entry = Console.ReadLine();
                        Stock s = current.getStock(entry);
                        if(s == null)
                        {
                            return false;
                        }
                        else
                        {
                            double extra = 0;
                            int am = Convert.ToInt32(over / s.Price);
                            current.Sell(am, s, out extra);
                            _cashBalance = extra - over;
                        }
                    }
                }
                else
                {
                    _cashBalance -= (amount + TRANSFERFEE);
                }
                return true;
            }
        }

        /// <summary>
        /// prints the current account balance
        /// </summary>
        public void CheckBal()
        {
            UpdateCash();
            Console.WriteLine("You have $" + _totalBalance.ToString("f") + " available.");
            Console.WriteLine("$" + _cashBalance.ToString("f") + " is not invested, $" + _stockBalance.ToString("f") + " is invested into stocks");
        }

        /// <summary>
        /// prints the current account positions and balance
        /// </summary>
        public void CheckPosBal()
        {
            UpdateCash();
            if(portfolios.Count < 1)
            {
                Console.WriteLine("You dont have any portfolios");
                return;
            }
            foreach(Portfolio p in portfolios)
            {
                Console.WriteLine("In your portfolio '" + p.Name + "' you own:");
                double total = p.Stocks.Count;
                Stock prev = null;
                double count = 1;
                double value = 0;
                foreach (Stock s in p.Stocks)
                {
                    if(s == prev)
                    {
                        count++;
                        value += s.Price;
                        prev = s;
                    }
                    else if(prev == null)
                    {
                        prev = s;
                    }
                    else
                    {
                        Console.WriteLine("$" + value.ToString("F") + " of " + prev.Ticker + " " + prev.Name + " (" + (count / total * 100) + "%)");
                        count = 1;
                        value = 0;
                        prev = s;
                    }
                }
                Console.WriteLine("$" + value.ToString("F") + " of " + prev.Ticker + " " + prev.Name + " (" + ((count) / total * 100) + "%)");
                count = 0;
                value = 0;
            }
        }

        public void GenerateReport(string start, string end)
        {

        }

        /// <summary>
        /// updates a portfolio
        /// </summary>
        /// <param name="p">the portfolio that is to replace the existing one</param>
        public void UpdatePortfolio(Portfolio p)
        {
            foreach (Portfolio po in portfolios)
            {
                if (po.Name == p.Name)
                {
                    portfolios.Remove(po);
                    portfolios.Add(p);
                }
            }
        }

        /// <summary>
        /// gets a portfolio from the account with the provided name
        /// </summary>
        /// <param name="n">the name of the wanted portfolio</param>
        /// <returns>the wanted portfolio</returns>
        public Portfolio GetPortfolio(string n)
        {
            foreach(Portfolio p in portfolios)
            {
                if(p.Name == n)
                {
                    return p;
                }
            }
            return new Portfolio("error");
        }

        /// <summary>
        /// creates a new portfolio
        /// </summary>
        /// <param name="n">name of the new portfolio</param>
        public void NewPortfolio(string n)
        {
            Portfolio temp = new Portfolio(n);
            portfolios.Add(temp);
        }

        /// <summary>
        /// deletes a portfolio
        /// </summary>
        /// <param name="n">name of the portfolio we want to delete</param>
        /// <returns>wether a portfolio was delete or not</returns>
        public bool DelPortfolio(string n)
        {
            foreach (Portfolio p in portfolios)
            {
                if (p.Name == n)
                {
                    _cashBalance += p.Balance;
                    portfolios.Remove(p);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// updates the value of the stocks and balances
        /// </summary>
        public void UpdateCash()
        {
            _stockBalance = 0;
            if (portfolios == null)
                return;
            foreach(Portfolio p in portfolios)
            {
                foreach(Stock s in p.Stocks)
                {
                    _stockBalance += s.Price;
                }
            }
            _totalBalance = _stockBalance + _cashBalance;
        }

        
    }
}
