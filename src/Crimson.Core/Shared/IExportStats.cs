using Crimson.Model;

namespace Crimson.Core.Shared
{
    public interface IExportStats
    {
        void AddPostcodeStat (string postCode, string fileName, int priceCount);

        IEnumerable<PostCodeStat> GetAllStats();
    }
}