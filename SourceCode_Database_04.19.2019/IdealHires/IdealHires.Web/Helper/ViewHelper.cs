using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdealHires.Web.Helper
{
    public static class ViewHelper
    {
        public static string GetJobTypeClass(string value)
        {
            string cssClassName = string.Empty;
            switch (value)
            {
                case "Full Time":
                    {
                        cssClassName = "job-type-freelance";
                        break;
                    }
                case "Contractor":
                    {
                        cssClassName = "job-type-freelance";
                        break;
                    }
                case "Part Time":
                    {
                        cssClassName = "job-type-part-time";
                        break;
                    }
                case "Intern":
                    {
                        cssClassName = "job-type-contract";
                        break;
                    }
                case "Seasonal":
                    {
                        cssClassName = "job-type-seasonal";
                        break;
                    }
            }
            return string.Empty;
        }
    }
}