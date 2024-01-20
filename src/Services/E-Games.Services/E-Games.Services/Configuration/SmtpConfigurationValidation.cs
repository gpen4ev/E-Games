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

            if (options.Port < 1 || options.Port > 65535)
            {
                errors.Add($"'{nameof(options.Port)}' must be between 1 and 65535.");
            }

            if (string.IsNullOrEmpty(options.Username))
            {
                errors.Add("Username is a required field.");
            }

            if (string.IsNullOrEmpty(options.Password))
            {
                errors.Add("Password is a required filed.");
            }

            if (string.IsNullOrEmpty(options.SenderName) || !IsValidEmail(options.SenderEmail!))
            {
                errors.Add($"'{nameof(options.SenderEmail)}' must be a valid email address.");
            }

            if (string.IsNullOrEmpty(options.SenderName))
            {
                errors.Add($"'{nameof(options.SenderName)}' is a required field.");
            }

            if (errors.Any())
            {
                return ValidateOptionsResult.Fail(errors);
            }

            return ValidateOptionsResult.Success;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
