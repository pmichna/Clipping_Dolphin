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
            //int step = 25;
            //if (_waveMoveLeft)
            //{
            //    step = -step;
            //}
            //_waveMoveLeft = !_waveMoveLeft;
            //for (int i = 0; i < _wavesPoints.Count; i++)
            //{
            //    _wavesPoints[i] = new PointF(_wavesPoints[i].X + step, _wavesPoints[i].Y);
            //}
            //this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.DrawPolygon(new Pen(Color.Gray), _dolphinPoints.ToArray());
            e.Graphics.DrawPolygon(new Pen(Color.Blue), _wavesPoints.ToArray());
            List<PointF[]> clipAreas;
            if (_dolphinPosition.Y + _dolphinHeight * 0.5f > _workingArea.Height - 140)
            {
                clipAreas = myClip(_dolphinPoints, _wavesPoints);
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
            _wavesPoints.Add(new PointF(14 * 0.08f * _workingArea.Width, _workingArea.Height));
            _wavesPoints.Add(new PointF(13 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(12 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(11 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(10 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(9 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(8 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(7 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(6 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(5 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(4 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(3 * 0.08f * _workingArea.Width, _workingArea.Height - 140));
            _wavesPoints.Add(new PointF(2 * 0.08f * _workingArea.Width, _workingArea.Height - 56));
            _wavesPoints.Add(new PointF(0.08f * _workingArea.Width, _workingArea.Height - 140));
        }

        private void myFill(Graphics g, PointF[] p, Brush b)
        {
            g.FillPolygon(b, p);
        }

        private List<PointF[]> myClip(List<PointF> polygon1, List<PointF> polygon2)
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
                        insertIntoPolygon(ref newPolygon1, intersection, p1, p2);
                        insertIntoPolygon(ref newPolygon2, intersection, pA, pB);
                        intersectionPoints.Add(intersection);
                    }
                }
            }
            if (intersectionPoints.Count == 0)
                return null;

            //marking entering points for newPolygon2
            foreach (PointF p in intersectionPoints)
            {
                int index = newPolygon2.IndexOf(p);
                if (!isPointInPolygon(newPolygon1.ToArray(), newPolygon2[index - 1]))
                {
                    enteringPoints.Add(p);
                }
            }
            
            //TODO: fix out of memory Exception;
            while (enteringPoints.Count != 0)
            {
                List<PointF> area = new List<PointF>();
                PointF currPoint = enteringPoints[0];
                area.Add(enteringPoints[0]);
                do
                {
                    do
                    {
                        int index = newPolygon2.IndexOf(currPoint);
                        int nextIndex = index + 1;
                        if (nextIndex == newPolygon2.Count)
                            nextIndex = 0;
                        currPoint = newPolygon2[nextIndex];
                        if (!(currPoint.Equals(enteringPoints[0])))
                            area.Add(currPoint);
                    } while (!(intersectionPoints.Contains(currPoint) && !enteringPoints.Contains(currPoint))); //found exiting point for newPolygon2
                    do
                    {
                        int index = newPolygon1.IndexOf(currPoint);
                        int nextIndex = index + 1;
                        if (nextIndex == newPolygon1.Count)
                            nextIndex = 0;
                        currPoint = newPolygon1[nextIndex];
                        if (!(currPoint.Equals(enteringPoints[0])))
                            area.Add(currPoint);
                    } while (!enteringPoints.Contains(currPoint)); //found entering point for newPolygon2
                } while (!(currPoint.Equals(enteringPoints[0])));

                //one area found
                result.Add(area.ToArray());
                foreach (PointF p in result[result.Count - 1])
                {
                    enteringPoints.Remove(p);
                }
            }
            return result;
        }

        private void insertIntoPolygon(ref List<PointF> polygon, PointF intersection, PointF segmentBegin, PointF segmentEnd)
        {
            // get all intersections for current line segment on polygon
            List<PointF> newLineSegment = new List<PointF>();
            int indexP1 = polygon.IndexOf(segmentBegin);
            int indexP2 = polygon.IndexOf(segmentEnd);
            int count;
            if (indexP2 == 0)
            {
                if (indexP1 == polygon.Count - 1)
                {
                    polygon.Add(intersection);
                    return;
                }
                else
                {
                    bool inserted = false;
                    for (int i = indexP1; i < polygon.Count; i++)
                    {
                        if (calculateDistance(segmentBegin, intersection) < calculateDistance(segmentBegin, polygon[i]) && !inserted)
                        {
                            newLineSegment.Add(intersection);
                            inserted = true;
                        }
                        newLineSegment.Add(polygon[i]);
                    }
                    count = polygon.Count - indexP1;
                }
            }
            else
            {
                bool inserted = false;
                for (int i = indexP1; i <= indexP2; i++)
                {
                    if (calculateDistance(segmentBegin, intersection) < calculateDistance(segmentBegin, polygon[i]) && !inserted)
                    {
                        newLineSegment.Add(intersection);
                        inserted = true;
                    }
                    newLineSegment.Add(polygon[i]);
                }
                //count points on the segment
                count = indexP2 - indexP1 + 1;
            }
            polygon.RemoveRange(indexP1, count);
            polygon.InsertRange(indexP1, newLineSegment);
        }

        private float calculateDistance(PointF p1, PointF p2)
        {
            return ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        private static bool isPointInPolygon(PointF[] polygon, PointF testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        private float[] getABC(PointF p1, PointF p2)
        {
            float a = p2.Y - p1.Y;
            float b = p1.X - p2.X;
            float c = a * p1.X + b * p1.Y;
            return new float[] { a, b, c };
        }

        private PointF findIntersection(PointF p1, PointF p2, PointF pA, PointF pB)
        {
            //coefficients for the first line
            float[] coefficients1 = getABC(p1, p2);

            //coefficients for the second line
            float[] coefficients2 = getABC(pA, pB);

            float det = coefficients1[0] * coefficients2[1] - coefficients2[0] * coefficients1[1];
            if (det == 0)
                return new PointF(-99, -99);

            float x = (coefficients2[1] * coefficients1[2] - coefficients1[1] * coefficients2[2]) / det;
            float y = (coefficients1[0] * coefficients2[2] - coefficients2[0] * coefficients1[2]) / det;
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
