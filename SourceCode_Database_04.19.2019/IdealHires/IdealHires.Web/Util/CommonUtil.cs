using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdealHires.Web.Util
{
    public class CommonUtil
    {
        public static bool UploadFileToS3(System.IO.Stream localFilePath, string bucketName, string fileNameInS3)
        {
            try
            {
                IAmazonS3 client = new AmazonS3Client(RegionEndpoint.USEast1);
                TransferUtility utility = new TransferUtility(client);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
                request.BucketName = bucketName;
                request.Key = fileNameInS3;
                request.InputStream = localFilePath;
                utility.Upload(request); //commensing the transfer
                return true; //indicate that the file was sent
            }
            catch (AmazonS3Exception ex)
            {
                throw;
            }
        }

    }
    enum UserTypeData
    {
        Candidate = 1,
        Employer = 2,
        IdealHiresAdmin = 3
    }
}