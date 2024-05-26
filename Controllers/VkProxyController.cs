using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Proxy_VK_Api.Controllers.VkProxyController;

namespace Proxy_VK_Api.Controllers
{
    [Route("vk")]
    [ApiController]
    public class VkProxyController : ControllerBase
    {
        private static readonly HttpClient _httpClient;

        static VkProxyController()
        {
            _httpClient = new()
            {
                BaseAddress = new Uri("https://api.vk.com/method/"),
            };
        }
        public VkProxyController()
        {
            
        }

        [HttpPost("profileInfo")]
        public async Task<IActionResult> GetProfileInfo([FromQuery]string token, [FromQuery] string version)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("v", version)
            });

            var response = await _httpClient.PostAsync("account.getProfileInfo", formContent);
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("friends")]
        public async Task<IActionResult> GetFriends(
            [FromQuery] string token,
            [FromQuery] string version,
            [FromQuery] int count,
            [FromQuery] int offset)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("v", version),
                new KeyValuePair<string, string> ("count", count.ToString()),
                new KeyValuePair<string, string>("offset", offset.ToString()),
                new KeyValuePair<string, string>("fields", "status,bdate,city,country,domain,education,exports,has_photo,has_mobile,home_town,photo_50,photo_100")

            });

            var response = await _httpClient.PostAsync("friends.get", formContent);
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("friendsSearch")]
        public async Task<IActionResult> GetSearchFriends(
            [FromQuery] string query,
            [FromQuery] string token,
            [FromQuery] string version,
            [FromQuery] int count,
            [FromQuery] int offset)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("q", query),
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("v", version),
                new KeyValuePair<string, string> ("count", count.ToString()),
                new KeyValuePair<string, string>("offset", offset.ToString()),
                new KeyValuePair<string, string>("fields", "status,bdate,city,country,domain,education,exports,has_photo,has_mobile,home_town,photo_50,photo_100")

            });

            var response = await _httpClient.PostAsync("friends.search", formContent);
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("friendInfo")]
        public async Task<IActionResult> GetFriendInfo(
            [FromQuery] string token,
            [FromQuery] string version,
            [FromQuery] int id)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("v", version),
                new KeyValuePair<string, string>("user_ids", id.ToString()),
                new KeyValuePair<string, string>("fields", "city,site, tv, quotes, followers_count, screen_name, schools, sex, photo_400_orig, education, activities, about, books, connections,")

            });

            var response = await _httpClient.PostAsync("users.get", formContent);
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("videos")]
        public async Task<IActionResult> GeVideos(
            [FromQuery] string query,
            [FromQuery] string token,
            [FromQuery] string version,
            [FromQuery] int count,
            [FromQuery] int offset)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("q", query),
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("v", version),
                new KeyValuePair<string, string> ("count", count.ToString()),
                new KeyValuePair<string, string>("offset", offset.ToString()),
            });

            var response = await _httpClient.PostAsync("video.search", formContent);
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("myVideos")]
        public async Task<IActionResult> GetMyVideos(
            [FromQuery] string token,
            [FromQuery] string version,
            [FromQuery] int count,
            [FromQuery] int offset)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("v", version),
                new KeyValuePair<string, string> ("count", count.ToString()),
                new KeyValuePair<string, string>("offset", offset.ToString()),
            });

            var response = await _httpClient.PostAsync("video.get", formContent);
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("myGroups")]
        public async Task<IActionResult> GetGroups(
            [FromQuery] string token,
            [FromQuery] string version,
            [FromQuery] int count,
            [FromQuery] int offset)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("v", version),
                new KeyValuePair<string, string> ("count", count.ToString()),
                new KeyValuePair<string, string>("offset", offset.ToString()),
            });

            var response = await _httpClient.PostAsync("groups.get", formContent);
            var responseText = await response.Content.ReadAsStringAsync();
            var typedResponse = JsonConvert.DeserializeObject<ResponseVk<GetGroupsResponse>>(responseText);

            var groupIdsString = typedResponse?.response?.items == null
                ? ""
                : string.Join(',', typedResponse.response.items);

            var formContentGroups = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("v", version),
                new KeyValuePair<string, string> ("group_ids", groupIdsString),
            });

            var responseFinal = await _httpClient.PostAsync("groups.getById", formContentGroups);

            return Ok(await responseFinal.Content.ReadAsStringAsync());
        }

        [HttpPost("myGroupsCount")]
        public async Task<IActionResult> GetGroupsTotalCount(
            [FromQuery] string token,
            [FromQuery] string version,
            [FromQuery] int count,
            [FromQuery] int offset)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("v", version),
                new KeyValuePair<string, string> ("count", count.ToString()),
                new KeyValuePair<string, string>("offset", offset.ToString()),
            });

            var response = await _httpClient.PostAsync("groups.get", formContent);
            var responseText = await response.Content.ReadAsStringAsync();
            var typedResponse = JsonConvert.DeserializeObject<ResponseVk<GetGroupsResponse>>(responseText);

            return Ok(typedResponse.response.count);
        }

        [HttpPost("myPhotos")]
        public async Task<IActionResult> GetPhotos(
            [FromQuery] string token,
            [FromQuery] string version,
            [FromQuery] int count = 0,
            [FromQuery] int offset = 0)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("access_token", token),
                new KeyValuePair<string, string>("v", version),
                //new KeyValuePair<string, string> ("count", count.ToString()),
                //new KeyValuePair<string, string>("offset", offset.ToString()),
            });

            var response = await _httpClient.PostAsync("photos.getAll", formContent);

            return Ok(await response.Content.ReadAsStringAsync());
        }

        public class ResponseVk<T>
        {
           public T response { get; set; }
        }

        public class GetGroupsResponse
        {
            public int count { get; set; }

            public int[] items { get; set; }

            public long last_updated_time { get; set; }
        }

        public class GetPhotosResponse
        {
            public long date { get; set;}

            public int owner_id { get; set; }

            public PhotoSize[] sizes { get; set; }
        }

        public class PhotoSize
        {
            public string type { get; set; }

            public string url { get; set; }
        }
    }
}
