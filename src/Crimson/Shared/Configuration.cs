using System;

namespace Crimson.Shared
{
    public class Configuration
    {
        public string LocalFileLocation = "downloads/postcodes";
        public string LocalFileName = "pp-monthly.csv";

        public string FileExtension = ".dat.gz";

        public string WebFileLocation = "http://prod.publicdata.landregistry.gov.uk.s3-website-eu-west-1.amazonaws.com/pp-monthly-update-new-version.csv";
    }
}