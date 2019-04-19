using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Fields
{
    public class CandidateFields
    {
        public const string ApiUrl = "apiUrl";
        public const string TokenType = "TokenType";
        public const string AccessToken = "AccessToken";
        public const string ApplicationJson = "application/json";
        public const string GetJobDetailsById = "/api/candidate/getjobdetailsbyid/";
        public const string Jobs = "/api/candidate/jobs";
        public const string GetAppliedJobs = "/api/candidate/getappliedjobs";
        public const string GetSavedJobs = "/api/candidate/getsavedjobs";
        public const string GetNotInterestedJobs = "/api/candidate/getnotinterestedjobs";
        public const string GetJobExplore = "/api/candidate/getjobexplore";
        public const string ApplyJob = "/api/candidate/applyjob";
        public const string ApplyNow="Apply Now";
        public const string SaveThisJob = "Save this Job";
        public const string NotInterested="Not Interested";
        public const string OrderByKeyWordASC = "Title Asc";
        public const string OrderByKeyWordDESC = "Title Desc";
        public const string OrderByCompanyASC = "Company Asc";
        public const string OrderByCompanyDESC = "Company Desc";
        public const string OrderByRecentASC = "Recent";
        public const string OrderByRecentDESC = "Recent Desc";
        public const string OrderBy="Order By";
        public const string SaveViewDetails = "/api/candidate/saveviewdetails";
        public const string GetStateByCountryId = "/api/candidate/getstatebycountryid/";
        public const string GetPhoneFormat = "/api/candidate/getphoneformat/";
        public const string Preferences = "/api/candidate/preferences/";
        public const string Education = "/api/candidate/education/";
        public const string Basic = "/api/candidate/basic/";
        public const string Contact = "/api/candidate/contact/";
        public const string Save = "Save";
        public const string PreviwSave = "PreviwSave";
        public const string Work = "/api/candidate/work/";
        public const string EnableProfileOption = "/api/candidate/enableprofileoption/";
        public const string Details="/details";
        public const string Remove="/remove";
        public const string Preview = "/api/candidate/preview/";
        public const string Dashboard = "/api/candidate/dashboard/";
        public const string JobDashboard = "/api/candidate/allJobs/"; 
        public const string CallForInterview = "/api/candidate/callforinterview/";
        public const string GetNotificationDetailsById = "/api/candidate/getnotificationbyId/";
        public const string GetShortNotificationDetailsById = "/api/candidate/getshortnotificationbyId/";
        public const string ShortListNotification = "/api/candidate/shortlist";
        public const string SendNotificationOnEmail = "/api/account/sendnotification";

        public const string NotificationRead = "/notificationread";
        public const string DownloadResume = "/api/candidate/downloadresume/";
    }
}
