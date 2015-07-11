using System;


namespace Szx.CsharpUtilibs
{
    internal static class TypeExtensions
    {
        public static bool IsPrintable(this Type t) {
            return (t.IsPrimitive || (t == typeof(string)));
        }
    }
}
