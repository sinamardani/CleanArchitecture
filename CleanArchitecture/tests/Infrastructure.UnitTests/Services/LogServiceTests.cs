using Application.Commons.Interfaces.Data;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Infrastructure.UnitTests.Services;

public class LogServiceTests
{
    private LogService CreateLogService()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:LoggingDb", null }
            })
            .Build();

        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var currentUserService = new Mock<ICurrentUserService>();
        var hostEnvironment = new Mock<IHostEnvironment>();

        var mockHttpContext = new Mock<HttpContext>();
        httpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
        currentUserService.Setup(x => x.UserId).Returns((int?)null);
        hostEnvironment.Setup(x => x.EnvironmentName).Returns("Test");

        return new LogService(
            configuration,
            httpContextAccessor.Object,
            currentUserService.Object,
            hostEnvironment.Object);
    }

    [Fact]
    public void DbLog_WithValidMessage_ShouldNotThrow()
    {
        var logService = CreateLogService();

        var act = () => logService.DbLog("Test message");

        act.Should().NotThrow();
    }

    [Fact]
    public void DbLog_WithEmptyMessage_ShouldNotThrow()
    {
        var logService = CreateLogService();

        var act = () => logService.DbLog(string.Empty);

        act.Should().NotThrow();
    }

    [Fact]
    public void ConsoleLog_WithValidMessage_ShouldNotThrow()
    {
        var logService = CreateLogService();

        var act = () => logService.ConsoleLog("Test message");

        act.Should().NotThrow();
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        var logService = CreateLogService();

        var act = () => logService.Dispose();

        act.Should().NotThrow();
    }
}
