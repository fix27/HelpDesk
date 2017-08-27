using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace HelpDesk.Common.Helpers
{
    public static class StringHelper
    {
        public static string ToFirstLetterUpper(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static string ToPhoneList(this string str)
        {
            if (str == null)
                return null;

            string p = "";
            string[] phones = str.Split(',');
            foreach (string s in phones)
                if (!String.IsNullOrWhiteSpace(s))
                    p += Regex.Replace(s, "[^0-9+]", "") + ", ";
            if (!String.IsNullOrWhiteSpace(p))
                p = p.Substring(0, p.Length - 2);

            return p;
        }

        public static T Convert<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(input);
                }
                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }

        public static T[] ToArrayValues<T>(this string listStr)
        {
            if (String.IsNullOrWhiteSpace(listStr))
                return null;

            T v = default(T);
            string[] values = listStr.Split(',');
            IList<T> list = new List<T>();
            foreach (string s in values)
            {
                list.Add(s.Convert<T>());
            }

            return list.ToArray();
        }
    }
}
