namespace Visualizesse.API;

public static class IdentificadonHeader
{
    public static string GetHeader(HttpContext httpContext)
    {
        return httpContext.Request.Headers["x-user-identification"].ToString();
    }
}