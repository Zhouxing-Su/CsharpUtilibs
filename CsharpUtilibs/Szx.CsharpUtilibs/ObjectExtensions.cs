#define USE_TYPE_EXTENSION_IN_IS_PRINTABLE


namespace IDeal.Szx.CsharpUtilibs
{
    internal static class ObjectExtensions
    {
        public static bool IsPrintable(this object obj) {
#if USE_TYPE_EXTENSION_IN_IS_PRINTABLE
            return ((obj == null) || obj.GetType().IsPrintable());
#else
            return ((obj == null) || obj.GetType().IsPrimitive || (obj is string));
#endif
        }
    }
}
