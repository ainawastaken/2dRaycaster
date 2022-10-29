using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2dRaycastThing
{
    public partial class Form1 : Form
    {
        List<PointF[]> objects = new List<PointF[]>();

        bool buttonHeld = false;

        PointF location;
        PointF originalLocation;
        double angle;

        bool locMove;

        int fov = 60;
        List<PointF> pnts = new List<PointF>();

        public Form1()
        {
            PointF[] points = { new PointF(100, 0), new PointF(0, 150), new PointF(200, 150)};
            objects.Add(points);
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            location = e.Location;
            angle = util.Angle(originalLocation, location);
            if (locMove)
            {
                pnts = util.multicast(fov, 150f, originalLocation, angle*-1, objects.ToArray());
                pnts.Add(originalLocation);
            }
            this.Refresh();
            GC.Collect();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (buttonHeld)
            {
                e.Graphics.DrawLine(Pens.Black, location, originalLocation);

                e.Graphics.DrawString(Math.Round(angle).ToString(),SystemFonts.DefaultFont,Brushes.Black, originalLocation);
                for (int i = 0; i < fov; i++)
                {
                    e.Graphics.DrawLine(Pens.Green, pnts[i], originalLocation);
                }
                if (locMove)
                {
                    
                    e.Graphics.DrawPolygon(Pens.Green, pnts.ToArray());
                    e.Graphics.DrawString(pnts.Count.ToString(), SystemFonts.DefaultFont, Brushes.Black, location);
                    e.Graphics.DrawString(string.Join(",\n", pnts), SystemFonts.DefaultFont, Brushes.Black, Point.Empty);
                }
            }
            foreach (PointF[] obj in objects)
            {
                e.Graphics.DrawPolygon(Pens.Black, obj);
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            originalLocation = location;
            locMove = true;
            buttonHeld = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            originalLocation = PointF.Empty;
            locMove = false;
            buttonHeld = false;
        }
    }
}
