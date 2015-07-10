using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Szx.CsharpUtilibs
{
    public static class FieldInfoExtensions
    {
        private static char[] BackingFieldNameDelimiters = new char[] { '<', '>' };

        // UPDATE[5]: use a more compatible/portable way to achieve it!
        public static string GetFriendlyName(this FieldInfo fieldInfo) {
            return ((fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute)))
                ? fieldInfo.Name.Split(BackingFieldNameDelimiters, StringSplitOptions.RemoveEmptyEntries)[0]
                : fieldInfo.Name);
        }
    }
}
