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

        double[] xs;
        double[] ys;

        int fov = 110;
        PointF[] pnts;

        public Form1()
        {
            pnts = new PointF[fov + 1];
            PointF[] points = { new PointF(110, 10), new PointF(10, 160), new PointF(210, 160)};
            objects.Add(points);
            xs = new double[fov];
            for (double i = 0; i < fov; i++)
            {
                xs[(int)i] = i;
            }

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            rayTime.Plot.Title("Ray time (ms)");
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            location = e.Location;
            angle = util.Angle(originalLocation, location);
            if (locMove)
            {
                pnts = util.multicast(fov, 200f, originalLocation, angle * -1, objects.ToArray());
                rayTime.Plot.Clear();
                util.NoiseReduction(ref util.multicastTime,100);
                rayTime.Plot.AddScatter(xs, util.multicastTime);
                rayTime.Refresh();
                pnts[pnts.Length-1] = originalLocation;
            }
            this.Refresh();
            GC.Collect();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (buttonHeld)
            {
                e.Graphics.DrawLine(Pens.Black, location, originalLocation);

                e.Graphics.DrawString(Math.Round(angle).ToString(),SystemFonts.DefaultFont,Brushes.Black, originalLocation);
                for (int i = 0; i < fov; i++)
                {
                    if (visRays_cb1.Checked)
                    {
                        e.Graphics.DrawLine(Pens.Green, pnts[i], originalLocation);
                    }
                }
                if (locMove)
                {
                    
                    e.Graphics.DrawPolygon(Pens.Green, pnts.ToArray());
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
