using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal static class V1TypeExtensions
    {
        public static void WriteToFile(this Stream stream, string fileName)
        {
            if (File.Exists(fileName))
            {
                fileName = fileName.Replace("FilteredImage.png", "FilteredImage_Copy.png");
            }
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            byte[] bytesStream = new byte[stream.Length];
            if (stream.Read(bytesStream, 0, bytesStream.Length) != -1)
            {
                fs.Write(bytesStream);
            }
            fs.Close();
        }
    }
}