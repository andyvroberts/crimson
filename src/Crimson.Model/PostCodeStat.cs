
namespace Crimson.Model
{
    public class PostCodeStat
    {
        private string _outcode;
        public string Outcode
        {
            get { return _outcode; }
        }
    
        private string _postcode;
        public string Postcode
        {
            get { return _postcode; }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
        }
             
        private int _priceCount;
        public int PriceCount
        {
            get { return _priceCount; }
        }

        public PostCodeStat(string postcode, string fileName, int priceCount)
        {
            _outcode = postcode.Split(' ')[0];
            _postcode = postcode;
            _fileName = fileName;
            _priceCount = priceCount;
        }
    }
}