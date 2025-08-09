using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookRadar.Bussiness.Service.Http
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions ReadJsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly JsonSerializerOptions WriteJsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void AddAuthorizationHeader(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<T> GetAsync<T>(string url, string token = null)
        {
            try
            {
                AddAuthorizationHeader(token);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, ReadJsonOptions);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error: {e.Message}");
            }
        }

        public async Task<byte[]> GetFileAsync(string url, string token = null)
        {
            try
            {
                AddAuthorizationHeader(token);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error al descargar el archivo desde la URL {url}: {e.Message}");
            }
        }

        public async Task<(byte[] contenido, string nombreArchivo)> PostForFileAsync<TRequest>(string url, TRequest data, string token = null)
        {
            try
            {
                AddAuthorizationHeader(token);

                var json = JsonSerializer.Serialize(data, WriteJsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Error al descargar el archivo: {response.StatusCode}\n{errorText}");
                }

                var bytes = await response.Content.ReadAsByteArrayAsync();
                var nombre = response.Content.Headers.ContentDisposition?.FileName?.Trim('\"') ?? "macro.xls";

                return (bytes, nombre);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error en PostForFileAsync: {ex.Message}", ex);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data, bool isForm, string token = null)
        {
            try
            {
                AddAuthorizationHeader(token);
                HttpContent content = isForm ? BuildMultipartContent(data) : BuildJsonContent(data);

                var response = await _httpClient.PostAsync(url, content);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"Error HTTP {(int)response.StatusCode}: {response.ReasonPhrase}\n{responseJson}");
                }

                return JsonSerializer.Deserialize<TResponse>(responseJson, ReadJsonOptions);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"ERROR: {ex.Message}");
            }
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest data, bool isForm, string token = null)
        {
            try
            {
                AddAuthorizationHeader(token);
                HttpContent content = isForm ? BuildMultipartContent(data, skipEmptyFiles: true) : BuildJsonContent(data);

                var response = await _httpClient.PutAsync(url, content);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"Error HTTP {(int)response.StatusCode}: {response.ReasonPhrase}\n{responseJson}");
                }

                return JsonSerializer.Deserialize<TResponse>(responseJson, ReadJsonOptions);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"ERROR: {ex.Message}");
            }
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string url, string token = null)
        {
            try
            {
                AddAuthorizationHeader(token);

                var response = await _httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(responseJson, ReadJsonOptions);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"ERROR: {e.Message}");
            }
        }

        // Helpers privados

        private StringContent BuildJsonContent<TRequest>(TRequest data)
        {
            var json = JsonSerializer.Serialize(data, WriteJsonOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private MultipartFormDataContent BuildMultipartContent<TRequest>(TRequest data, bool skipEmptyFiles = false)
        {
            var formData = new MultipartFormDataContent();

            foreach (var prop in typeof(TRequest).GetProperties())
            {
                var value = prop.GetValue(data);

                if (value is IFormFile file)
                {
                    if (skipEmptyFiles && file.Length == 0) continue;

                    var streamContent = new StreamContent(file.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    formData.Add(streamContent, prop.Name, file.FileName);
                }
                else if (value is IEnumerable<IFormFile> files)
                {
                    foreach (var f in files)
                    {
                        if (skipEmptyFiles && f.Length == 0) continue;

                        var streamContent = new StreamContent(f.OpenReadStream());
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue(f.ContentType);
                        formData.Add(streamContent, prop.Name, f.FileName);
                    }
                }
                else if (value != null)
                {
                    formData.Add(new StringContent(value.ToString()), prop.Name);
                }
            }

            return formData;
        }
    }
}
