﻿using System;
using System.ComponentModel;
using System.Drawing;

namespace SimpleCAD
{
    public class Arc : Drawable
    {
        private Point2D center;
        private float radius;
        private float startAngle;
        private float endAngle;

        public Point2D Center { get => center; set { center = value; NotifyPropertyChanged(); } }
        public float Radius { get => radius; set { radius = value; NotifyPropertyChanged(); } }
        public float StartAngle { get => startAngle; set { startAngle = value; NotifyPropertyChanged(); } }
        public float EndAngle { get => endAngle; set { endAngle = value; NotifyPropertyChanged(); } }

        [Browsable(false)]
        public float X { get { return Center.X; } }
        [Browsable(false)]
        public float Y { get { return Center.Y; } }

        public Arc(Point2D center, float radius, float startAngle, float endAngle)
        {
            Center = center;
            Radius = radius;
            StartAngle = startAngle;
            EndAngle = endAngle;
        }

        public Arc(float x, float y, float radius, float startAngle, float endAngle)
            : this(new Point2D(x, y), radius, startAngle, endAngle)
        {
            ;
        }

        public override void Draw(DrawParams param)
        {
            using (Pen pen = OutlineStyle.CreatePen(param))
            {
                // Represent curved features by at most 4 pixels
                float sweep = EndAngle - StartAngle;
                while (sweep < 0) sweep += 2 * (float)Math.PI;
                while (sweep > 2 * (float)Math.PI) sweep -= 2 * (float)Math.PI;
                float curveLength = param.ModelToView(sweep * Radius);
                int n = (int)Math.Max(4, curveLength / 4);
                float a = StartAngle;
                float da = sweep / (float)n;
                PointF[] pts = new PointF[n + 1];
                for (int i = 0; i <= n; i++)
                {
                    pts[i] = new PointF(X + Radius * (float)Math.Cos(a), Y + Radius * (float)Math.Sin(a));
                    a += da;
                }
                param.Graphics.DrawLines(pen, pts);
            }
        }

        public override Extents GetExtents()
        {
            Extents extents = new Extents();
            extents.Add(X - Radius, Y - Radius);
            extents.Add(X + Radius, Y + Radius);
            return extents;
        }

        public override void TransformBy(TransformationMatrix2D transformation)
        {
            Point2D p = Center;
            p.TransformBy(transformation);
            Center = p;

            Vector2D dir = Vector2D.XAxis * Radius;
            dir.TransformBy(transformation);
            Radius = dir.Length;

            Vector2D a1 = Vector2D.FromAngle(StartAngle);
            Vector2D a2 = Vector2D.FromAngle(EndAngle);
            a1.TransformBy(transformation);
            a2.TransformBy(transformation);
            StartAngle = a1.Angle;
            EndAngle = a2.Angle;
        }

        public override bool Contains(Point2D pt, float pickBoxSize)
        {
            Vector2D dir = pt - Center;
            float dist = dir.Length;
            return (dist >= Radius - pickBoxSize / 2 && dist <= Radius + pickBoxSize / 2 &&
                dir.IsBetween(Vector2D.FromAngle(StartAngle), Vector2D.FromAngle(EndAngle)));
        }
    }
}
