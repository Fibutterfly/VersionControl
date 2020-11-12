using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Factory_example.Abstraction;
using Factory_example.Entites;

namespace Factory_example
{
    public partial class Form1 : Form
    {
        List<Toy> _toys = new List<Toy>();
        private Toy _nextToy;
        private IToyFactory _factory;

        public IToyFactory Factory
        {
            get { return _factory; }
            set 
            { 
                _factory = value;
                DisplayNeXt();
            }
        }

        public Form1()
        {
            InitializeComponent();
            Factory = new CarFactory();
            createTimer.Tick += CreateTimer_Tick;
            createTimer.Start();
            conveyorTimer.Tick += ConveyorTimer_Tick;
            conveyorTimer.Start();
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button4.Click += Button4_Click;
            button5.Click += Button4_Click;
            button6.Click += Button4_Click;
            button3.Click += Button3_Click;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Factory = new PresentFactory()
            {
                Box = button5.BackColor,
                Ribbon = button6.BackColor
            };
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            var input = (Button)sender;
            var cd = new ColorDialog();
            cd.Color = input.BackColor;
            if (cd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            input.BackColor = cd.Color;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Factory = new BallFactory()
            {
                BallColor = button4.BackColor
            };
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Factory = new CarFactory();
        }
        private void DisplayNeXt()
        {
            if(_nextToy != null)
            {
                Controls.Remove(_nextToy);
            }
            _nextToy = Factory.CreateNew();
            _nextToy.Top = label1.Top + label1.Height + 20;
            _nextToy.Left = label1.Left;
            Controls.Add(_nextToy);
        }
        private void ConveyorTimer_Tick(object sender, EventArgs e)
        {
            int maxPosition = 0;
            foreach (var ball in _toys)
            {
                ball.MoveToy();
                maxPosition = ball.Left;
            }
            if (maxPosition > 1000)
            {
                var oldestBall = _toys[0];
                mainPanel.Controls.Remove(oldestBall);
                _toys.Remove(oldestBall);
            }
        }

        private void CreateTimer_Tick(object sender, EventArgs e)
        {
            var ball = Factory.CreateNew();
            _toys.Add(ball);
            ball.Left = -ball.Width;
            mainPanel.Controls.Add(ball);
        }
    }
}
