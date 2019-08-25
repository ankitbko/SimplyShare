using System.Threading.Tasks;
using SimplyShare.Tracker.Models;

namespace SimplyShare.Tracker.Repository
{
    public interface ISharingContextRepository
    {
        Task CreateSharingContext(SharingContext context);
        Task<SharingContext> GetSharingContextForUserByInfoHash(string userId, string infoHash);
    }
}