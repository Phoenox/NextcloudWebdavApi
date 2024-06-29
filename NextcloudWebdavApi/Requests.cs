namespace NextcloudWebdavApi;

public record GetFileRequest(string Path);
public record PostFileRequest(string Path, string Content);