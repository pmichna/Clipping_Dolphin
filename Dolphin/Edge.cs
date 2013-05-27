using System;
using System.Collections.Generic;
using System.Drawing;

namespace Dolphin
{
    public class Edge
    {
        private Point _p1;
        private Point _p2;
        private int _xval;

        public Edge(PointF p1, PointF p2)
        {
            //_p1 = p1;
            _p1 = new Point((int)p1.X, (int)p1.Y);
            _p2 = new Point((int)p2.X, (int)p2.Y);
            _xval = _p1.Y < _p2.Y ? _p1.X : _p2.X;
        }

        public int Ymin
        {
            get
            {
                return _p1.Y < _p2.Y ? _p1.Y : _p2.Y;
            }
        }

        public int Ymax
        {
            get
            {
                return _p1.Y > _p2.Y ? _p1.Y : _p2.Y;
            }
        }

        public int Xmin
        {
            get
            {
                return _p1.X < _p2.X ? _p1.X : _p2.X;
            }
        }

        public int Xmax
        {
            get
            {
                return _p1.X > _p2.X ? _p1.X : _p2.X;
            }
        }

        public int getXval()
        {
            return _xval;
        }

        public int XvalOfYmin
        {
            get
            {
                return _xval;
            }
            set
            {
                _xval = value;
            }
        }

        public float Slope
        {
            get
            {
                if (Xmax - Xmin == 0) return 0;
                return (Ymax - Ymin) / (Xmax - Xmin);
            }
        }
    }
}
