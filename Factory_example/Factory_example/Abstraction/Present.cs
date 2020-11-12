using Factory_example.Entites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_example.Abstraction
{
    class Present : Toy
    {
        public SolidBrush Ribbon { get; private set; }
        public SolidBrush Box { get; private set; }
        public Present(Color ribbon, Color box)
        {
            Ribbon = new SolidBrush(ribbon);
            Box = new SolidBrush(box);
        }
        protected override void DrawImage(Graphics input)
        {
            input.FillRectangle(Box,0,0,Width,Height);
            input.FillRectangle(Ribbon, (float)Math.Round((double)(Width/2))-5, 0, 10, Height);
            input.FillRectangle(Ribbon, 0, (float)Math.Round((double)(Height / 2)) - 5, Width, 10);
        }
    }
}
