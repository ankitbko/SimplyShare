using System.Threading.Tasks;
using SimplyShare.Common.Models;
using SimplyShare.Tracker.Models;

namespace SimplyShare.Tracker.Operations
{
    public interface ISharingOperation
    {
        Task<SharingContext> GetSharingContext(string userId, string infoHash);
        Task StartSharing(ShareRequest request);
    }
}