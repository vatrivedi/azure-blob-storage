using System;
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

            //var uploads = Path.Combine(env.WebRootPath, "uploads");
            //bool exists = Directory.Exists(uploads);
            //if (!exists)
            //    Directory.CreateDirectory(uploads);

            var fileName = Path.GetFileName(@"C:\Vishal\Repos\ConsoleApp1\Readme.txt");
            var fileStream = new FileStream(fileName, FileMode.Create);
            string mimeType = MimeTypes.GetMimeType(fileName);
            byte[] fileData = new byte[fileName.Length];

            objBlobService.UploadFileToBlob(fileName, fileData, mimeType);


            //product.ImagePath = objBlobService.UploadFileToBlob(product.File.FileName, fileData, mimeType);
        }
    }
}
