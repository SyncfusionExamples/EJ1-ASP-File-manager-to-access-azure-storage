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
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Syncfusion.JavaScript;

namespace SyncfusionASPNETApplication9.FileExplorer
{
    /// <summary>
    /// Summary description for downloadFile
    /// </summary>
    public class downloadFile : System.Web.UI.Page
    {

        [System.Web.Services.WebMethod]

        public static void Download(string path, string[] names)
        {
            CloudBlobContainer container;
            string accountKey = "<-----account key------->";
            string accountName = "<-----account name------->";
            string blobName = "<-----container name------->";
            StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudBlobClient client = account.CreateCloudBlobClient();
            container = client.GetContainerReference(blobName);

            HttpResponse response = HttpContext.Current.Response;
                    foreach (var blobFileName in names)
                    {
                        string MyPath = path.Replace("https://<-----account name------->.blob.core.windows.net/<-----container name------->/", "");
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(MyPath + blobFileName);
                        using (var fileStream = System.IO.File.Create(@"D:\" + blobFileName)) // @"D:\" is the local path to where you wish to download the files
                {
                            blockBlob.DownloadToStream(fileStream);
                        }
                    }
                response.ContentType = "application/octet-stream";
                response.Flush();
                response.End();
        }
    }
}
