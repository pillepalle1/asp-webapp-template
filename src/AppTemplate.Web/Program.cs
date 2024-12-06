// ------------------------------------------------------------------------------------------------
// Configure services
var builder = WebApplication.CreateBuilder(args);

// Fetch run mode from configuration
var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
if (appSettings is null)
{
    Console.WriteLine("Missing section 'AppSettings' in appsettings.json");
    return;
}

// Configure services
builder.Services.AddSystemd();
builder.Services.AddApplication();
builder.ConfigureWeb();

builder.Services.AddHostedService<BlogWatcher>();

if (appSettings.RunApi)
    builder.Services.ConfigureApiServices();

if (appSettings.RunApp)
    builder.Services.ConfigureAppServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDatabasesInitializer>();
    await dbInitializer.InitializeAllAsync();
}

// Configure middleware and endpoints
if (appSettings.RunApi)
    app.ConfigureApiMiddleware();

if (appSettings.RunApp)
    app.ConfigureAppMiddleware();

app.Run();




