using Microsoft.Extensions.Options;

namespace E_Games.Services.E_Games.Services.Configuration
{
    public class SmtpConfigurationValidation : IValidateOptions<SmtpSettings>
    {
        public ValidateOptionsResult Validate(string name, SmtpSettings options)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(options.Server))
            {
                errors.Add($"'{nameof(options.Server)}' must be provided.");
            }

            if (string.IsNullOrEmpty(options.Username))
            {
                return ValidateOptionsResult.Fail("Username is a required field");
            }

            // TODO: add other validations as necessary

            if (errors.Any())
            {
                return ValidateOptionsResult.Fail(errors);
            }

            return ValidateOptionsResult.Success;
        }
    }
}
