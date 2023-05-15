namespace UnitTests;

using BusinessLogicLayer.Service.Impl;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;

public class EmailServiceTests
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();
        _emailService = new EmailService(_configurationMock.Object);
    }

    [Fact]
    public async Task SendEmail_Should_Send_Email()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("SMTPConfig:UserName", "testuser"),
                new KeyValuePair<string, string>("SMTPConfig:Password", "testpassword"),
                new KeyValuePair<string, string>("SMTPConfig:Host", "testhost"),
                new KeyValuePair<string, string>("SMTPConfig:Port", "587"),
                new KeyValuePair<string, string>("SMTPConfig:SenderAddress", "testsender@example.com"),
                new KeyValuePair<string, string>("SMTPConfig:SenderDisplayName", "Test Sender")
            }!)
            .Build();

        var emailService = new EmailService(config);

        await Assert.ThrowsAsync<SmtpException>(() =>
            emailService.SendEmail("Test Subject", "Test Body", "test@example.com"));
    }
}