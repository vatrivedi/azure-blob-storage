using System;
using System.Web;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            UploadToBlobStorage();
        }

        public static void UploadToBlobStorage()
        {
            var objBlobService = new BlobStorageService();

            var fileName = Path.GetFileName(@"Readme.txt");
            var fileStream = new FileStream(fileName, FileMode.Create);
            string mimeType = MimeMapping.MimeUtility.GetMimeMapping(fileName);
            byte[] fileData = new byte[fileName.Length];

            objBlobService.UploadFileToBlob(fileName, fileData, mimeType);

        }
    }
}
