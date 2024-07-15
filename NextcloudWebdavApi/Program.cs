using System.Net;
using NextcloudWebdavApi;
using WebDAVClient;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
	options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var webdavOptions = new WebdavOptions
{
	Username = Environment.GetEnvironmentVariable("WEBDAV__USERNAME") ?? string.Empty,
	Password = Environment.GetEnvironmentVariable("WEBDAV__PASSWORD") ?? string.Empty,
	Server = Environment.GetEnvironmentVariable("WEBDAV__SERVER") ?? string.Empty,
	BasePath = Environment.GetEnvironmentVariable("WEBDAV__BASEPATH") ?? string.Empty,
};

builder.Services.AddSingleton<IClient, Client>(_ =>
{
	var credential = new NetworkCredential(webdavOptions.Username, webdavOptions.Password);
	var client = new Client(credential);
	client.Server = webdavOptions.Server;
	client.BasePath = webdavOptions.BasePath;
	return client;
});

var app = builder.Build();

var fileApi = app.MapGroup("/files");
fileApi.MapPost("/", FileApi.UploadFile);
fileApi.MapGet("/", FileApi.DownloadFile);

app.Logger.LogInformation("Webdav configuration: {WebdavOptions}", webdavOptions with { Password = "********" });

await app.RunAsync();