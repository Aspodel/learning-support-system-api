using Azure.Storage.Blobs;

namespace LearningSupportSystemAPI;

public class BlobService : IBlobService
{
    private readonly string _blobConnectionString;
    private readonly string _containerName;
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(IConfiguration configuration)
    {
        _blobConnectionString = configuration["EduConfig:BlobConnectionString"]!;
        _containerName = "submission";
        _blobServiceClient = new BlobServiceClient(_blobConnectionString);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        // Extract the file extension from the original file name
        var fileExtension = Path.GetExtension(file.FileName);

        fileName += fileExtension;

        // Get the blob client for the file
        var blobClient = containerClient.GetBlobClient(fileName);

        // Upload the file to Azure Blob Storage
        await blobClient.UploadAsync(file.OpenReadStream(), true);

        return blobClient.Uri.ToString();
    }
}