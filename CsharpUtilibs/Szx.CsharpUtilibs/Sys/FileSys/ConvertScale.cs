using System;


namespace IDeal.Szx.CsharpUtilibs.Sys.FileSys {
    using TQuantity = Double;

    public enum EScale { B, KB, MB, GB, TB, PB, EB, ZB, YB }

    public class Size {
        public Size(TQuantity quantity, EScale scale = EScale.B) {
            Quantity = quantity;
            Scale = scale;
        }

        public override string ToString() { return Quantity.ToString("f1") + Scale.ToString(); }

        public TQuantity Quantity { get; set; }
        public EScale Scale { get; set; }
    }

    public class ConvertScale {
        public const TQuantity Base = 1024;
        public const TQuantity KiloBase = Base;
        public const TQuantity MegaBase = Base * KiloBase;
        public const TQuantity GigaBase = Base * MegaBase;
        public const TQuantity TeraBase = Base * GigaBase;
        public const TQuantity PetaBase = Base * TeraBase;
        public const TQuantity ExaBase = Base * PetaBase;
        public const TQuantity ZettaBase = Base * ExaBase;
        public const TQuantity YottaBase = Base * ZettaBase;


        public static Size toProper(TQuantity bytes) {
            EScale scale = EScale.B;
            for (; bytes > Base; ++scale) { bytes /= Base; }
            return new Size(bytes, scale);
        }

        public static Size toKB(TQuantity bytes) {
            return new Size(bytes /= KiloBase, EScale.KB);
        }

        public static Size toMB(TQuantity bytes) {
            return new Size(bytes /= MegaBase, EScale.MB);
        }

        public static Size toGB(TQuantity bytes) {
            return new Size(bytes /= GigaBase, EScale.GB);
        }

        public static Size toTB(TQuantity bytes) {
            return new Size(bytes /= TeraBase, EScale.TB);
        }

        public static Size toPB(TQuantity bytes) {
            return new Size(bytes /= PetaBase, EScale.PB);
        }

        public static Size toEB(TQuantity bytes) {
            return new Size(bytes /= ExaBase, EScale.EB);
        }

        public static Size toZB(TQuantity bytes) {
            return new Size(bytes /= ZettaBase, EScale.ZB);
        }

        public static Size toYB(TQuantity bytes) {
            return new Size(bytes /= YottaBase, EScale.YB);
        }
    }
}
