#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Syncfusion.JavaScript;

namespace SyncfusionASPNETApplication9
{
    /// <summary>
    /// Summary description for uploadFile
    /// </summary>
    public class uploadFiles : IHttpHandler
    {

        public async void ProcessRequest(HttpContext context)
        {
            CloudBlobContainer container;
            string accountKey = "<-----account key------->";
            string accountName = "<-----account name------->";
            string blobName = "<-----container name------->";
            StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudBlobClient client = account.CreateCloudBlobClient();
            container = client.GetContainerReference(blobName);
            HttpRequest request = context.Request;
            HttpFileCollection uploadedFiles = request.Files;
            string path = request.QueryString["Path"];
            try
            {
                foreach (var uploadedFile in uploadedFiles)
                {
                    for (int i = 0; i < uploadedFiles.Count; i++)
                    {
                        string fileName = uploadedFiles[i].FileName;
                        string MyPath = path.Replace("https://<-----account name------->.blob.core.windows.net/<-----container name------->/", "");
                        CloudBlockBlob blob = container.GetBlockBlobReference(MyPath + fileName);
                        await container.SetPermissionsAsync(new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        });
                        blob.Properties.ContentType = "application/octet-stream";
                        using (var fileStream = System.IO.File.OpenRead(@"D:\" + fileName)) // @"D:\" is the local path from where you wish to upload the files
                        {
                          await blob.UploadFromStreamAsync(fileStream);
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
