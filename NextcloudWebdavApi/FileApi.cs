namespace NextcloudWebdavApi;

using System.Text;
using WebDAVClient;

public abstract partial class FileApi
{
	public static async Task<IResult> UploadFile(PostFileRequest request, IClient webdavClient, ILogger<FileApi> logger)
	{
		try
		{
			var directory = Path.GetDirectoryName(request.Path);
			var fileName = Path.GetFileName(request.Path);
			var byteArray = Encoding.UTF8.GetBytes(request.Content);
			var stream = new MemoryStream(byteArray);
			await webdavClient.Upload(directory, stream, fileName);
			FileUploaded(logger, fileName);
			return Results.Ok();
		}
		catch (Exception ex)
		{
			ErrorUploadingFile(logger, ex, request.Path);
			return Results.Problem(detail: ex.Message, statusCode: 500);
		}
	}
	
	[LoggerMessage(Level = LogLevel.Information, Message = "File {File} uploaded")]
	private static partial void FileUploaded(ILogger logger, string file);
	
	[LoggerMessage(Level = LogLevel.Error, Message = "Error uploading file {File}")]
	private static partial void ErrorUploadingFile(ILogger logger, Exception exception, string file);
	
	public static async Task<IResult> DownloadFile(GetFileRequest request, IClient webdavClient, ILogger<FileApi> logger)
	{
		try
		{
			var stream = await webdavClient.Download(request.Path);
			using var reader = new StreamReader(stream);
			var content = await reader.ReadToEndAsync();
			FileDownloaded(logger, request.Path);
			return Results.Ok(content);
		}
		catch (Exception ex)
		{
			ErrorDownloadingFile(logger, ex, request.Path);
			return Results.Problem(detail: ex.Message, statusCode: 500);
		}
	}
	
	[LoggerMessage(Level = LogLevel.Information, Message = "File {File} downloaded")]
	private static partial void FileDownloaded(ILogger logger, string file);
	
	[LoggerMessage(Level = LogLevel.Error, Message = "Error downloading file {File}")]
	private static partial void ErrorDownloadingFile(ILogger logger, Exception exception, string file);
}
