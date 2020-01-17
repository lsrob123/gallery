namespace Gallery.Web.Abstractions
{
    public interface ISettings
    {
        string HashedPassword { get; }
        string TextMapFilePath { get; }
        int ThumbnailHeight { get; }
        string UploadImageRootPath { get; }
        string ConnectionString { get; }
        string DefaultThumbnailUriPathForAlbum { get; }
    }
}