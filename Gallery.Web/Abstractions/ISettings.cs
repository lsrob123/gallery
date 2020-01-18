namespace Gallery.Web.Abstractions
{
    public interface ISettings
    {
        string AlbumRootPath { get; }
        string Database { get; }
        string DefaultThumbnailUriPathForAlbum { get; }
        string HashedPassword { get; }
        string TextMapFilePath { get; }
        int ThumbnailHeight { get; }
        string UploadImageRootPath { get; }
    }
}