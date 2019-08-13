using System;

namespace SimplyShare.Core
{
    [Flags]
    public enum SharingScope
    {
        Public,
        Internal,
        Both = Public | Internal
    }
}
