using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimplyShare.Core.Models
{
    public class AnnounceResponse
    {
        public AnnounceResponse()
        { }

        public AnnounceResponse(string failureReason)
        {
            FailureReason = failureReason;
        }

        public string FailureReason { get; set; }

        public string WarningMessage { get; set; }

        public int Interval { get; set; }

        public int? MinimumInterval { get; set; }

        public string TrackerId { get; set; }

        public int Complete { get; set; }

        public int Incomplete { get; set; }

        public IEnumerable<Peer> Peers { get; set; }
    }

    public class Peer
    {
        public Peer(string peerId, string ip, int port)
        {
            Peer_Id = peerId;
            Ip = ip;
            Port = port;
        }

        public string Peer_Id { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }
    }
}
