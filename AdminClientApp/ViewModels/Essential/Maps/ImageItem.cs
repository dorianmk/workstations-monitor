using System.IO;
using System.Windows.Media.Imaging;

namespace AdminClientApp.ViewModels.Essential.Maps
{
    public class ImageItem : MapItemBase
    {
        public byte[] Bytes { get; }

        internal ImageItem(string filePath)
            : base(0.4, 0.4, 0.2, 0.1, 0, true)
        {
            Bytes = File.ReadAllBytes(filePath);
        }

        internal ImageItem(byte[] file, double relativeTop, double relativeLeft, double relativeWidth, double relativeHeight, int layer)
            : base(relativeTop, relativeLeft, relativeWidth, relativeHeight, layer)
        {
            Bytes = file;
        }

        private BitmapImage image;
        public BitmapImage ImageSource
        {
            get
            {
                if (image == null)
                {
                    using (var ms = new MemoryStream(Bytes))
                    {
                        image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = ms;
                        image.EndInit();
                    }
                }
                return image;
            }
        }
    }
}
