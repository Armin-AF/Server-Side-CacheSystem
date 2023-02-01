using Microsoft.AspNetCore.Mvc;
using GitHub.Server.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;


namespace GitHub.Server.Controllers;

[ApiController]
[Route("api/github/{username}")]
public class GitHubController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;

    public GitHubController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUser(string username)
    {
        if (_memoryCache.TryGetValue(username, out GitHubUser user))
        {
            user.IsFromCache = true;
            return Ok(user);
        }
        // Call the GitHub API to retrieve the data
        // If the data is not in the cache, retrieve it from the API and add it to the cache
        user = await GetGitHubUserFromApiAsync(username);
        user.IsFromCache = false;
        _memoryCache.Set(username, user, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        return Ok(user);
    }
    
    [HttpGet("cached")]
    public Task<IActionResult> GetUserCached(string username){
        return _memoryCache.TryGetValue(username, out GitHubUser user) ? Task.FromResult<IActionResult>(Ok(user)) : Task.FromResult<IActionResult>(NotFound());
    }
    
    
    async Task<GitHubUser> GetGitHubUserFromApiAsync(string username)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "MyGitHubClient");
        var response = await httpClient.GetAsync($"https://api.github.com/users/{username}");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
    
        using var reader = new JsonTextReader(new StringReader(json));
        var serializer = new JsonSerializer();
        return serializer.Deserialize<GitHubUser>(reader)!;
    }
}