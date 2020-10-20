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
using Szimulacio.Entities;

namespace Szimulacio
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        public Form1()
        {
            InitializeComponent();
            Population = GetPopulation(@"C:\Temp\nép.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\Születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");
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
                            Gender = (Gender)Enum.Parse(typeof(Gender), helper[0]),
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
                                P = double.Parse(helper[2])
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
                                P = double.Parse(helper[2])
                            }
                        );
                }
            }

            return deathProbabilities;
        }
    }
}
