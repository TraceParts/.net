using System.Web;

namespace tracepartsApi_NET8.utils;

public abstract class MyUrlEncoder
{
    public static string Encode(string rawString)
    {
        // This is to avoid forbidden characters in URLs
        return HttpUtility.UrlEncode(rawString);
    }
}