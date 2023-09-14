var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// enable Routing
app.UseRouting();

// binding Endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.Map("files/{fileName}.{extension}", async (context) =>
    {
        string? fileName = Convert.ToString(context.Request.RouteValues["fileName"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
        await context.Response.WriteAsync($"In files - {fileName}.{extension}");
    });
});

app.Run(async (context) =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();


