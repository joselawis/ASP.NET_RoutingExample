using Microsoft.Extensions.FileProviders;
using RoutingExample.CustomConstraints;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { WebRootPath = "myRoot" });
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});
var app = builder.Build();

app.UseStaticFiles(); // works with the web root path (myRoot)
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "myWebRoot"))
}); // works with "myWebRoot"

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

    endpoints.Map("employee/profile/{employeeName:alpha:length(3,7)=Jose}", async (context) =>
    {
        int employeeName = Convert.ToInt32(context.Request.RouteValues["employeeName"]);
        await context.Response.WriteAsync($"In employee profile - {employeeName}");
    });

    endpoints.Map("products/details/{id:int:range(1,1000)?}", async (context) =>
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

    endpoints.Map("daily-digest-report/{reportDate:datetime}", async (context) =>
    {
        DateTime? date = Convert.ToDateTime(context.Request.RouteValues["reportDate"]);
        await context.Response.WriteAsync($"In daily-digest-report - {date}");
    });

    endpoints.Map("cities/{cityId:guid}", async (context) =>
    {
        Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityId"])!);
        await context.Response.WriteAsync($"In city information - {cityId}");
    });

    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async (context) =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        if (month == "apr" || month == "jul" || month == "oct" || month == "jan")
        {
            await context.Response.WriteAsync($"In sales report - {year} - {month}");
        }
        else
        {
            await context.Response.WriteAsync($"{month} is not allowed for sales report");
        }
    });

    endpoints.Map("sales-report/2024/jan", async (context) =>
    {
        await context.Response.WriteAsync("Sales report exclusively for 2024 Jan");
    });

});

app.Run(async (context) =>
{
    await context.Response.WriteAsync($"No route matched at {context.Request.Path}");
});

app.Run();


