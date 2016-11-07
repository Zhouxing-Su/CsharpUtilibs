using System;
using System.Collections.Generic;


namespace IDeal.Szx.CsharpUtilibs.Geometry {
    using Number = Double;

    public class Circle {
        public Circle(Number cx, Number cy, Number cr) {
            x = cx;
            y = cy;
            r = cr;
        }

        public static List<Point2D> intersectionPoints(Circle c0, Circle c1) {
            return intersectionPoints(c0.x, c0.y, c0.r, c1.x, c1.y, c1.r);
        }
        public static List<Point2D> intersectionPoints(Number x0, Number y0, Number r0, Number x1, Number y1, Number r1) {
            List<Point2D> l = new List<Point2D>();
            double a, dx, dy, d, h, rx, ry;
            double x2, y2;

            // dx and dy are the vertical and horizontal distances between the circle centers.
            dx = x1 - x0;
            dy = y1 - y0;

            // determine the straight-line distance between the centers. 
            d = Math.Sqrt(dx * dx + dy * dy);

            // no solution. circles do not intersect. 
            if (d > (r0 + r1)) { return l; }
            // no solution. one circle is contained in the other.
            if (d < Math.Abs(r0 - r1)) { return l; }

            // 'point 2' is the point where the line through the circle intersection
            //  points crosses the line between the circle centers.

            // determine the distance from point 0 to point 2.
            a = ((r0 * r0) - (r1 * r1) + (d * d)) / (2.0 * d);

            // determine the coordinates of point 2.
            x2 = x0 + (dx * a / d);
            y2 = y0 + (dy * a / d);

            // determine the distance from point 2 to either of the intersection points.
            h = Math.Sqrt((r0 * r0) - (a * a));

            // Now determine the offsets of the intersection points from point 2.
            rx = dy * (h / d);
            ry = dx * (h / d);

            // determine the absolute intersection points.
            l.Add(new Point2D(x2 + rx, y2 - ry));
            l.Add(new Point2D(x2 - rx, y2 + ry));

            return l;
        }

        public Number x;
        public Number y;
        public Number r;
    }
}
