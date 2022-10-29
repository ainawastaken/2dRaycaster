using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace _2dRaycastThing
{
    public static class util
    {
        public static double[] multicastTime;

        const double Rad2Deg = 180.0 / Math.PI;
        const double Deg2Rad = Math.PI / 180.0;
        public static double Angle(PointF start, PointF end)
        {
            return Math.Atan2(start.Y - end.Y, end.X - start.X) * Rad2Deg;
        }

        public static PointF MoveInDirection(PointF _from, float distance, double direction)
        {
            double new_x = _from.X + distance * Math.Cos(direction * Math.PI / 180);
            double new_y = _from.Y + distance * Math.Sin(direction * Math.PI / 180);
            PointF value = new PointF((int)Math.Round(new_x), (int)Math.Round(new_y));
            return value;
        }

        public static bool IsPointInPolygon(PointF[] polygon, PointF point)
        {
            bool isInside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }

        public static List<PointF> multicast(int fov, float range, PointF location, double angle, PointF[][] objects, float precision=0.1f)
        {
            Stopwatch sw = new Stopwatch();
            double ang;
            multicastTime = new double[fov];
            List<PointF> arr = new List<PointF>();
            float f = (fov / 2) * -1;
            for (int i = 0; i < fov; i++)
            {
                sw.Start();
                ang = angle + (double)f;
                PointF pnt = raycast(objects,location, ang, range, precision);
                arr.Add(pnt);
                multicastTime[i] = sw.Elapsed.TotalMilliseconds;
                sw.Reset();
                sw.Stop();
                f++;
            }
            return arr;
        }

        public static PointF raycast(PointF[][] objects, PointF startLocation, double angle, float maxDist, float step)
        {
            PointF point = new PointF();
            for (float i = 0; i < maxDist; i = i+step)
            {
                point = MoveInDirection(startLocation, i, angle);
                foreach (PointF[] objec in objects)
                {
                    if (IsPointInPolygon(objec,point))
                    {
                        return point;
                    }
                }
            }
            return point;
        }

        public static void NoiseReduction(ref double[] src, int severity = 1)
        {
            for (int i = 1; i < src.Length; i++)
            {
                //---------------------------------------------------------------avg
                var start = (i - severity > 0 ? i - severity : 0);
                var end = (i + severity < src.Length ? i + severity : src.Length);

                double sum = 0;

                for (int j = start; j < end; j++)
                {
                    sum += src[j];
                }

                var avg = sum / (end - start);
                //---------------------------------------------------------------
                src[i] = avg;

            }
        }
    }
    
}
