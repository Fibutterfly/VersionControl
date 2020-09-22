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
using UserMaintenance.Entities;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<User>();
        public Form1()
        {
            InitializeComponent();
            label1.Text = Resource1.FullName;
            button1.Text = Resource1.Add;

            listBox1.DataSource = users;
            listBox1.ValueMember = "ID";
            listBox1.DisplayMember = "FullName";

            button1.Click += Button1_Click;

            button2.Text = Resource1.savefeil;
            button2.Click += Button2_Click;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog asd = new SaveFileDialog();
            if (asd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(asd.FileName))
                {
                    foreach (var item in users)
                    {
                        sw.WriteLine($"{item.ID};{item.FullName}");
                    }
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            User u = new User()
            {
                FullName = textBox1.Text,
            };
            users.Add(u);
        }
    }
}
