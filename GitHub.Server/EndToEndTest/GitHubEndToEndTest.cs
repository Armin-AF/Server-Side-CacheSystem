
using Microsoft.Extensions.Logging;

namespace EndToEndTest;

[TestClass]
public class GitHubEndToEndTest
{
    readonly GitHubController _controller;
    readonly IMemoryCache _memoryCache;
    readonly ILogger<GitHubController> _logger;


    public GitHubEndToEndTest()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _controller = new GitHubController(_memoryCache, _logger);
    }

    [TestMethod]
    public async Task GetUser_ReturnsCachedUser_WhenUsernameExistsInCache()
    {
        // Arrange
        var username = "testuser";
        var expectedUser = new GitHubUser
        {
            login = username,
            Id = 12345,
            avatar_url = "https://avatars.com/user.png",
            html_url = "https://github.com/user",
            Url = "https://api.github.com/users/user"
        };
        _memoryCache.Set(username, expectedUser, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        // Act
        var result = await _controller.GetUser(username);

        // Assert
        var actualUser = (GitHubUser)((OkObjectResult)result).Value!;
        Assert.AreEqual(expectedUser, actualUser);
    }

    [TestMethod]
    public async Task GetUser_ReturnsUserFromApi_WhenUsernameDoesNotExistInCache()
    {
        // Arrange
        var controller = new GitHubController(new MemoryCache(new MemoryCacheOptions()), _logger);
        var username = "testuser";

        // Act
        var result = await controller.GetUser(username);
        var user = (GitHubUser)((ObjectResult)result).Value!;

        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual(username, user.login);
    }

    [TestMethod]
    public async Task GetUser_AddsUserToCache_WhenUsernameDoesNotExistInCache()
    {
        // Arrange
        var username = "testuser";

        // Act
        await _controller.GetUser(username);

        // Assert
        var user = _memoryCache.Get<GitHubUser>(username);
        Assert.IsNotNull(user);
    }
}