using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICSLicenseMaintV2.ViewModels
{
    public class AlertModel
    {
        public string Type { get; set; }
        public string Text { get; set; }

        public static AlertModel Success(string text)
        {
            return new AlertModel { Type = "success", Text = text };
        }

        public static AlertModel Warning(string text)
        {
            return new AlertModel { Type = "warning", Text = text };
        }

        public static AlertModel Error(string text)
        {
            return new AlertModel { Type = "danger", Text = text };
        }
    }
}