using System.Net;
using NextcloudWebdavApi;
using WebDAVClient;

var builder = WebApplication.CreateBuilder(args);

var webdavOptions = builder.Configuration.GetSection("Webdav").Get<WebdavOptions>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<IClient, Client>(_ =>
{
	var credential = new NetworkCredential(webdavOptions?.Username, webdavOptions?.Password);
	var client = new Client(credential);
	client.Server = webdavOptions?.Server;
	client.BasePath = webdavOptions?.BasePath;
	return client;
});

var app = builder.Build();

if (webdavOptions is not null)
	app.Logger.LogInformation("Webdav configuration: {webdavOptions}", webdavOptions with { Password = "********" });
else
	app.Logger.LogCritical("Webdav configuration not found");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();