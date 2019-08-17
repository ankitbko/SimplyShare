using System.Threading.Tasks;
using SimplyShare.Common.Models;

namespace SimplyShare.Tracker.Operations
{
    public interface ISharingOperation
    {
        Task StartSharing(ShareRequest request);
    }
}