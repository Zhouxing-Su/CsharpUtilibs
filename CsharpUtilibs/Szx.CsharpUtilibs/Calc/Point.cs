using System;


namespace IDeal.Szx.CsharpUtilibs.Calc {
    using Number = Double;

    public class Point2D {
        public Point2D(Number px, Number py) {
            x = px;
            y = py;
        }

        public Number x;
        public Number y;
    }

    public class Point3D {
        public Point3D(Number px, Number py, Number pz) {
            x = px;
            y = py;
            z = pz;
        }

        public Number x;
        public Number y;
        public Number z;
    }
}
