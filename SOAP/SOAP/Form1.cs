using SOAP.MnbServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOAP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadWebService();
        }
        private void LoadWebService()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-30"

            };
            GetExchangeRatesResponseBody response = mnbService.GetExchangeRates(request);
            string result = response.GetExchangeRatesResult;
            writeToFile("Eur_20_01_01__20_06_30.xml",result);
        }
        private void writeToFile(string name, string data)
        {
            using (StreamWriter sw = new StreamWriter(name))
            {
                sw.WriteLine(data);
            }
        }

    }
}
