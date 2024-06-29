namespace NextcloudWebdavApi;

public record WebdavOptions
{
	public string Username { get; init; } = string.Empty;
	public string Password { get; init; } = string.Empty;
	public string Server { get; init; } = string.Empty;
	public string BasePath { get; init; } = string.Empty;
}