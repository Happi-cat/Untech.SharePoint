using System;

namespace Untech.SharePoint.Core
{
    internal static class Guard
    {
	    public static void NotNull(object obj, string paramName)
	    {
		    if (obj == null)
		    {
			    throw new ArgumentNullException(paramName);
		    }
	    }


        public static void ThrowIfNot<T>(object obj, string message)
        {
            if (!(obj is T))
            {
                throw new ArgumentException(message);
            }
        }
    }
}