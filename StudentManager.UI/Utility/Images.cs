using System.Data.SqlClient;
using System.IO;
using System.Windows.Media.Imaging;

namespace StudentManager.UI.Utility;
public static class Images {
  public static BitmapImage ByteArrayToBitmapImage(byte[] picture) {
    using var memoryStream = new MemoryStream(picture);
    var bitmap = new BitmapImage();
    bitmap.BeginInit();
    bitmap.StreamSource = memoryStream;
    bitmap.CacheOption = BitmapCacheOption.OnLoad;
    bitmap.EndInit();
    bitmap.Freeze();
    return bitmap;
  }
  public static byte[] BitmapImageToByteArray(BitmapImage image) {
    var jpegEncoder = new JpegBitmapEncoder();
    jpegEncoder.Frames.Add(BitmapFrame.Create(image));
    using var memoryStream = new MemoryStream();
    jpegEncoder.Save(memoryStream);
    return memoryStream.ToArray();
  }
}