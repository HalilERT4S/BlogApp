using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlogApp.Domain.Enums;
using BlogApp.Domain.Interfaces;

namespace BlogApp.Application.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            _httpClient.BaseAddress = new Uri("https://api.openai.com/");
        }

        
        public async Task<CommentProfanityStatus> ContainsProfanityAsync(string comment)
        {
            try
            {
                
                var requestData = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = "You are a content moderation assistant." },
                        new { role = "user", content = $"Does the following text contain profanity or offensive language? Reply with only 'Yes' or 'No': '{comment}'" }
                    },
                    max_tokens = 10,
                    temperature = 0.5
                };

                
                var content = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json");

                
                var response = await _httpClient.PostAsync("v1/chat/completions", content);

                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API hatası: {response.StatusCode}, Detay: {errorContent}");
                    throw new Exception($"OpenAI API hatası: {response.StatusCode}, Detay: {errorContent}");
                }

                
                var responseString = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ChatCompletionResponse>(responseString);

               
                var result = responseData?.Choices?[0]?.Message?.Content?.Trim().ToLowerInvariant() ?? "";
                return result.Contains("yes") ? CommentProfanityStatus.Profane : CommentProfanityStatus.Clean;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Küfür kontrolü sırasında hata: {ex.Message}");
                throw;
            }
        }

        
        public async Task<BlogType> IsContentAdultAsync(string content)
        
        {
            try
            {
                var requestData = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                new { role = "system", content = "You are a content moderation assistant." },
                new { role = "user", content = $"Does the following text contain adult content (+18)? Reply with only 'Yes' or 'No': '{content}'" }
            },
                    max_tokens = 5,
                    temperature = 0.5
                };

                var requestContent = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage response = null;

                for (int attempt = 0; attempt < 3; attempt++)
                {
                    response = await _httpClient.PostAsync("v1/chat/completions", requestContent);

                    if (response.IsSuccessStatusCode)
                        break;

                    if ((int)response.StatusCode == 429)
                    {
                        Console.WriteLine("429 - Çok fazla istek. Bekleniyor...");
                        await Task.Delay(2000); 
                    }
                    else
                    {
                        break;
                    }
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API hatası: {response.StatusCode}, Detay: {errorContent}");
                    throw new Exception($"OpenAI API hatası: {response.StatusCode}, Detay: {errorContent}");
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ChatCompletionResponse>(responseString);
                var result = responseData?.Choices?[0]?.Message?.Content?.Trim().ToLowerInvariant() ?? "";

                return result.Contains("yes") ? BlogType.Adult : BlogType.Normal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"İçerik analizi sırasında hata: {ex.Message}");
                throw;
            }
        }

        
        private class ChatCompletionResponse
        {
            [JsonPropertyName("choices")]
            public Choice[] Choices { get; set; }

            public class Choice
            {
                [JsonPropertyName("message")]
                public Message Message { get; set; }
            }

            public class Message
            {
                [JsonPropertyName("content")]
                public string Content { get; set; }
            }
        }
    }
}
