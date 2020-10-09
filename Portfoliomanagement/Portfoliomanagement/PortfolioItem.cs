using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfoliomanagement
{
    class PortfolioItem
    {
        private string _index;

        public string Index
        {
            get { return _index; }
            set { _index = value; }
        }
        private decimal _volume;

        public decimal Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }


    }
}
