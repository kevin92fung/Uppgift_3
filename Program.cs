using ToDoApp.Services; // Add this line at the top

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register the appropriate implementation for IToDoService based on the environment variable value
var todoServiceImplementation = Environment.GetEnvironmentVariable("TODO_SERVICE_IMPLEMENTATION");
switch (todoServiceImplementation)
{
    case "MongoDb":
        builder.Services.AddSingleton<IToDoService, MongoDbToDoService>();
        break;
    case "Json":
    default:
        builder.Services.AddSingleton<IToDoService, JsonToDoService>();
        break;
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
