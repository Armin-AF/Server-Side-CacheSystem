using GitHub.Server.Controllers;
using GitHub.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace GitHub.Server.Test;

public class UnitTest1
{
    [Fact]
        public async Task GetUser_ReturnsUserFromCache_WhenUserIsInCache()
        {
            // Arrange
            var memoryCache = new TestMemoryCache();
            var controller = new GitHubController(memoryCache);
            const string username = "testuser";
            var expected = new GitHubUser{
                AvatarUrl = null, HtmlUrl = null, Id = 19480, Login = "testuser",
                Url = "https://api.github.com/users/testuser"
            };


            // Act
            var result = await controller.GetUser(username);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<GitHubUser>(okResult.Value);
            Assert.Equal(expected, user);
        }

        [Fact]
        public async Task GetUser_ReturnsUserFromApi_WhenUserIsNotInCache()
        {
            // Arrange
            var memoryCache = new TestMemoryCache();
            var controller = new GitHubController(memoryCache);
            const string username = "testuser";
            var expected = new GitHubUser{
                AvatarUrl = null, HtmlUrl = null, Id = 19480, Login = "testuser",
                Url = "https://api.github.com/users/testuser"
            };

            // Act
            var result = await controller.GetUser(username);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<GitHubUser>(okResult.Value);
            Assert.Equal(expected, user);
        }

        class TestMemoryCache : IMemoryCache
        {
            readonly Dictionary<object, object> _cache = new();

            public void Dispose()
            {
                // No-op
            }

            public void Remove(object key){
                throw new NotImplementedException();
            }

            public bool TryGetValue(object key, out object value)
            {
                return _cache.TryGetValue(key, out value);
            }

            public ICacheEntry CreateEntry(object key)
            {
                var entry = new TestCacheEntry(key, this);
                _cache[key] = entry.Value;
                return entry;
            }

            class TestCacheEntry : ICacheEntry
            {
                readonly TestMemoryCache _cache;
                readonly object _key;

                public TestCacheEntry(object key, TestMemoryCache cache)
                {
                    _key = key;
                    _cache = cache;
                }

                public object Key => _key;
                public object Value { get; set; } = null!;
                public DateTimeOffset? AbsoluteExpiration { get; set; }
                public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
                public long? Size{ get; set; }
                public TimeSpan? SlidingExpiration { get; set; }
                public IList<IChangeToken> ExpirationTokens { get; } = new List<IChangeToken>();
                public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = new List<PostEvictionCallbackRegistration>();
                public CacheItemPriority Priority { get; set; }

                public void Dispose()
                {
                    _cache._cache.Remove(_key);
                }
            }
        }


        private class ApiClientMock
        {
            public GitHubUser User { get; set; }

            public Task<HttpResponseMessage> GetAsync(string requestUri)
            {
                return Task.FromResult(new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(User))
                });
            }
        }
    

}