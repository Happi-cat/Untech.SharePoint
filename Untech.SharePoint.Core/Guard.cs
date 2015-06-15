using System;

namespace Untech.SharePoint.Core
{
    internal static class Guard
    {
        public static void ThrowIfNot<T>(object obj, string message)
        {
            if (!(obj is T))
            {
                throw new ArgumentException(message);
            }
        }
    }
}