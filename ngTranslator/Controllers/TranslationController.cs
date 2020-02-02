using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlazorTranslator.Models;
using System.Net.Http;
using System.Text;
using ngTranslator.Models;

namespace ngTranslator.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TranslationController : Controller
    {
        [HttpPost("{textToTranslate}/{targetLanguage}")]
        public async Task<TranslationResultDTO> Post(string textToTranslate, string targetLanguage)
        {
            TranslationResult[] translationResultList = await TranslateText(textToTranslate, targetLanguage);

            TranslationResultDTO translationResult = new TranslationResultDTO
            {
                DetectedLanguage = translationResultList[0].DetectedLanguage.Language,
                TranslationOutput = translationResultList[0].Translations[0].Text
            };
            return translationResult;
        }

        async Task<TranslationResult[]> TranslateText(string inputText, string targetLanguage)
        {
            string subscriptionKey = "af19d996a3cb4a70a808567aad5bc41a";
            string translateTextEndpoint = "https://api.cognitive.microsofttranslator.com/";
            string route = $"/translate?api-version=3.0&to={targetLanguage}";
            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(translateTextEndpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                HttpResponseMessage responseMessage = await client.SendAsync(request).ConfigureAwait(false);
                string result = await responseMessage.Content.ReadAsStringAsync();
                TranslationResult[] translationResult = JsonConvert.DeserializeObject<TranslationResult[]>(result);

                return translationResult;
            }
        }

        [HttpGet]
        public async Task<List<AvailableLanguageDTO>> GetAvailableLanguages()
        {
            string endpoint = "https://api.cognitive.microsofttranslator.com/languages?api-version=3.0&scope=translation";
            var client = new HttpClient();
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(endpoint);
                var response = await client.SendAsync(request).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync();

                AvailableLanguage deserializedOutput = JsonConvert.DeserializeObject<AvailableLanguage>(result);

                List<AvailableLanguageDTO> availableLanguage = new List<AvailableLanguageDTO>();

                foreach (KeyValuePair<string, LanguageDetails> translation in deserializedOutput.Translation)
                {
                    AvailableLanguageDTO language = new AvailableLanguageDTO();
                    language.LanguageID = translation.Key;
                    language.LanguageName = translation.Value.Name;

                    availableLanguage.Add(language);
                }
                return availableLanguage;
            }
        }
    }
}
