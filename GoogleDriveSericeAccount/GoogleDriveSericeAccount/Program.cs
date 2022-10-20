using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using System;

namespace GoogleDriveSericeAccount
{
    class Program
    {
        private const string PathToServiceAccountKeyFile = "C:\\Users\\yborisov\\Desktop\\work\\C#\\C#Develepment\\05C#Web\\02ASP.Net\\03Api\\GoogleDriveSericeAccount\\GoogleDriveSericeAccount\\testproject-366105-a9a5c2a64300.json";
        private const string ServiceAccountEmail = "testproject@testproject-366105.iam.gserviceaccount.com";
        private const string UploadFileName = "asenovgradskiBairi.gpx";
        private const string DirectoryId = "1NeqkP2bplJdbeEGC8UIeY2oQkr317YYa";

        static async Task Main(string[] args)
        {
            var credentials = GoogleCredential.FromFile(PathToServiceAccountKeyFile).CreateScoped(DriveService.ScopeConstants.Drive);

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials
            });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "first.gxp",
                Parents = new List<string>() { DirectoryId },
            };

            string uploadFileId;

            await using (var fsSource = new FileStream(UploadFileName, FileMode.Open, FileAccess.Read))
            {
                var request = service.Files.Create(fileMetadata, fsSource, "application/gpx+xml");
                request.Fields = "*";
                var result = await request.UploadAsync(CancellationToken.None);

                if (result.Status == UploadStatus.Failed)
                {
                    Console.WriteLine($"Error uploading file: {result.Exception.Message}");
                }

                uploadFileId = request.ResponseBody?.Id;
            }
        }
    }
}
