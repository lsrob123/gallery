namespace Gallery.Web.Abstractions
{
    public interface ISettings
    {
        string AlbumRootPath { get; }
        string Database { get; }
        string DefaultThumbnailUriPathForAlbum { get; }
        string HashedPassword { get; }
        string TextMapFilePath { get; }
        int UploadImageIconHeight { get; }
        string UploadImageRootPath { get; }
        int UploadImageThumbnailHeight { get; }
    }
}