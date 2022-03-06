using VendingMachine.Api;
using VendingMachine.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Lifetime);

app.MapControllers();
app.MapHub<DisplayHub>("/display");
app.Run();
