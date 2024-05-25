namespace AspNetCore8Day3.Models;

public class AppSettingsOptions
{
    public const string AppSettings = "AppSettings";

    public required string SomeKey { get; set; }

    public required SMTPOptions Smtp { get; set; }
}

public class SMTPOptions
{
    public required string SmtpIp { get; set; }
    public int SmtpPort { get; set; } = 589;

}