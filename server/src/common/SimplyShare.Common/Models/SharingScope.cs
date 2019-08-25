using System;

namespace SimplyShare.Core.Models
{
    [Flags]
    public enum SharingScope
    {
        Public,
        Internal,
        Both = Public | Internal
    }
}
