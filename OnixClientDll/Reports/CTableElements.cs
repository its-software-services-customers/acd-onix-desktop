using System;
using System.Windows;
using System.Collections;
using System.Windows.Media;
using Onix.Client.Helper;

namespace Onix.Client.Report
{
    public class CText
    {
        private String text = "";

        public CText(String txt)
        {
            text = txt;
        }

        public String Text
        {
            get
            {
                return (text);
            }

            set
            {
                text = value;
            }
        }
    }

    public class CColumn
    {
        private CText text = new CText("");
        private Color txtClr = Colors.Black;
        private Brush bdClr = Brushes.Black;
        private Thickness thickness = new Thickness(0.5, 05, 0.5, 0.5);
        private VerticalAlignment valign = VerticalAlignment.Center;
        private HorizontalAlignment halign = HorizontalAlignment.Center;
        private GridLength w = GridLength.Auto;
        private String dataType = "S";

        public CColumn(Color txtColor, Brush borderColor, Thickness borderThickness, VerticalAlignment verticalAlign, HorizontalAlignment horizontalAlignment)
        {
            txtClr = txtColor;
            bdClr = borderColor;
            thickness = borderThickness;
            valign = verticalAlign;
            halign = horizontalAlignment;
        }

        public CColumn(Thickness borderThickness, GridLength width)
        {
            thickness = borderThickness;
            w = width;
        }

        public void SetDataType(String type)
        {
            dataType = type;
        }

        public String getDataType()
        {
            return (dataType);
        }

        public void SetText(CText txt)
        {
            text = txt;
        }

        public void SetVerticalAlignment(VerticalAlignment v)
        {
            valign = v;
        }

        public void SetHorizontalAlignment(HorizontalAlignment h)
        {
            halign = h;
        }

        public CColumn Clone()
        {
            CColumn nc = new CColumn(txtClr, bdClr, thickness, valign, halign);
            nc.SetWidth(this.GetWidth());
            nc.SetDataType(this.getDataType());

            return (nc);
        }

        public CText GetText()
        {
            return (text);
        }

        public Brush GetBorderColor()
        {
            return (bdClr);
        }

        public HorizontalAlignment GetHorizontalAlignment()
        {
            return (halign);
        }

        public VerticalAlignment GetVertocalAlignment()
        {
            return (valign);
        }

        public Thickness GetBorderThickness()
        {
            return (thickness);
        }

        public GridLength GetWidth()
        {
            return (w);
        }

        public void SetWidth(GridLength width)
        {
            w = width;
        }
    }

    public class CRow
    {
        private Double h = 0.00;
        private int column = 1;
        private ArrayList columns = new ArrayList();
        private String nm = "";
        private Thickness margin = new Thickness(3, 3, 3, 3);

        private FontFamily ff = null;
        private FontStyle fs = FontStyles.Normal;
        private int fz = 0;
        private FontWeight fw = FontWeights.Normal;

        public CRow(String name, Double height, int columnCount)
        {
            column = columnCount;
            h = height;
            nm = name;
        }

        public CRow(String name, Double height, int columnCount, Thickness mg)
        {
            column = columnCount;
            h = height;
            nm = name;
            margin = mg;
        }

        public FontFamily GetFontFamily()
        {
            return (ff);
        }

        public FontStyle GetFontStyle()
        {
            return (fs);
        }

        public int GetFontSize()
        {
            return (fz);
        }

        public FontWeight GetFontWeight()
        {
            return (fw);
        }

        public Thickness GetMargin()
        {
            return (margin);
        }

        public Double GetHeight()
        {
            return (h);
        }

        public int GetColumnCount()
        {
            return (column);
        }

        public CColumn GetColumn(int idx)
        {
            return((CColumn) columns[idx]);
        }

        public void AddColumn(CColumn c)
        {
            columns.Add(c);
        }

        public String GetName()
        {
            return (nm);
        }

        public void SetFont(FontFamily ffamily, FontStyle fstyle, int fsize, FontWeight fweight)
        {
            ff = ffamily;
            fs = fstyle;
            fz = fsize;
            fw = fweight;
    }

        public void SetMargin(Thickness mrg)
        {
            this.margin = mrg;
        }

        public CRow Clone()
        {
            CRow nrw = new CRow(nm, h, column);

            foreach (CColumn c in columns)
            {
                CColumn o = c.Clone();
                nrw.AddColumn(o);
            }

            nrw.SetMargin(this.margin);
            nrw.SetFont(this.ff, this.fs, this.fz, this.fw);
            
            return(nrw);
        }

        public void FillColumnsText(params String[] values)
        {
            int i = 0;

            foreach (String value in values)
            {
                CColumn c = (CColumn)columns[i];
                c.GetText().Text = value;
                
                i++;
            }
        }

        public void FillColumnsText(ArrayList values)
        {
            int i = 0;
            foreach (String rc in values)
            {
                CColumn c = (CColumn)columns[i];
                c.GetText().Text = rc;

                i++;
            }
        }
    }
}
