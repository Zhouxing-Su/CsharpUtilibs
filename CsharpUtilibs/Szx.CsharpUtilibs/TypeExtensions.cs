using System;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace Szx.CsharpUtilibs
{
    internal static class TypeExtensions
    {
        public static bool IsPrintable(this Type t) {
            return (t.IsPrimitive || (t == typeof(string)));
        }

        public static bool CheckIsPrintable(Type t) {
            return t.IsPrintable();
        }
    }

    internal static class FieldInfoExtensions
    {
        private static char[] BackingFieldNameDelimiters = new char[] { '<', '>' };

        // TUNEUP[5]: use a more compatible/portable way to achieve it!
        public static string GetFriendlyName(this FieldInfo fieldInfo) {
            return ((fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute)))
                ? fieldInfo.Name.Split(BackingFieldNameDelimiters, StringSplitOptions.RemoveEmptyEntries)[0]
                : fieldInfo.Name);
        }
    }
}
