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
        DateTime StartDate = new DateTime(year: 2020, month: 01, day: 01);
        DateTime EndDate = new DateTime(year: 2020, month: 06, day: 30);
        string currency = "EUR";
        public Form1()
        {
            InitializeComponent();
            dateTimePicker1.Value = StartDate;
            dateTimePicker2.Value = EndDate;
            dateTimePicker1.ValueChanged += DateTimePicker1_ValueChanged;
            dateTimePicker2.ValueChanged += DateTimePicker2_ValueChanged;
            FillCombo();
            RefreshData();

            comboBox1.Click += ComboBox1_Click;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void ComboBox1_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void FillCombo()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetCurrenciesRequestBody();
            var response = mnbService.GetCurrencies(request);
            var xml = new XmlDocument();
            xml.LoadXml(response.GetCurrenciesResult);
            BindingList<string> curr = new BindingList<string>();
            foreach (XmlElement item in xml.GetElementsByTagName("Currencies"))
            {
                foreach (XmlElement childElement in item.ChildNodes)
                {
                    //Console.WriteLine(childElement.InnerText);
                    curr.Add(childElement.InnerText);
                }
               
            }
            comboBox1.DataSource = curr;
            comboBox1.SelectedItem = currency;

        }
        private void RefreshData()
        {
            Rates.Clear();
            LoadWebService(comboBox1.SelectedItem.ToString() ,startDate: dateTimePicker1.Value, endDate:dateTimePicker2.Value);
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
                if (childElement == null)
                {
                    continue;
                }
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
        private void LoadWebService(string currency = "EUR",DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate == null)
            {
                startDate = StartDate;
            }
            if (endDate == null)
            {
                endDate = EndDate;
            }
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = currency,
                startDate = startDate.ToString(),
                endDate = endDate.ToString()

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
