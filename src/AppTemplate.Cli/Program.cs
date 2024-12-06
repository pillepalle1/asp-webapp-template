var builder = CoconaApp.CreateBuilder();

builder.AddConfiguration();
builder.Services.AddApplication();

var app = builder.Build();

// ------------------------------------------------------------------------------------------------
// Database initialization and migrations here
using (var scope = app.Services.CreateScope())
{
    var blogDbInitializer = scope.ServiceProvider.GetRequiredService<IDatabasesInitializer>();
    await blogDbInitializer.InitializeAllAsync();
}

app.AddBlogCommands();

app.Run();