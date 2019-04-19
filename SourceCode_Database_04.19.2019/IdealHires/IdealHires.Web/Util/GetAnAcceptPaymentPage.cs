using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using IdealHires.DTO.Employer;

namespace IdealHires.Web.Util
{
    public class GetAnAcceptPaymentPage
    {
        public static string Run(String ApiLoginID, String ApiTransactionKey, decimal amount, CompanyEmployee companyEmployee,string profileId)
        {
            string token = string.Empty;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            settingType[] settings = new settingType[10];

            settings[0] = new settingType();
            settings[0].settingName = settingNameEnum.hostedPaymentReturnOptions.ToString();
            settings[0].settingValue = "{\"showReceipt\": false, \"url\": \"https://idealhires-env-staging.5fvvpgghi7.us-east-2.elasticbeanstalk.com/Home/About\", \"urlText\": \"Continue\", \"cancelUrl\": \"https://idealhires-env-staging.5fvvpgghi7.us-east-2.elasticbeanstalk.com/Home/About\", \"cancelUrlText\": \"Cancel\"}";

            settings[1] = new settingType();
            settings[1].settingName = settingNameEnum.hostedPaymentButtonOptions.ToString();
            settings[1].settingValue = "{\"text\": \"Pay\"}";

            settings[2] = new settingType();
            settings[2].settingName = settingNameEnum.hostedPaymentStyleOptions.ToString();
            settings[2].settingValue = "{\"bgColor\": \"blue\"}";

            settings[3] = new settingType();
            settings[3].settingName = settingNameEnum.hostedPaymentPaymentOptions.ToString();
            settings[3].settingValue = "{\"cardCodeRequired\": false, \"showCreditCard\": true, \"showBankAccount\": true}";

            settings[4] = new settingType();
            settings[4].settingName = settingNameEnum.hostedPaymentSecurityOptions.ToString();
            settings[4].settingValue = "{\"captcha\": false}";

            settings[5] = new settingType();
            settings[5].settingName = settingNameEnum.hostedPaymentShippingAddressOptions.ToString();
            settings[5].settingValue = "{\"show\": false, \"required\": false}";

            settings[6] = new settingType();
            settings[6].settingName = settingNameEnum.hostedPaymentBillingAddressOptions.ToString();
            settings[6].settingValue = "{\"show\": true, \"required\": false}";

            settings[7] = new settingType();
            settings[7].settingName = settingNameEnum.hostedPaymentCustomerOptions.ToString();
            settings[7].settingValue = "{\"showEmail\": false, \"requiredEmail\": false, \"addPaymentProfile\": true}";

            settings[8] = new settingType();
            settings[8].settingName = settingNameEnum.hostedPaymentOrderOptions.ToString();
            settings[8].settingValue = "{\"show\": false}";

            settings[9] = new settingType();
            settings[9].settingName = settingNameEnum.hostedPaymentIFrameCommunicatorUrl.ToString();
            settings[9].settingValue = "{\"url\": \"https://idealhires-env-staging.5fvvpgghi7.us-east-2.elasticbeanstalk.com/IFrameCommunicator.html\"}";
            

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // authorize capture only
                amount = amount,
                profile = new customerProfilePaymentType()
                {
                    customerProfileId = profileId
                },
                customer = new customerDataType()
                {
                    email = companyEmployee.EmailAddress
                },
                billTo = new customerAddressType
                {
                    firstName = companyEmployee.FirstName,
                    lastName = companyEmployee.LastName,
                    address = companyEmployee.Address,
                    city = companyEmployee.City,
                    state = companyEmployee.StateCode,
                    zip = companyEmployee.Zip!=null ? Convert.ToString(companyEmployee.Zip) : string.Empty,
                    company = companyEmployee.CompanyName,
                    country = companyEmployee.Country,
                    phoneNumber=companyEmployee.Phone
                    //firstName = "Ellen",
                    //lastName = "Johnson",
                    //address = "14 Main Street",
                    //city = "Pecan Springs",
                    //state = "TX",
                    //zip = "44628",
                    //company = "Souveniropolis",
                    //country = "USA"
                }
            };

            var request = new getHostedPaymentPageRequest();
            request.transactionRequest = transactionRequest;
            request.hostedPaymentSettings = settings;

            // instantiate the controller that will call the service
            var controller = new getHostedPaymentPageController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            // validate response
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                token = response.token;
            }
            else if (response != null)
            {
                token = string.Empty;
            }

            return token;
        }
    }
}