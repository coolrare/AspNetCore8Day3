using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Day3.Models;

public class AppSettingsOptions : IValidatableObject
{
    public const string AppSettings = "AppSettings";

    public string? SomeKey { get; set; }

    [Required]
    [RegularExpression(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$")]
    public required string SmtpIp { get; set; }

    public int SmtpPort { get; set; } = 589;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SmtpPort == 25 && SmtpIp.Length > 10)
        {
            yield return new ValidationResult("錯誤");
        }

        yield return ValidationResult.Success!;
    }
}
