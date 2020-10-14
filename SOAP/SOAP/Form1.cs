using SOAP.Entities;
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
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace SOAP
{
    public partial class Form1 : Form
    {
        BindingList<Entities.RateData> Rates = new BindingList<Entities.RateData>();
        string result;

        public Form1()
        {
            InitializeComponent();
            LoadWebService();
            dataGridView1.DataSource = Rates;
            ProcessXML();
            MakeDiagramm();
        }
        private void MakeDiagramm()
        {
            chartRateData.DataSource = Rates;
            Series series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            Legend legend = chartRateData.Legends[0];
            legend.Enabled = false;

            ChartArea chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }
        private void ProcessXML()
        {
            var xml = new XmlDocument();
            xml.LoadXml(result);
            foreach (XmlElement element in xml.DocumentElement)
            {
                var childElement = (XmlElement)element.ChildNodes[0];
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                var rate = new RateData()
                {
                    Date = DateTime.Parse(element.GetAttribute("date")),
                    Currenty = childElement.GetAttribute("curr"),
                    Value = (unit != 0)?(value/unit):(default)
                };
                Rates.Add(rate);
            }
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
            var response = mnbService.GetExchangeRates(request);
            result = response.GetExchangeRatesResult;
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
