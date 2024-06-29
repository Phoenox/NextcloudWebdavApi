using System.Net;
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

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public record WebdavOptions
{
	public string Username { get; init; } = string.Empty;
	public string Password { get; init; } = string.Empty;
	public string Server { get; init; } = string.Empty;
	public string BasePath { get; init; } = string.Empty;
}
