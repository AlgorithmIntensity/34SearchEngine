using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


// DO NOT TOUCH IT!


/*

if (lang == "ru-RU")
{
    Log(smth);
}
else
{
    Log(smthButEng);
}

this is a example of translation to languages (for logs)

before use it, add this:
CultureInfo systemCulture = CultureInfo.InstalledUICulture;
string lang = systemCulture.Name;
*/


namespace _34Downloader.Engine
{
    public class Downloader
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string API_BASE_URL = "https://api.rule34.xxx";
        private readonly int _limit;
        private readonly string _tags;
        private readonly Dictionary<string, string> _params;
        private readonly RichTextBox _logBox;
        private readonly SynchronizationContext _syncContext;

        public Downloader(string tags, RichTextBox logBox = null, int limit = 50)
        {
            _tags = tags;
            _limit = limit;
            _logBox = logBox;
            _syncContext = SynchronizationContext.Current;

            _params = new Dictionary<string, string>
            {
                ["page"] = "dapi",
                ["s"] = "post",
                ["q"] = "index",
                ["tags"] = tags,
                ["limit"] = limit.ToString(),
                ["json"] = "1",
                ["api_key"] = "ur_api_key",
                ["user_id"] = "ur_user_id"
            };
        }

        private void Log(string message)
        {
            if (_logBox != null && _syncContext != null)
            {
                _syncContext.Post(_ =>
                {
                    _logBox.AppendText($"{DateTime.Now:HH:mm:ss} {message}{Environment.NewLine}{Environment.NewLine}");
                    _logBox.ScrollToCaret();
                }, null);
            }
            else
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss} {message}");
            }
        }

        private static string GetFileExtension(string fileUrl, string contentType = null)
        {
            var uri = new Uri(fileUrl);
            var path = uri.AbsolutePath.ToLower();

            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
            string[] videoExtensions = { ".mp4", ".webm", ".avi", ".mov", ".mkv" };

            foreach (var ext in imageExtensions.Concat(videoExtensions))
            {
                if (path.EndsWith(ext))
                    return ext;
            }

            if (!string.IsNullOrEmpty(contentType))
            {
                var mimeType = contentType.Split(';')[0];
                var extension = MimeTypeToExtension(mimeType);
                return extension ?? ".bin";
            }

            return ".bin";
        }

        private static string MimeTypeToExtension(string mimeType)
        {
            switch (mimeType)
            {
                case "image/jpeg": return ".jpg";
                case "image/png": return ".png";
                case "image/gif": return ".gif";
                case "image/webp": return ".webp";
                case "image/bmp": return ".bmp";
                case "video/mp4": return ".mp4";
                case "video/webm": return ".webm";
                case "video/avi": return ".avi";
                case "video/quicktime": return ".mov";
                case "video/x-matroska": return ".mkv";
                default: return null;
            }
        }

        private static string GetCacheKey(string fileUrl)
        {
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(fileUrl));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private async Task<string> DownloadFileAsync(string fileUrl, int maxRetries = 3)
        {
            var cacheName = GetCacheKey(fileUrl);
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    using (var response = await _httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        var contentType = response.Content.Headers.ContentType?.MediaType;
                        var extension = GetFileExtension(fileUrl, contentType);

                        string folder;
                        if (new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" }.Contains(extension))
                            folder = "images";
                        else if (new[] { ".mp4", ".webm", ".avi", ".mov", ".mkv" }.Contains(extension))
                            folder = "videos";
                        else
                            folder = "other";

                        var cachePath = $"downloads/cache/{_tags}/{cacheName}{extension}";
                        var finalPath = $"downloads/{folder}/{_tags}/{cacheName}{extension}";

                        if (File.Exists(finalPath))
                        {
                            CultureInfo systemCulture2 = CultureInfo.InstalledUICulture;
                            string lang2 = systemCulture2.Name;
                            if (lang2 == "ru-RU")
                            {
                                Log($"✓ Уже скачан: {fileUrl}");
                            }
                            else
                            {
                                Log($"✓ Already downloaded: {fileUrl}");
                            }
                            return finalPath;
                        }

                        using (var fileStream = new FileStream(cachePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        using (var httpStream = await response.Content.ReadAsStreamAsync())
                        {
                            await httpStream.CopyToAsync(fileStream);
                        }

                        File.Move(cachePath, finalPath);
                        CultureInfo systemCulture1 = CultureInfo.InstalledUICulture;
                        string lang = systemCulture1.Name;
                        if (lang == "ru-RU")
                        {
                            Log($"✓ Успешно [{folder}]: {fileUrl}");
                        }
                        else
                        {
                            Log($"✓ Sucefully [{folder}]: {fileUrl}");
                        }
                        return finalPath;
                    }
                }
                catch (TaskCanceledException) when (attempt < maxRetries - 1)
                {
                    CultureInfo systemCulture1 = CultureInfo.InstalledUICulture;
                    string lang = systemCulture1.Name;
                    if (lang == "ru-RU")
                    {
                        Log($"✗ Таймаут {attempt + 1}/{maxRetries} для {fileUrl}");
                    }
                    else
                    {
                        Log($"✗ Timeout {attempt + 1}/{maxRetries} для {fileUrl}");
                    }
                    
                }
                catch (HttpRequestException ex) when (attempt < maxRetries - 1)
                {
                    CultureInfo systemCulture1 = CultureInfo.InstalledUICulture;
                    string lang = systemCulture1.Name;
                    if (lang == "ru-RU")
                    {
                        Log($"✗ Ошибка сети {attempt + 1}/{maxRetries} для {fileUrl}: {ex.Message}");
                    }
                    else
                    {
                        Log($"✗ Network Error {attempt + 1}/{maxRetries} для {fileUrl}: {ex.Message}");
                    }
                }
                catch (IOException ex) when (attempt < maxRetries - 1)
                {
                    CultureInfo systemCulture1 = CultureInfo.InstalledUICulture;
                    string lang = systemCulture1.Name;
                    if (lang == "ru-RU")
                    {
                        Log($"✗ Ошибка файловой системы для {fileUrl}: {ex.Message}");
                    }
                    else
                    {
                        Log($"✗ File system error for {fileUrl}: {ex.Message}");
                    }
                    break;
                }

                if (attempt < maxRetries - 1)
                {
                    await Task.Delay((int)Math.Pow(2, attempt) * 1000);
                }
            }

            var cacheDir = $"downloads/cache/{_tags}";
            if (Directory.Exists(cacheDir))
            {
                foreach (var file in Directory.GetFiles(cacheDir))
                {
                    if (Path.GetFileNameWithoutExtension(file) == cacheName)
                    {
                        try { File.Delete(file); } catch { }
                    }
                }
            }

            return null;
        }

        public async Task RunAsync()
        {
            Directory.CreateDirectory($"downloads/cache/{_tags}");
            Directory.CreateDirectory($"downloads/images/{_tags}");
            Directory.CreateDirectory($"downloads/videos/{_tags}");
            Directory.CreateDirectory($"downloads/other/{_tags}");
            CultureInfo systemCulture1 = CultureInfo.InstalledUICulture;
            string lang = systemCulture1.Name;
            if (lang == "ru-RU")
            {
                Log("Запрашиваем список файлов с API...");
            }
            else
            {
                Log("Requesting a list of files from the API...");
            }

            try
            {
                var queryString = string.Join("&", _params.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
                var apiUrl = $"{API_BASE_URL}?{queryString}";

                var response = await _httpClient.GetStringAsync(apiUrl);

                JsonDocument doc = null;
                try
                {
                    doc = JsonDocument.Parse(response);
                    var data = doc.RootElement;

                    if (data.ValueKind == JsonValueKind.Null || data.ValueKind == JsonValueKind.Undefined)
                    {
                        if (lang == "ru-RU")
                        {
                            Log("Нет данных для скачивания (пустой ответ от API)");
                        }
                        else
                        {
                            Log("No data to download (empty response from API)");
                        }
                        
                        return;
                    }

                    List<string> fileUrls = new List<string>();

                    if (data.ValueKind == JsonValueKind.Array)
                    {
                        if (lang == "ru-RU")
                        {
                            Log("API вернул массив постов");
                        }
                        else
                        {
                            Log("API returned an array of posts");
                        }

                        foreach (var post in data.EnumerateArray())
                        {
                            ExtractFileUrlFromPost(post, fileUrls);
                        }
                    }
                    else if (data.ValueKind == JsonValueKind.Object)
                    {
                        if (lang == "ru-RU")
                        {
                            Log("API вернул объект с постами");
                        }
                        else
                        {
                            Log("API returned an object with posts");
                        }
                        

                        string[] possibleKeys = { "post", "posts", "data", "items", "results" };

                        foreach (var key in possibleKeys)
                        {
                            if (data.TryGetProperty(key, out var postsProp) &&
                                postsProp.ValueKind == JsonValueKind.Array)
                            {
                                if (lang == "ru-RU")
                                {
                                    Log($"Найден ключ '{key}' с массивом постов");
                                }
                                else
                                {
                                    Log($"Found key '{key}' with array of posts");
                                }
                                foreach (var post in postsProp.EnumerateArray())
                                {
                                    ExtractFileUrlFromPost(post, fileUrls);
                                }
                                break;
                            }
                        }

                        if (fileUrls.Count == 0)
                        {
                            ExtractFileUrlFromPost(data, fileUrls);
                        }
                    }
                    else
                    {
                        if (lang == "ru-RU")
                        {
                            Log($"Неподдерживаемый тип ответа: {data.ValueKind}");
                        }
                        else
                        {
                            Log($"Unsupported response type: {data.ValueKind}");
                        }
                        
                        return;
                    }

                    if (lang == "ru-RU")
                    {
                        Log($"Найдено {fileUrls.Count} файлов для скачивания");
                    }
                    else
                    {
                        Log($"Found {fileUrls.Count} files for download");
                    }

                    if (fileUrls.Count == 0)
                    {
                        if (lang == "ru-RU")
                        {
                            Log("Нет доступных URL для скачивания");
                            Log($"Полный ответ API (первые 500 символов): {response.Substring(0, Math.Min(500, response.Length))}...");
                        }
                        else
                        {
                            Log("No available URLs for download");
                            Log($"Full API response (first 500 characters): {response.Substring(0, Math.Min(500, response.Length))}...");
                        }
                        return;
                    }

                    var typesCount = new ConcurrentDictionary<string, int>
                    {
                        ["images"] = 0,
                        ["videos"] = 0,
                        ["other"] = 0
                    };

                    var maxWorkers = Math.Min(4, fileUrls.Count);
                    var successful = 0;

                    var semaphore = new SemaphoreSlim(maxWorkers);
                    var downloadTasks = new List<Task>();
                    if (lang == "ru-RU")
                    {
                        Log($"Начинаем скачивание с {maxWorkers} потоками...");
                    }
                    else
                    {
                        Log($"Starting download with {maxWorkers} threads...");
                    }

                    foreach (var fileUrl in fileUrls)
                    {
                        await semaphore.WaitAsync();

                        var task = Task.Run(async () =>
                        {
                            try
                            {
                                var result = await DownloadFileAsync(fileUrl);
                                if (result != null)
                                {
                                    Interlocked.Increment(ref successful);

                                    if (result.Contains("downloads/images/"))
                                        typesCount.AddOrUpdate("images", 1, (key, count) => count + 1);
                                    else if (result.Contains("downloads/videos/"))
                                        typesCount.AddOrUpdate("videos", 1, (key, count) => count + 1);
                                    else
                                        typesCount.AddOrUpdate("other", 1, (key, count) => count + 1);
                                }
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        });

                        downloadTasks.Add(task);
                    }

                    await Task.WhenAll(downloadTasks);
                    if (lang == "ru-RU")
                    {
                        Log($"\nИтог скачивания:");
                        Log($"Успешно: {successful}/{fileUrls.Count}");
                        Log($"Изображений: {typesCount["images"]}");
                        Log($"Видео: {typesCount["videos"]}");
                        Log($"Прочих: {typesCount["other"]}");
                    }
                    else
                    {
                        Log($"\nDownload Result:");
                        Log($"Successful: {successful}/{fileUrls.Count}");
                        Log($"Images: {typesCount["images"]}");
                        Log($"Videos: {typesCount["videos"]}");
                        Log($"Other: {typesCount["other"]}");
                    }
                    
                }
                finally
                {
                    doc?.Dispose();
                }
            }
            catch (HttpRequestException ex)
            {
                if (lang == "ru-RU")
                {
                    Log($"Ошибка подключения к API: {ex.Message}");
                }
                else
                {
                    Log($"API connection error: {ex.Message}");
                }
            }
            catch (JsonException ex)
            {
                if (lang == "ru-RU")
                {
                    Log($"Ошибка парсинга JSON: {ex.Message}");
                }
                else
                {
                    Log($"JSON parsing error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                if (lang == "ru-RU")
                {
                    Log($"Критическая ошибка: \n{ex.GetType().Name}: {ex.Message}");
                    Log($"Stack trace: {ex.StackTrace}");
                }
                else
                {
                    Log($"Critical error: \n{ex.GetType().Name}: {ex.Message}");
                    Log($"Stack trace: {ex.StackTrace}");
                }
            }
            finally
            {
                if (lang == "ru-RU")
                {
                    Log("Скачивание завершено.");
                }
                else
                {
                    Log("Download complete.");
                }
            }
        }

        private void ExtractFileUrlFromPost(JsonElement post, List<string> fileUrls)
        {
            CultureInfo systemCulture1 = CultureInfo.InstalledUICulture;
            string lang = systemCulture1.Name;
            string[] possibleUrlKeys = { "file_url", "file", "url", "source", "image", "video", "sample_url", "jpeg_url" };

            foreach (var key in possibleUrlKeys)
            {
                if (post.TryGetProperty(key, out var urlProp) &&
                    urlProp.ValueKind == JsonValueKind.String)
                {
                    string url = urlProp.GetString();
                    if (!string.IsNullOrEmpty(url) &&
                        (url.StartsWith("http://") || url.StartsWith("https://")))
                    {
                        fileUrls.Add(url);
                        if (lang == "ru-RU")
                        {
                            Log($"Найден URL в ключе '{key}': {url}");
                        }
                        else
                        {
                            Log($"Found URL in key '{key}': {url}");
                        }
                        return;
                    }
                }
            }

            if (lang == "ru-RU")
            {
                Log($"Не найден URL в посте. Структура поста: {post}");
            }
            else
            {
                Log($"URL not found in post. Post structure: {post}");
            }
        }
    }

}
