﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Factory_example.Entites
{
    class Ball : Label
    {
        public Ball()
        {
            AutoSize = false;
            Width = 50;
            Height = 50;
            Paint += Ball_Paint;
        }

        private void Ball_Paint(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }
        protected void DrawImage(Graphics input)
        {
            input.FillEllipse(new SolidBrush(Color.Blue), 0, 0, Width, Height);
        }
        private void MoveBall()
        {
            Left += 1;
        }
    }
}
