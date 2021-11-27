using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ngTranslator.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ngTranslator.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TranslationController : Controller
    {
        static string subscriptionKey;
        static string apiEndpoint;
        static string location;

        public TranslationController()
        {
            subscriptionKey = "Add your key";
            apiEndpoint = "https://api.cognitive.microsofttranslator.com/";
            location = "Add your location";
        }

        [HttpPost("{textToTranslate}/{targetLanguage}")]
        public async Task<TranslationResultDTO> Post(string textToTranslate, string targetLanguage)
        {
            string route = $"translate?api-version=3.0&to={targetLanguage}";
            string requestUri = apiEndpoint + route;

            string result = await TranslateText(requestUri, textToTranslate);

            TranslationResult[] translationResult = JsonConvert.DeserializeObject<TranslationResult[]>(result);

            TranslationResultDTO translationResultDTO = new TranslationResultDTO
            {
                DetectedLanguage = translationResult[0].DetectedLanguage.Language,
                TranslationOutput = translationResult[0].Translations[0].Text
            };
            return translationResultDTO;
        }

        async Task<string> TranslateText(string requestUri, string inputText)
        {
            string result = string.Empty;
            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);

            using var client = new HttpClient();
            using var request = new HttpRequestMessage();

            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(requestUri);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            request.Headers.Add("Ocp-Apim-Subscription-Region", location);

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }

        [HttpGet]
        public async Task<List<AvailableLanguageDTO>> GetAvailableLanguages()
        {
            string endpoint = "https://api.cognitive.microsofttranslator.com/languages?api-version=3.0&scope=translation";

            var client = new HttpClient();
            using var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(endpoint);

            var response = await client.SendAsync(request).ConfigureAwait(false);

            string result = await response.Content.ReadAsStringAsync();
            AvailableLanguage deserializedOutput = JsonConvert.DeserializeObject<AvailableLanguage>(result);

            List<AvailableLanguageDTO> availableLanguage = new List<AvailableLanguageDTO>();

            foreach (KeyValuePair<string, LanguageDetails> translation in deserializedOutput.Translation)
            {
                AvailableLanguageDTO language = new AvailableLanguageDTO
                {
                    LanguageID = translation.Key,
                    LanguageName = translation.Value.Name
                };

                availableLanguage.Add(language);
            }

            return availableLanguage;
        }
    }
}
