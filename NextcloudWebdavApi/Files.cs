namespace NextcloudWebdavApi;

public static class Files
{
	public static void CreateOrUpdateFile(string path, string content)
	{
		File.WriteAllText(path, content);
	}
	
	public static string GetFile(string path)
	{
		return File.ReadAllText(path);
	}
}
