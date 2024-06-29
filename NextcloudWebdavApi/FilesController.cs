namespace NextcloudWebdavApi;

using System.Text;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class FilesController(WebDAVClient.IClient webdavClient) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> PostFile([FromBody] PostFileRequest request)
	{
		try
		{
			var directory = Path.GetDirectoryName(request.Path);
			var fileName = Path.GetFileName(request.Path);
			var byteArray = Encoding.UTF8.GetBytes(request.Content);
			var stream = new MemoryStream(byteArray);
			await webdavClient.Upload(directory, stream, fileName);
			return Ok();
		}
		catch (Exception ex)
		{
			return Problem(detail: ex.Message, statusCode: 500);
		}
	}

	[HttpGet]
	public async Task<IActionResult> GetFile([FromBody] GetFileRequest request)
	{
		try
		{
			var stream = await webdavClient.Download(request.Path);
			using var reader = new StreamReader(stream);
			var content = await reader.ReadToEndAsync();
			return Ok(content);
		}
		catch (Exception ex)
		{
			return Problem(detail: ex.Message, statusCode: 500);
		}
	}
}