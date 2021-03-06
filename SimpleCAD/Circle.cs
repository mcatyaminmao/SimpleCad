﻿using System;
using System.ComponentModel;
using System.Drawing;

namespace SimpleCAD
{
    public class Circle : Drawable
    {
        private Point2D center;
        private float radius;

        public Point2D Center { get => center; set { center = value; NotifyPropertyChanged(); } }
        public float Radius { get => radius; set { radius = value; NotifyPropertyChanged(); } }

        [Browsable(false)]
        public float X { get { return Center.X; } }
        [Browsable(false)]
        public float Y { get { return Center.Y; } }

        public Circle(Point2D center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public Circle(float x, float y, float radius)
            : this(new Point2D(x, y), radius)
        {
            ;
        }

        public override void Draw(DrawParams param)
        {
            using (Brush brush = FillStyle.CreateBrush(param))
            {
                param.Graphics.FillEllipse(brush, X - Radius, Y - Radius, 2f * Radius, 2f * Radius);
            }
            using (Pen pen = OutlineStyle.CreatePen(param))
            {
                param.Graphics.DrawEllipse(pen, X - Radius, Y - Radius, 2f * Radius, 2f * Radius);
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
        }

        public override bool Contains(Point2D pt, float pickBoxSize)
        {
            float dist = (pt - Center).Length;
            return dist <= Radius + pickBoxSize / 2 && dist >= Radius - pickBoxSize / 2;
        }
    }
}
