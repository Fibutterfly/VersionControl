using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Factory_example.Entites;

namespace Factory_example
{
    public partial class Form1 : Form
    {
        List<Toy> _toys = new List<Toy>();
        private BallFactory _factory;

        public BallFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }

        public Form1()
        {
            InitializeComponent();
            Factory = new BallFactory();
            createTimer.Tick += CreateTimer_Tick;
            createTimer.Start();
            conveyorTimer.Tick += ConveyorTimer_Tick;
            conveyorTimer.Start();
        }

        private void ConveyorTimer_Tick(object sender, EventArgs e)
        {
            int maxPosition = 0;
            foreach (var ball in _toys)
            {
                ball.MoveToy();
                maxPosition = ball.Left;
            }
            if (maxPosition > 100)
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
