using System;
using System.Drawing;
using System.Windows.Forms;

namespace Budzisz
{
    public partial class Potencjometr : UserControl
    {
        private double _Value=13;
        [System.ComponentModel.CategoryAttribute("Value")]
        public double Value
        {
            get
            {
                return _Value;  
            }
            set
            {
                _angle = (value * 360) / (Math.Abs(min) + Math.Abs(max))+90;
                _Value = value;
                Invalidate();
            }
        }
        private void setValueByAngle(double value)
        {

        double ang = value;
        double result = 0;

        result = ((Math.Abs(min)+Math.Abs(max))*ang)/360.0;
                if (result > max) Value = max;
                if (result<min) Value = min;
                else Value = result;
        }
        private int min=0;
        private int max=20;
        [System.ComponentModel.CategoryAttribute("Min")]
        public int Min
        {
            get { return min; }
            set { min = value;
                if (value > _Value) _Value = value;
                Invalidate();
            }
        }
        [System.ComponentModel.CategoryAttribute("Max")]
        public int Max
        {
            get { return max; }
            set { max = value;
                if (value < _Value) _Value = value;
                Invalidate();
            }
        }

        private bool _IsType1=false;
        [System.ComponentModel.CategoryAttribute("Type1")]
        public bool IsType1
        {
            get { return _IsType1; }
            set {
                _IsType1 = value;
                if (value == true)
                {
                    IsType2 = false;
                    IsType3 = false;
                }
                else if (!IsType2 && !IsType3)
                { IsType2 = true; }
                Invalidate();
            }
        }
        private bool _IsType2 = true;
        [System.ComponentModel.CategoryAttribute("Type2")]
        public bool IsType2
        {
            get { return _IsType2; }
            set { _IsType2 = value;

                if (value == true)
                {
                    IsType1 = false;
                    IsType3 = false;
                }
                else if (!IsType1 && !IsType3)
                { IsType3 = true; }
                Invalidate();
            }
        }
        private bool _IsType3 = false;
        [System.ComponentModel.CategoryAttribute("Type3")]
        public bool IsType3
        {
            get { return _IsType3; }
            set
            {
                _IsType3 = value;
                if (value == true)
                {
                    IsType2 = false;
                    IsType1 = false;
                }
                else if (!IsType2 && !IsType1)
                { IsType1 = true; }
                Invalidate();
            }
        }
        [System.ComponentModel.CategoryAttribute("PotentiometerColor")]
        private Color _PotentiometerColor = Color.FromArgb(100, 50, 50, 250);
        public Color PotentiometerColor
        {
            get { return _PotentiometerColor; }
            set
            {
                _PotentiometerColor = value;
                Invalidate();
            }
        }
        Rectangle figureToCath = new Rectangle(0, 0, 0, 0);
        private double _angle = 0;
        public double angle {
            get
            {
                if (_angle - 90 >= 0)
                {
                    return _angle - 90;
                }
                else
                {
                    return 360 - (90 - _angle);
                }
            }
        }
        public Potencjometr()
        {
            InitializeComponent();
            DoubleBuffered = true;
            MouseLocation = new Point(Width / 2, 0);
            Invalidate();
        }
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Right)
            {
                _angle = ((_angle+1) % 360);
                setValueByAngle(angle);
                Invalidate();
            }
            else if (e.KeyCode == Keys.Left)
            {
                _angle = ((_angle - 1) % 360);
                setValueByAngle(angle);
                Invalidate();
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                MouseLocation = e.Location;
                Point PointLocation = CalculateNewLocation(MouseLocation, Width / 2 - 10);
                Invalidate();
            }
        }

        public event EventHandler MyClickEvent
        {
            add
            { this.Click += value; }
            remove
            { this.Click -= value; }
        }

        Point MouseLocation;
        private Point CalculateNewLocation(Point e, int R)
        {
            Point result = e;

            int X = e.X - Width / 2;
            int Y = e.Y - Height / 2;
            if (X == 0) X = 1;
            _angle = Math.Atan2(Y - 0, X - 0);
            result.X = (int)(Math.Cos(_angle) * R) + Width / 2;
            result.Y = (int)(Math.Sin(_angle) * R) + Height / 2;
            _angle = _angle / Math.PI * 180 + 180;
            setValueByAngle(angle);
            return result;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            BackColor = Color.Transparent;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (IsType1)
            {
                e.Graphics.DrawImage(
                    RotateImage(Budzisz.Properties.Resources.type1Front, convertToFloat(_angle - 90)),
                    ClientRectangle);
                e.Graphics.DrawImage(Budzisz.Properties.Resources.type1BG,ClientRectangle);
                e.Graphics.FillEllipse(new SolidBrush(PotentiometerColor),ClientRectangle);
            }
            else if (IsType2)
            {
                e.Graphics.DrawImage(
                    RotateImage(Budzisz.Properties.Resources.Type2Front, convertToFloat(_angle - 90)),
                    ClientRectangle);
                e.Graphics.DrawImage(Budzisz.Properties.Resources.type2BG, ClientRectangle);
                e.Graphics.FillEllipse(new SolidBrush(PotentiometerColor), ClientRectangle);
            }
            else if (IsType3)
            {
                e.Graphics.DrawImage(
                    RotateImage(Budzisz.Properties.Resources.type3Front, convertToFloat(_angle - 90)),
                    ClientRectangle);
               e.Graphics.DrawImage(Budzisz.Properties.Resources.type3BG, 
                    ClientRectangle);
                e.Graphics.FillEllipse(new SolidBrush(PotentiometerColor), ClientRectangle);
            }

            //e.Graphics.DrawString(angle.ToString(), new Font("Arial", 12), new SolidBrush(Color.Blue), 0, 0);
            e.Graphics.DrawString(((int)Value).ToString(), new Font("Arial", 12), new SolidBrush(Color.Black), 0, 0);
            
            //narysowanie środka
            double mid = min+(Math.Abs(min) + Math.Abs(max)/2);
            e.Graphics.FillEllipse(new SolidBrush(Color.Red), Width / 2 - 3, Height - 10, 6, 10);
            e.Graphics.DrawString( mid.ToString(), new Font("Arial", 12), new SolidBrush(Color.Black), Width/2-15, Height-25);
        }
        public static Bitmap RotateImage(Bitmap b, float angle)
        {
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            Graphics g = Graphics.FromImage(returnBitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
            g.DrawImage(b, 0, 0, b.Width, b.Height);
            return returnBitmap;
        }
        private float convertToFloat(double input)
        {
            float result = (float)input;
            if (float.IsPositiveInfinity(result))
            {
                result = float.MaxValue;
            }
            else if (float.IsNegativeInfinity(result))
            {
                result = float.MinValue;
            }
            return result;
        }
    }
}