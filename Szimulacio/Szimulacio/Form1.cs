using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Szimulacio.Entities;

namespace Szimulacio
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        Random rnd = new Random(1024);
        string PopLocation = @"C:\Temp\nép-teszt.csv";
        public Form1()
        {
            InitializeComponent();
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\Születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");
            button2.Click += Button2_Click;
            button1.Click += Button1_Click;
            numericUpDown1.Minimum = 1950;
            numericUpDown1.Maximum = 2500;
            numericUpDown1.Value = 2024;
            RefreshFile();
        }
        private void RefreshFile()
        {
            textBox1.Text = PopLocation;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    PopLocation = ofd.FileName;
                    RefreshFile();
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Population = GetPopulation(textBox1.Text);
            Szimu();
        }

        public void SimStep(int year, Person person)
        {
            if (!person.IsAlive)
            {
                return;
            }
            byte age = (byte)(year - person.BirthYear);
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.P).FirstOrDefault();
            if (rnd.NextDouble() <= pDeath)
            {
                person.IsAlive = false;
            }
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.P).FirstOrDefault();
                if (rnd.NextDouble() <= pBirth)
                {
                    Person newborn = new Person()
                    {
                        BirthYear = year,
                        NbrOfChildren = 0,
                        Gender = (Gender)(rnd.Next(1,3))
                    };
                    Population.Add(newborn);
                }
            }
        }
        public void Szimu()
        {
            for (int year = 2005; year <= numericUpDown1.Value; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }
                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                Console.WriteLine($"Év:{year} Fiúk:{nbrOfMales} Lányok:{nbrOfFemales}");
                listBox1.Items.Add($"Szimulációs év: {year}");
                listBox1.Items.Add($"\t Fiúk: {nbrOfMales}");
                listBox1.Items.Add($"\t Lányok: {nbrOfFemales}");
                listBox1.Items.Add("");
            }
        }
        public List<Person> GetPopulation(string csv)
        {
            List<Person> population = new List<Person>();
            using (StreamReader sr = new StreamReader(csv, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string[] helper = sr.ReadLine().Split(';');
                    population.Add
                    (
                        new Person()
                        {
                            BirthYear = int.Parse(helper[0]),
                            Gender = (Gender)Enum.Parse(typeof(Gender), helper[1]),
                            NbrOfChildren = int.Parse(helper[2])   
                        }
                    );
                }
            }

           return population;
        }
        public List<BirthProbability> GetBirthProbabilities(string csv)
        {
            List<BirthProbability> birthProbabilities = new List<BirthProbability>();
            using (StreamReader sr = new StreamReader(csv, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string[] helper = sr.ReadLine().Split(';');
                    birthProbabilities.Add
                        (
                            new BirthProbability()
                            {
                                
                                Age = int.Parse(helper[0]),
                                NbrOfChildren = int.Parse(helper[1]),
                                P = double.Parse(helper[2])/1000
                            }
                        );

                }
            }
            return birthProbabilities;
        }
        public List<DeathProbability> GetDeathProbabilities(string csv)
        {
            List<DeathProbability> deathProbabilities = new List<DeathProbability>();
            using (StreamReader sr = new StreamReader(csv,Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string[] helper = sr.ReadLine().Split(';');
                    deathProbabilities.Add
                        (
                            new DeathProbability()
                            {
                                Gender = (Gender)Enum.Parse(typeof(Gender), helper[0]),
                                Age = int.Parse(helper[1]),
                                P = double.Parse(helper[2])/1000
                            }
                        );
                }
            }

            return deathProbabilities;
        }
    }
}
