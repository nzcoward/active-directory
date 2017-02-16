using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Arc.ActiveDirectory.Web
{
    public static class Constants
    {
        public static bool OptimizeBundles
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["OptimizeBundles"]); }
        }

        public static string ApiSecret
        {
            get { return ConfigurationManager.AppSettings["ApiSecret"]; }
        }

        public static string ApiUrl
        {
            get { return ConfigurationManager.AppSettings["ApiUrl"]; }
        }

        public static string UsersEndPoint
        {
            get { return ConfigurationManager.AppSettings["UsersEndPoint"]; }
        }

        public static string ResetPasswordEndPoint
        {
            get { return ConfigurationManager.AppSettings["ResetPasswordEndPoint"]; }
        }

        public static bool EncryptConfig
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["EncryptConfig"]); }
        }
    }
}