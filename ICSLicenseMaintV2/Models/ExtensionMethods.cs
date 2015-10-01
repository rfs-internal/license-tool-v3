using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ICSLicenseMaintV2
{
    public partial class License
    {
        [DisplayName("Machine ID")]
        public string ShortMachineID
        {
            get
            {
                if(MachineID.Length > 28)
                {
                    return MachineID.Substring(0, 25) + "...";
                }
                return MachineID;
            }
        }

        [DisplayName("Is Permanent")]
        public bool IsPermanent
        {
            get
            {
                return !TimeOut;
            }
        }

        [DisplayName("Is Expired")]
        public bool IsExpired
        {
            get
            {
                if (TimeOut)
                {
                    return ExpiryDate < DateTime.Now;
                }
                return false;
            }
        }

        [DisplayName("Expiration Date")]
        public DateTime ExpiryDate
        {
            get
            {
                return DateIssued.AddDays(DaysRemaining);
            }
        }


        [DisplayName("Product Version")]
        public RfsmartVersion ProductVersion
        {
            get
            {
                // replace space, backslash, dot, and dash with space
                var path = System.Text.RegularExpressions.Regex.Replace(InstallPath ?? string.Empty, @"\s|\\|\.|-", "").ToLower();
                return path.Contains("rfsmart4") || path.Contains("rfsmart5") ? RfsmartVersion.V4 : RfsmartVersion.V3;
            }
        }
    }

    public enum RfsmartVersion
    {
        V3,
        V4
    }
}