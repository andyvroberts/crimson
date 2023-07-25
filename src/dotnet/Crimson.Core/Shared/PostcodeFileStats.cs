using Crimson.Model;

namespace Crimson.Core.Shared
{
    public class PostcodeFileStats : IExportStats
    {
        private List<PostCodeStat> fileStats = new();

        public void AddPostcodeStat(string postCode, string fileName, int priceCount)
        {
            fileStats.Add(new PostCodeStat(postCode, fileName, priceCount));
        }

        public IEnumerable<PostCodeStat> GetAllStats()
        {
            return fileStats;
        }
    }
}