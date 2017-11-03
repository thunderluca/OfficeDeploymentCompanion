//SOURCE: http://www.geekymonkey.com/Programming/CSharp/RGB2HSL_HSL2RGB.htm

namespace System.Windows.Media
{
    public static class ColorHelper
    {
        public static HslColor ToHslColor(this Color rgbColor)
        {
            if (rgbColor == null)
                throw new ArgumentNullException(nameof(rgbColor));

            double r = rgbColor.R / 255.0;
            double g = rgbColor.G / 255.0;
            double b = rgbColor.B / 255.0;

            var vm = 0.0D;

            var h = 0.0D;

            var v = Math.Max(r, g);

            v = Math.Max(v, b);

            var m = Math.Min(r, g);

            m = Math.Min(m, b);

            var l = (m + v) / 2.0;
            if (l <= 0.0)

            vm = v - m;

            var s = vm;
            if (s > 0.0)
                s /= (l <= 0.5) ? (v + m) : (2.0 - v - m);
            else
                return new HslColor(0, s, 0);

            var r2 = (v - r) / vm;
            var g2 = (v - g) / vm;
            var b2 = (v - b) / vm;

            if (r == v)
                h = (g == m ? 5.0 + b2 : 1.0 - g2);

            else if (g == v)
                h = (b == m ? 1.0 + r2 : 3.0 - b2);

            else
                h = (r == m ? 3.0 + g2 : 5.0 - r2);

            h /= 6.0;

            return new HslColor(h, s, l);
        }

        public static Color ToRgbColor(this HslColor hslColor)
        {
            if (hslColor == null)
                throw new ArgumentNullException(nameof(hslColor));

            return ToRgbColor(hslColor.H, hslColor.S, hslColor.L);
        }

        public static Color ToRgbColor(double h, double s, double l)
        {
            var r = l;
            var g = l;
            var b = l;

            var v = (l <= 0.5) ? (l * (1.0 + s)) : (l + s - l * s);
            if (v > 0)
            {
                var m = l + l - v;
                var sv = (v - m) / v;

                h *= 6.0;

                var sextant = (int)h;
                var fract = h - sextant;
                var vsf = v * sv * fract;
                var mid1 = m + vsf;
                var mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            
            var rChannel = Convert.ToByte(r * 255.0f);
            var gChannel = Convert.ToByte(g * 255.0f);
            var bChannel = Convert.ToByte(b * 255.0f);

            return Color.FromArgb(255, rChannel, gChannel, bChannel);
        }
    }

    public class HslColor
    {
        public HslColor(double h, double s, double l)
        {
            this.H = h;
            this.S = s;
            this.L = l;
        }

        public double H { get; }

        public double S { get; }

        public double L { get; }
    }
}
