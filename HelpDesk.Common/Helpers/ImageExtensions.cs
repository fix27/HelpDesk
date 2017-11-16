using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace HelpDesk.Common.Helpers
{
    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
