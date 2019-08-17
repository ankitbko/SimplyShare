using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SimplyShare.Tracker.Exceptions
{
    public class DuplicateSharingContextException : Exception
    {
        public DuplicateSharingContextException()
        {
        }

        public DuplicateSharingContextException(string message) : base(message)
        {
        }

        public DuplicateSharingContextException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateSharingContextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
