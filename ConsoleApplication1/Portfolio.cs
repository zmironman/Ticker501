using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Portfolio
    {
        public List<Stock> Stocks = new List<Stock>();
        private List<Stock> _boughtPrice = new List<Stock>();
        private double _balance;
        private string _name;
        private double _profit;

        /// <summary>
        /// Constructor for the portfolio object
        /// </summary>
        /// <param name="n">name of the new portfolio</param>
        public Portfolio(string n)
        {
            _name = n;
            _balance = 0;
            _profit = 0;
        }

        /// <summary>
        /// getter for the profit of the protfolio
        /// </summary>
        public double Profit
        {
            get
            {
                return _profit;
            }
        }

        /// <summary>
        /// Buys the number of stocks provided
        /// </summary>
        /// <param name="number">number of stocks to buy</param>
        /// <param name="stock">stock to buy</param>
        /// <returns>the price of the transaction</returns>
        public double BuyNumber(int number, Stock stock)
        {
            double price = 0;
            for (int i = 0; i < number; i++)
            {
                Stocks.Add(stock);
                _boughtPrice.Add(stock);
                _balance += stock.Price;
                price += stock.Price;
            }
            return price;
        }

        /// <summary>
        /// buys the number of stocks with the money value provided
        /// </summary>
        /// <param name="amount">amount of money to spend</param>
        /// <param name="stock">the stock to buy</param>
        /// <param name="price">the price of the transaction</param>
        /// <returns>the number of stocks bought</returns>
        public int BuyValue(double amount, Stock stock, out double price)
        {
            price = 0;
            double stockVal = stock.Price;
            int total = 0;
            while(stockVal < amount)
            {
                Stocks.Add(stock);
                _boughtPrice.Add(stock);
                total += 1;
                stockVal += stock.Price;
                _balance += stock.Price;
                price += stock.Price;
            }
            return total;
        }

        /// <summary>
        /// Sells the number of stocks provided
        /// </summary>
        /// <param name="number">number of stocks to sell</param>
        /// <param name="stock">the stock to sell</param>
        /// <param name="total">the amount of money made</param>
        /// <returns>if the stocks sold or not</returns>
        public bool Sell(int number, Stock stock, out double total)
        {
            total = 0;
            int count = numberOfStocks(stock);
            if (count < number)
            {
                while (Stocks.Contains(stock))
                {
                    Stocks.Remove(stock);
                    _boughtPrice.Remove(stock);
                    total += stock.Price;
                    _balance -= stock.Price;
                }
                return false;
            }
            for(int i = 0; i < number; i++)
            {
                Stocks.Remove(stock);
                _boughtPrice.Remove(stock);
                total += stock.Price;
                _balance -= stock.Price;
            }
            return true;
        }

        /// <summary>
        /// Name getter
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }
        
        /// <summary>
        /// Balance getter
        /// </summary>
        public double Balance
        {
            get
            {
                UpdateBalance();
                return _balance;
            }
        }

        /// <summary>
        /// Gets the number of stocks owned with the stock provided
        /// </summary>
        /// <param name="stock">stock of interest</param>
        /// <returns>number of stocks with the same name</returns>
        private int numberOfStocks(Stock stock)
        {
            int count = 0;
            foreach (Stock s in Stocks)
            {
                if (s.Name == stock.Name)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// updates the balance of the portfolio
        /// </summary>
        public void UpdateBalance()
        {
            _balance = 0;
            foreach(Stock s in Stocks)
            {
                _balance += s.Price;
            }
        }

        /// <summary>
        /// updates the stock with the new price
        /// </summary>
        /// <param name="s">the stock we want to update</param>
        /// <param name="newPrice">new price of the stock</param>
        public void UpdateStock(Stock s, double newPrice)
        {
            foreach(Stock st in Stocks)
            {
                if(s.Name == st.Name)
                {
                    _profit += newPrice - st.Price;
                    st.Price = newPrice;
                }
            }
        }

        /// <summary>
        /// checks the portfolio's balance
        /// </summary>
        /// <param name="total"></param>
        public void CheckBal(double total)
        {
            double percentage = _balance / total;
            Console.WriteLine("Portfolio: " + Name);
            Console.WriteLine("Total Investment: $" + _balance);
            Console.WriteLine("Percentage of account: " + percentage + "%");
          
        }

        /// <summary>
        /// checks the portfolios position balance
        /// </summary>
        public void CheckPosBal()
        {
            Console.WriteLine("In your portfolio '" + Name + "' you own:");
            int total = Stocks.Count;
            Stock prev = null;
            int count = 0;
            double value = 0;
            foreach (Stock s in Stocks)
            {
                if (s == prev)
                {
                    count++;
                    value += s.Price;
                    prev = s;
                }
                else if (prev == null)
                {
                    prev = s;
                }
                else
                {
                    Console.WriteLine("$" + value.ToString("F") + " of " + prev.Ticker + " " + prev.Name + " (" + (count / total * 100) + "%)");
                    count = 0;
                    value = 0;
                    prev = s;
                }
            }
            Console.WriteLine("$" + value.ToString("F") + " of " + prev.Ticker + " " + prev.Name + " (" + ((count + 1) / total * 100) + "%)");
            count = 0;
            value = 0;
        }

        /// <summary>
        /// gets the stock with the name provided
        /// </summary>
        /// <param name="name">name of the stock we want</param>
        /// <returns>the stock with the name of the parameter</returns>
        public Stock getStock(string name)
        {
            foreach(Stock s in Stocks)
            {
                if (s.Name == name)
                {
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// gets the stock with the name provided
        /// </summary>
        /// <param name="name">name of the stock we want</param>
        /// <returns>the stock with the name of the parameter</returns>
        public Stock getStockT(string tick)
        {
            foreach (Stock s in Stocks)
            {
                if (s.Ticker == tick)
                {
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// generates a report for the given portfolio
        /// </summary>
        public void GenerateReport()
        {
            if(_profit < 0)
            {
                Console.WriteLine("You have lost $" + _profit * -1);
            }
            else
            {
                Console.WriteLine("You have lost $" + _profit);
            }
            return;
        }
    }
}
