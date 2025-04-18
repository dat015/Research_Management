using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _smtpHost = _configuration["EmailSettings:SmtpHost"];
        _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
        _smtpUsername = _configuration["EmailSettings:SmtpUsername"];
        _smtpPassword = _configuration["EmailSettings:SmtpPassword"];
        _fromEmail = _configuration["EmailSettings:FromEmail"];
        _fromName = _configuration["EmailSettings:FromName"];
        _logger = logger;
    }

    public async Task SendEvaluationEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        try
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_configuration["EmailSettings:FromEmail"],
                    _configuration["EmailSettings:FromName"], System.Text.Encoding.UTF8);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.SubjectEncoding = System.Text.Encoding.UTF8;

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = _configuration["EmailSettings:SmtpHost"];
                    smtpClient.Port = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Credentials = new NetworkCredential(
                        _configuration["EmailSettings:SmtpUsername"],
                        _configuration["EmailSettings:SmtpPassword"]
                    );

                    await smtpClient.SendMailAsync(message);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email: {ex.Message}");
            throw;
        }
    }
}

public interface IEmailService
{
    Task SendEvaluationEmailAsync(string to, string subject, string body, bool isHtml = false);
}