using TestingLib;
using TestingServer.Config;
using TestingServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ITestsService, TestsXMLService>();
builder.Services.AddOptions();
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));
var app = builder.Build();

var testConfig = new AppSettings();
app.Configuration.Bind(testConfig);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
