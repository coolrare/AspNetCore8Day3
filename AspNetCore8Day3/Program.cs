using AspNetCore8Day3.Models;
using Serilog;


// 在 Console 專案需安裝 Microsoft.Extensions.Configuration 套件 (NuGet)
//var configuration = new ConfigurationBuilder()
//    .AddJsonFile("serilog.json", optional: false, reloadOnChange: true)
//    .AddEnvironmentVariables()
//    .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()

    .WriteTo.Seq(serverUrl: "http://localhost:5341")

    //.WriteTo.File(
    //    path: "logs/log.txt",
    //    rollingInterval: RollingInterval.Day,
    //    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    //)

    //.WriteTo.MSSqlServer(
    //    connectionString: configuration.GetConnectionString("DefaultConnection"),
    //    sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents" })

    .CreateLogger();

try
{
    Log.Information("Starting web application");

    #region ASP.NET Core 主程式
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddSerilog();
    //builder.Logging.AddSerilog();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Logging.ClearProviders();

    //builder.Logging.AddSimpleConsole(options =>
    //{
    //    options.c = true;
    //    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    //    options.UseUtcTimestamp = false;
    //    options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
    //});

    //builder.Services.Configure<ConsoleFormatterOptions>(builder.Configuration.GetSection("Logging"))

    builder.Logging.AddJsonConsole(options =>
    {
        // 包含範圍資訊
        //options.IncludeScopes = true;

        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
        options.UseUtcTimestamp = false;
        options.JsonWriterOptions = new System.Text.Json.JsonWriterOptions()
        {
            Indented = true
        };
    });

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

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
    #endregion

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
