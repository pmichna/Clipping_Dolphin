using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dolphin
{
    public partial class MainForm : Form
    {
        private PointF _dolphinPosition;
        private List<PointF> _dolphinPoints = new List<PointF>();
        private List<PointF> _wavesPoints = new List<PointF>();
        private Rectangle _workingArea;
        private int _dolphinHeight = 100;
        private int _dolphinWidth = 220;
        private Timer _timer = new Timer();
        private bool _waveMoveLeft = false;

        public MainForm()
        {
            InitializeComponent();
            _workingArea = ClientRectangle;
            _dolphinPosition = new PointF(_workingArea.Width / 2, _workingArea.Height / 2);
            _dolphinPoints.Add(new PointF(_dolphinPosition.X - _dolphinWidth * 0.46f, _dolphinPosition.Y));
            _dolphinPoints.Add(new PointF(_dolphinPosition.X - _dolphinWidth * 0.3f, _dolphinPosition.Y + _dolphinHeight * 0.4f));
            _dolphinPoints.Add(new PointF(_dolphinPosition.X, _dolphinPosition.Y + _dolphinHeight * 0.5f));
            _dolphinPoints.Add(new PointF(_dolphinPosition.X + _dolphinWidth * 0.23f, _dolphinPosition.Y + _dolphinHeight * 0.4f));
            _dolphinPoints.Add(new PointF(_dolphinPosition.X + _dolphinWidth * 0.38f, _dolphinPosition.Y + _dolphinHeight * 0.4f));
            _dolphinPoints.Add(new PointF(_dolphinPosition.X + _dolphinWidth * 0.54f, _dolphinPosition.Y + _dolphinHeight * 0.6f));
            _dolphinPoints.Add(new PointF(_dolphinPosition.X + _dolphinWidth * 0.54f, _dolphinPosition.Y - _dolphinHeight * 0.6f));
            _dolphinPoints.Add(new PointF(_dolphinPosition.X + _dolphinWidth * 0.38f, _dolphinPosition.Y - _dolphinHeight * 0.4f));
            _dolphinPoints.Add(new PointF(_dolphinPosition.X + _dolphinWidth * 0.23f, _dolphinPosition.Y - _dolphinHeight * 0.4f));
            _dolphinPoints.Add(new PointF(_dolphinPosition.X, _dolphinPosition.Y - _dolphinHeight * 0.5f));
            _dolphinPoints.Add(new PointF(_dolphinPosition.X - _dolphinWidth * 0.3f, _dolphinPosition.Y - _dolphinHeight * 0.4f));
            calculateWavePoints();
            _timer.Interval = 900;
            _timer.Tick += new EventHandler(tick);
            _timer.Start();
        }

        private void tick(object sender, EventArgs e)
        {
            int step = 25;
            if (_waveMoveLeft)
            {
                step = -step;
            }
            _waveMoveLeft = !_waveMoveLeft;
            for (int i = 0; i < _wavesPoints.Count; i++)
            {
                _wavesPoints[i] = new PointF(_wavesPoints[i].X + step, _wavesPoints[i].Y);
            }
            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.DrawPolygon(new Pen(Color.Gray), _dolphinPoints.ToArray());
            //e.Graphics.FillPolygon(Brushes.Gray, _dolphinPoints.ToArray());
            //myFill(e.Graphics, _wavesPoints.ToArray(), Brushes.Blue);
            e.Graphics.DrawPolygon(new Pen(Color.Blue), _wavesPoints.ToArray());
            List<PointF[]> clipAreas;
            if (_dolphinPosition.Y + _dolphinHeight * 0.5f > _workingArea.Height - 140)
            {
                clipAreas = myClip(_dolphinPoints, _wavesPoints, e.Graphics);
                if (clipAreas != null && clipAreas.Count > 0)
                {
                    foreach (PointF[] points in clipAreas)
                        e.Graphics.FillPolygon(Brushes.Black, points);
                }
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            _workingArea = ClientRectangle;
            calculateWavePoints();
            this.Invalidate();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case (Keys.Down):
                    if (_dolphinPosition.Y + _dolphinHeight * 0.5f + 10 <= _workingArea.Height)
                    {
                        for (int i = 0; i < _dolphinPoints.Count; i++)
                            _dolphinPoints[i] = new PointF(_dolphinPoints[i].X, _dolphinPoints[i].Y + 10);
                        _dolphinPosition.Y += 10;
                        this.Invalidate();
                    }
                    break;
                case (Keys.Up):
                    if (_dolphinPosition.Y - _dolphinHeight * 0.5f - 10 >= 0)
                    {
                        for (int i = 0; i < _dolphinPoints.Count; i++)
                            _dolphinPoints[i] = new PointF(_dolphinPoints[i].X, _dolphinPoints[i].Y - 10);
                        _dolphinPosition.Y -= 10;
                        this.Invalidate();
                    }
                    break;
                case (Keys.Left):
                    if (_dolphinPosition.X - _dolphinWidth * 0.46f - 10 >= 0)
                    {
                        for (int i = 0; i < _dolphinPoints.Count; i++)
                            _dolphinPoints[i] = new PointF(_dolphinPoints[i].X - 10, _dolphinPoints[i].Y);
                        _dolphinPosition.X -= 10;
                        this.Invalidate();
                    }
                    break;
                case (Keys.Right):
                    if (_dolphinPosition.X + _dolphinWidth * 0.54f + 10 <= _workingArea.Width)
                    {
                        for (int i = 0; i < _dolphinPoints.Count; i++)
                            _dolphinPoints[i] = new PointF(_dolphinPoints[i].X + 10, _dolphinPoints[i].Y);
                        _dolphinPosition.X += 10;
                        this.Invalidate();
                    }
                    break;
            }

        }

        private void calculateWavePoints()
        {
            _wavesPoints.Clear();
            _wavesPoints.Add(new PointF(-0.05f * _workingArea.Width, _workingArea.Height));
            _wavesPoints.Add(new PointF(0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(2 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(3 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(4 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(5 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(6 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(7 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(8 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(9 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(10 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(11 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(12 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(13 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(14 * 0.08f * _workingArea.Width, _workingArea.Height));
        }

        private void myFill(Graphics g, PointF[] p, Brush b)
        {
            g.FillPolygon(b, p);
        }

        private List<PointF[]> myClip(List<PointF> polygon1, List<PointF> polygon2, Graphics g)
        {
            List<PointF> newPolygon1 = new List<PointF>(polygon1);
            List<PointF> newPolygon2 = new List<PointF>(polygon2);
            List<PointF> enteringPoints = new List<PointF>();
            List<PointF> intersectionPoints = new List<PointF>();
            List<PointF[]> result = new List<PointF[]>();

            for (int sideCnt1 = 0; sideCnt1 < polygon1.Count; sideCnt1++)
            {
                for (int sideCnt2 = 0; sideCnt2 < polygon2.Count; sideCnt2++)
                {
                    PointF p1 = polygon1[sideCnt1];
                    PointF p2;
                    if (sideCnt1 == polygon1.Count - 1)
                        p2 = polygon1[0];
                    else
                        p2 = polygon1[sideCnt1 + 1];
                    PointF pA = polygon2[sideCnt2];
                    PointF pB;
                    if (sideCnt2 == polygon2.Count - 1)
                        pB = polygon2[0];
                    else
                        pB = polygon2[sideCnt2 + 1];
                    PointF intersection = findIntersection(p1, p2, pA, pB);
                    if (intersection.X != -99 && intersection.Y != -99)
                    {
                        newPolygon1.Insert(newPolygon1.IndexOf(p2), intersection);
                        newPolygon2.Insert(newPolygon2.IndexOf(pB), intersection);
                        intersectionPoints.Add(intersection);
                    }
                }
            }
            if(intersectionPoints.Count == 0)
                return null;

            //marking entering points for newPolygon2
            foreach (PointF p in intersectionPoints)
            {
                int index = newPolygon2.IndexOf(p);
                if (!intersectionPoints.Contains(newPolygon2[index - 1]))
                {
                    enteringPoints.Add(p);
                }
            }

            while (enteringPoints.Count != 0)
            {
                List<PointF> area = new List<PointF>();
                int indexP = 0;
                int indexQ = 0;
                indexP = newPolygon2.IndexOf(enteringPoints[0]);
                area.Add(enteringPoints[0]);
                do
                {
                    do
                    {
                        indexP++;
                    } while (!(intersectionPoints.Contains(newPolygon2[indexP]) && !enteringPoints.Contains(newPolygon2[indexP]))); //found exiting point for newPolygon2
                    area.Add(newPolygon2[indexP]);
                    indexQ = newPolygon1.IndexOf(newPolygon2[indexP]);
                    do
                    {
                        indexQ++;
                    } while (!enteringPoints.Contains(newPolygon1[indexQ])); //found entering point for newPolygon2
                    if (!(newPolygon1[indexQ].Equals(enteringPoints[0])))
                        area.Add(newPolygon1[indexQ]);
                } while (!(newPolygon1[indexQ].Equals(enteringPoints[0])));
                
                //one area found
                result.Add(area.ToArray());
                foreach (PointF p in result[result.Count - 1])
                {
                    if (enteringPoints.Contains(p))
                        enteringPoints.Remove(p);
                }
            }

            //g.FillPolygon(Brushes.AliceBlue, newPolygon1.ToArray());
            //g.FillPolygon(Brushes.Beige, newPolygon2.ToArray());
            return result;
        }

        private PointF findIntersection(PointF p1, PointF p2, PointF pA, PointF pB)
        {
            //coefficients for the first line
            float a1 = p2.Y - p1.Y;
            float b1 = p1.X - p2.X;
            float c1 = a1 * p1.X + b1 * p1.Y;

            //coefficients for the second line
            float a2 = pB.Y - pA.Y;
            float b2 = pA.X - pB.X;
            float c2 = a2 * pA.X + b2 * pA.Y;

            float det = a1 * b2 - a2 * b1;
            if (det == 0)
                return new PointF(-99, -99);

            float x = (b2 * c1 - b1 * c2) / det;
            float y = (a1 * c2 - a2 * c1) / det;
            float minx1, maxx1, minxa, maxxa;
            if (p2.X > p1.X)
            {
                minx1 = p1.X;
                maxx1 = p2.X;
            }
            else
            {
                minx1 = p2.X;
                maxx1 = p1.X;
            }
            if (pA.X < pB.X)
            {
                minxa = pA.X;
                maxxa = pB.X;
            }
            else
            {
                minxa = pB.X;
                maxxa = pA.X;
            }

            float miny1, maxy1, minya, maxya;
            if (p1.Y < p2.Y)
            {
                miny1 = p1.Y;
                maxy1 = p2.Y;
            }
            else
            {
                miny1 = p2.Y;
                maxy1 = p1.Y;
            }
            if (pA.Y < pB.Y)
            {
                minya = pA.Y;
                maxya = pB.Y;
            }
            else
            {
                minya = pB.Y;
                maxya = pA.Y;
            }

            if (x >= minx1 && x <= maxx1 && x >= minxa && x <= maxxa &&
                y >= miny1 && y <= maxy1 && y >= minya && y <= maxya)
                return new PointF(x, y);
            else
                return new PointF(-99, -99); //indicates error
        }
    }
}
