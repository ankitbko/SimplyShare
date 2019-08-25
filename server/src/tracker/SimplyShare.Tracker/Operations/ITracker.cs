using System.Threading.Tasks;
using SimplyShare.Core.Models;

namespace SimplyShare.Tracker.Operations
{
    public interface ITracker
    {
        Task<AnnounceResponse> Announce(AnnounceRequest request, string userId);
    }
}