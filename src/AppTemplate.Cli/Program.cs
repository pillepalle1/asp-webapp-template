var builder = CoconaApp.CreateBuilder();

builder.AddConfiguration();
builder.Services.AddApplication();

var app = builder.Build();

// ------------------------------------------------------------------------------------------------
// Database initialization and migrations here
using (var scope = app.Services.CreateScope())
{
    var statsDbInitializer = scope.ServiceProvider.GetRequiredService<IDatabasesInitializer>();
    await statsDbInitializer.InitializeAllAsync();
}

app.AddBlogCommands();

app.Run();