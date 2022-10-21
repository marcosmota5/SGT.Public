using System.IO;
using System.Windows.Media.Imaging;

namespace SGT.HelperClasses
{
    public class ImageClasses
    {
        public static BitmapImage LoadImage(byte[] imageData)
        {
#pragma warning disable CS8603 // Possible null reference return.
            if (imageData == null || imageData.Length == 0) return null;
#pragma warning restore CS8603 // Possible null reference return.
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}