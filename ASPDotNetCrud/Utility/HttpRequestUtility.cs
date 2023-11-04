namespace ASPDotNetCrud.Utility
{
    public static class HttpRequestUtility
    {

        public static T? GETrequest<T>(string what, HttpContext httpContext) where T : struct
        {
            if (httpContext.Request.Query.TryGetValue(what, out var values) && values.Count > 0)
            {
                string value = values[0];
                return (T)Convert.ChangeType(value, typeof(T));
            }
            return null;
        }


    }
}
