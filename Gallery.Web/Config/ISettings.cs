namespace Gallery.Web.Config
{
    public interface ISettings
    {
        string HashedPassword { get; }
        string TextMapFilePath { get; }
        int ThumbnailHeight { get; }
        string UploadImageRootPath { get; }
    }
}