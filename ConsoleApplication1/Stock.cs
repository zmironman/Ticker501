using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Stock
    {
        private string _ticker;
        private string _name;
        private double _price;

        /// <summary>
        /// constructor for any stock object
        /// </summary>
        /// <param name="t">ticker of the stock</param>
        /// <param name="n">name of the stock</param>
        /// <param name="p">price of the stock</param>
        public Stock(string t, string n, double p)
        {
            _ticker = t;
            _name = n;
            _price = p;
        }

        /// <summary>
        /// getter for the name of the stock
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// getter for the ticker of the stock
        /// </summary>
        public string Ticker
        {
            get
            {
                return _ticker;
            }
        }

        /// <summary>
        /// getter and setter for the price of the stock
        /// </summary>
        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
            }

        }
    }
}
