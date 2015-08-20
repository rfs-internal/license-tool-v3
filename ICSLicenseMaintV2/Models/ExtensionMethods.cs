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
    }
}