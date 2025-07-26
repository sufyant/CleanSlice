using CleanSlice.Api.Extensions;
using CleanSlice.Application;
using CleanSlice.Infrastructure;
using CleanSlice.Persistence;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddPresentation();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(pattern: "api/document.json");
    app.MapScalarApiReference(options =>
    {
        options.OpenApiRoutePattern = "api/document.json";
        options.ShowSidebar = true;
        options.Title = "CleanSlice API";
        options.Theme = ScalarTheme.BluePlanet;
        options.Favicon = "/favicon.svg";
        options.Layout = ScalarLayout.Modern;
        options.DarkMode = true;
        options.CustomCss = "* { font-family: 'Monaco'; }";
        options.DefaultHttpClient = new(ScalarTarget.JavaScript, ScalarClient.Axios);
        options.Servers = new List<ScalarServer>()
        {
            new ScalarServer("http://localhost:7100"),
        };
        options.Metadata = new Dictionary<string, string>()
        {
            { "ogDescription", "Open Graph description" },
            { "ogTitle", "Open Graph title" },
            { "twitterCard", "summary_large_image" },
            { "twitterSite", "https://example.com/api" },
            { "twitterTitle", "My Api documentation" },
            { "twitterDescription", "This is the description for the twitter card" },
            { "twitterImage", "https://dotnet.microsoft.com/blob-assets/images/illustrations/swimlane-build-scalable-web-apps.svg" }
        };
    });
}

app.Run();
