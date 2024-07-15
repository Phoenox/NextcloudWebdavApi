namespace NextcloudWebdavApi;

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

[JsonSerializable(typeof(GetFileRequest))]
[JsonSerializable(typeof(PostFileRequest))]
[JsonSerializable(typeof(ProblemDetails))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;
