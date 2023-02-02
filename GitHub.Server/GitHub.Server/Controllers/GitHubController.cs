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
            var cached = new GitHubUser
            {
                login = user.login,
                Id = user.Id,
                avatar_url = user.avatar_url,
                Url = user.Url,
                html_url = user.html_url,
                public_repos = user.public_repos,
                bio = user.bio,
                name = user.name,
                location = user.location,
                IsFromCache = true
            };
            return Ok(cached);
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
    public IActionResult GetUserCached(string username)
    {
        if (_memoryCache.TryGetValue(username, out GitHubUser user))
        {
            user.IsFromCache = true;
            return Ok(user);
        }
        return NotFound();
    }
    
    [HttpGet]
    [Route("/api/github/all")]
    public IActionResult GetAllCachedUsers()
    {
        var cacheKeys = _memoryCache.GetType()
            .GetField("_entries", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
            .GetValue(_memoryCache) as System.Collections.Concurrent.ConcurrentDictionary<object, object>;

        if (cacheKeys == null)
        {
            return NotFound();
        }

        var cachedUsers = new List<GitHubUser>();
        foreach (var cacheKey in cacheKeys.Keys)
        {
            if (_memoryCache.TryGetValue(cacheKey, out GitHubUser user))
            {
                cachedUsers.Add(user);
            }
        }

        return Ok(cachedUsers);
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