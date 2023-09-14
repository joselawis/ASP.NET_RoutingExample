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

    endpoints.Map("employee/profile/{employeeName=Jose}", async (context) =>
    {
        int? employeeName = Convert.ToInt32(context.Request.RouteValues["employeeName"]);
        await context.Response.WriteAsync($"In employee profile - {employeeName}");
    });

    endpoints.Map("products/details/{id?}", async (context) =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            string? productId = Convert.ToString(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"In products details - {productId}");
        }
        else
        {
            await context.Response.WriteAsync($"In products details - id is not supplied");
        }
    });
});

app.Run(async (context) =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();


