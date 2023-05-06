using System;

namespace Crimson.Core.Shared
{
    public class Configuration
    {
        public string LocalFileLocation = "downloads/postcodes";
        public string LocalFileName = "pp-complete.csv";
        public string LocalFileName2 = "pp-2022.csv";
        public string LocalFileName3 = "pp-monthly.csv";

        public string FileExtension = ".dat.gz";

        public string WebFileLocation = "http://prod.publicdata.landregistry.gov.uk.s3-website-eu-west-1.amazonaws.com/pp-2022.csv";
        public string WebFileMonthly = "http://prod.publicdata.landregistry.gov.uk.s3-website-eu-west-1.amazonaws.com/pp-monthly-update-new-version.csv";
    }
}