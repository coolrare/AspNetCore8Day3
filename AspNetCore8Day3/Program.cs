using AspNetCore8Day3.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Configuration.Sources.Clear();
//builder.Configuration.AddJsonFile(
//    path: builder.Configuration.GetValue<string>("ExternalConfig")!,
//    optional: true,
//    reloadOnChange: true);

//builder.Services.Configure<AppSettingsOptions>(builder.Configuration.GetSection(AppSettingsOptions.AppSettings));

builder.Services.AddOptions<AppSettingsOptions>()
    .Bind(builder.Configuration.GetSection(AppSettingsOptions.AppSettings))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
