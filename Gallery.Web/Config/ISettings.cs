namespace Gallery.Web.Config
{
    public interface ISettings
    {
        string HashedPassword { get; }
        string TextMapFilePath { get; }
    }
}