namespace Gallery.Web.Config
{
    public enum ProcessResult
    {
        Unknown = 0, OK = 200, Created = 201, Accepted = 202, Found = 302, BadRequest = 400, NotFound = 404,
        InternalServerError = 500
    }
}
