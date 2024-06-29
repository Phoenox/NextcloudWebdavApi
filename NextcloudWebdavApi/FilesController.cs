namespace NextcloudWebdavApi;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
	[HttpPost]
	public IActionResult PostFile([FromBody] PostFileRequest request)
	{
		try
		{
			Files.CreateOrUpdateFile(request.Path, request.Content);
			return Ok();
		}
		catch (Exception ex)
		{
			return Problem(detail: ex.Message, statusCode: 500);
		}
	}

	[HttpGet]
	public IActionResult GetFile([FromBody] GetFileRequest request)
	{
		try
		{
			var content = Files.GetFile(request.Path);
			return Ok(content);
		}
		catch (Exception ex)
		{
			return Problem(detail: ex.Message, statusCode: 500);
		}
	}
}