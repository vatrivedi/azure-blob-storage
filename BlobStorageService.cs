﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ConsoleApp1
{
    public class BlobStorageService
    {
        string accessKey = string.Empty;
        public BlobStorageService()
        {
            this.accessKey = "ENTER DETAILS FROM BLOB HERE";
        }

        public string UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {

                var _task = Task.Run(() => this.UploadFileToBlobAsync(strFileName, fileData, fileMimeType));
                _task.Wait();
                string fileUrl = _task.Result;
                return fileUrl;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async void DeleteBlobData(string fileUrl)
        {
            Uri uriObj = new Uri(fileUrl);
            string BlobName = Path.GetFileName(uriObj.LocalPath);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string strContainerName = "uploads";
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

            string pathPrefix = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/";
            CloudBlobDirectory blobDirectory = cloudBlobContainer.GetDirectoryReference(pathPrefix);
            // get block blob refarence  
            CloudBlockBlob blockBlob = blobDirectory.GetBlockBlobReference(BlobName);

            // delete blob from container      
            await blockBlob.DeleteAsync();
        }


        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }

        private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            // access key will be available from Azure blob - "DefaultEndpointsProtocol=https;AccountName=XXX;AccountKey=;EndpointSuffix=core.windows.net"
            CloudStorageAccount csa = CloudStorageAccount.Parse(accessKey);
            CloudBlobClient cloudBlobClient = csa.CreateCloudBlobClient();
            string containerName = "my-blob-container"; //Name of your Blob Container
            CloudBlobContainer cbContainer = cloudBlobClient.GetContainerReference(containerName);
            string fileName = this.GenerateFileName(strFileName);

            if (await cbContainer.CreateIfNotExistsAsync())
            {
                await cbContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            if (fileName != null && fileData != null)
            {
                CloudBlockBlob cbb = cbContainer.GetBlockBlobReference(fileName);
                cbb.Properties.ContentType = fileMimeType;
                await cbb.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                return cbb.Uri.AbsoluteUri;
            }
            return "";
        }
    }
}
