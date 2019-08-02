using NA.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NA.Common.Extentions
{
    public static class HttpClientExtensions
    {
        public static async Task<JObject> GetAsJsonAsync<T>(this HttpClient httpClient, string url, object data = null)
        {
            var content = new StringContent(data.JsonToString());            
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await httpClient.GetAsync(url);
            if (result.IsSuccessStatusCode)
            {
                var dataAsString = await result.Content.ReadAsStringAsync();
                return JObject.FromObject(new ResultModel<object>
                {
                    data = dataAsString.JsonToObject<T>(),
                    success = true,
                    paging = null,
                    error = new List<ErrorModel>()
                });
            }
            else
            {
                return JObject.FromObject(new ResultModel<object>
                {
                    data = null,
                    success = false,
                    paging = null,
                    error = new List<ErrorModel>
                    {
                        new ErrorModel { key="", value = result.StatusCode.ToString() }
                    }
                });
            }
        }

        public static async Task<JObject> PostAsJsonAsync<T>(this HttpClient httpClient, string url, object data)
        {
            var content = new StringContent(data.JsonToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await httpClient.PostAsync(url, content);
            if (result.IsSuccessStatusCode)
            {
                var dataAsString = await result.Content.ReadAsStringAsync();
                return JObject.FromObject(new ResultModel<object>
                {
                    data = dataAsString.JsonToObject<T>(),
                    success = true,
                    paging = null,
                    error = new List<ErrorModel>()
                });
            }
            else
            {
                return JObject.FromObject(new ResultModel<object>
                {
                    data = null,
                    success = false,
                    paging = null,
                    error = new List<ErrorModel>
                    {
                        new ErrorModel { key="", value = result.StatusCode.ToString() }
                    }
                });
            }
        }

        public static async Task<JObject> PutAsJsonAsync<T>(this HttpClient httpClient, string url, object data)
        {
            var content = new StringContent(data.JsonToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await httpClient.PutAsync(url, content);
            if (result.IsSuccessStatusCode)
            {
                var dataAsString = await result.Content.ReadAsStringAsync();
                return JObject.FromObject(new ResultModel<object>
                {
                    data = dataAsString.JsonToObject<T>(),
                    success = true,
                    paging = null,
                    error = new List<ErrorModel>()
                });
            }
            else
            {
                return JObject.FromObject(new ResultModel<object>
                {
                    data = null,
                    success = false,
                    paging = null,
                    error = new List<ErrorModel>
                    {
                        new ErrorModel { key="", value = result.StatusCode.ToString() }
                    }
                });
            }
        }

        public static async Task<JObject> PatchAsJsonAsync<T>(this HttpClient httpClient, string url, object data)
        {
            var content = new StringContent(data.JsonToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await httpClient.PatchAsync(url, content);
            if (result.IsSuccessStatusCode)
            {
                var dataAsString = await result.Content.ReadAsStringAsync();
                return JObject.FromObject(new ResultModel<object>
                {
                    data = dataAsString.JsonToObject<T>(),
                    success = true,
                    paging = null,
                    error = new List<ErrorModel>()
                });
            }
            else
            {
                return JObject.FromObject(new ResultModel<object>
                {
                    data = null,
                    success = false,
                    paging = null,
                    error = new List<ErrorModel>
                    {
                        new ErrorModel { key="", value = result.StatusCode.ToString() }
                    }
                });
            }
        }

        public static async Task<JObject> DeleteAsJsonAsync<T>(this HttpClient httpClient, string url)
        {
            var result = await httpClient.DeleteAsync(url);
            if (result.IsSuccessStatusCode)
            {
                var dataAsString = await result.Content.ReadAsStringAsync();
                return JObject.FromObject(new ResultModel<object>
                {
                    data = dataAsString.JsonToObject<T>(),
                    success = true,
                    paging = null,
                    error = new List<ErrorModel>()
                });
            }
            else
            {
                return JObject.FromObject(new ResultModel<object>
                {
                    data = null,
                    success = false,
                    paging = null,
                    error = new List<ErrorModel>
                    {
                        new ErrorModel { key="", value = result.StatusCode.ToString() }
                    }
                });
            }
        }
    }
}
