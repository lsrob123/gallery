namespace Gallery.Web.Abstractions
{
    public interface ISettings
    {
        string HashedPassword { get; }
        string TextMapFilePath { get; }
        int ThumbnailHeight { get; }
        string UploadImageRootPath { get; }
        string Database { get; }
        string DefaultThumbnailUriPathForAlbum { get; }
    }
}