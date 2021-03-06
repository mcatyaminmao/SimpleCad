﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SimpleCAD
{
    [TypeConverter(typeof(OutlineStyleConverter))]
    public partial struct OutlineStyle
    {
        public Color Color { get; set; }
        public float LineWeight { get; set; }
        public DashStyle DashStyle { get; set; }

        public OutlineStyle(Color color, float lineWeight, DashStyle dashStyle)
            : this()
        {
            Color = color;
            LineWeight = lineWeight;
            DashStyle = dashStyle;
        }

        public OutlineStyle(Color color, float lineWeight)
            : this(color, lineWeight, DashStyle.Solid)
        {
            ;
        }

        public OutlineStyle(Color color)
            : this(color, 0, DashStyle.Solid)
        {
            ;
        }

        public Pen CreatePen(DrawParams param)
        {
            if (param.SelectionMode)
            {
                Pen pen = new Pen(param.SelectionColor, param.GetScaledLineWeight(LineWeight + 6));
                pen.DashStyle = DashStyle.Solid;
                return pen;
            }
            else
            {
                Pen pen = new Pen(Color, param.GetScaledLineWeight(LineWeight));
                pen.DashStyle = DashStyle;
                return pen;
            }
        }
    }
}
