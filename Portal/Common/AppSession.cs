using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Common
{
    public class AppSession
    {
        public static void SetSessionValue(string Key, string Value)
        {
            HttpContext.Current.Session[Key] = Value;
        }

        public static void SetSessionValue(string Key, object ObjValue)
        {
            HttpContext.Current.Session[Key] = ObjValue;
        }

        public static object GetSessionValue(string Key)
        {
            object obj = null;
            if (!string.IsNullOrEmpty(Key))
            {
                if (HttpContext.Current.Session[Key] != null)
                {
                    obj = (object)HttpContext.Current.Session[Key];
                }
            }
            return obj;
        }

        public static int GetSessionValueAsInt(string Key)
        {
            int iReturn = 0;
            if (!string.IsNullOrEmpty(Key))
            {
                if (HttpContext.Current.Session[Key] != null)
                {
                    iReturn = (int)HttpContext.Current.Session[Key];
                }
            }
            return iReturn;
        }

        public static string GetSessionValueAsString(string Key)
        {
            string sReturn = null;
            if (!string.IsNullOrEmpty(Key))
            {
                if (HttpContext.Current.Session[Key] != null)
                {
                    sReturn = HttpContext.Current.Session[Key].ToString();
                }
            }
            return sReturn;
        }
    }
}