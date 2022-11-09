using System.Data.SqlClient;

namespace StudentManager.DAL.Extensions;

public static class SqlDataReaderExtensions {
  public static byte[] GetVarBinary(this SqlDataReader reader, int idx) {
    int bufferSize = 1024 * 4;
    byte[]? buffer = new byte[bufferSize];

    int currentBytes = 0;

    using var memoryStream = new MemoryStream();
    using var binaryWriter = new BinaryWriter(memoryStream);

    int readBytes;
    do {
      readBytes = (int)reader.GetBytes(idx, currentBytes, buffer, 0, bufferSize);
      binaryWriter.Write(buffer, 0, readBytes);
      binaryWriter.Flush();

      currentBytes += readBytes;
    } while (readBytes == bufferSize);

    return memoryStream.ToArray();
  }
}
