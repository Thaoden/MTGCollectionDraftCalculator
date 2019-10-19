using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTGDraftCollectionCalculator
{
    internal class MtgJsonHelper
    {
        private readonly static string _fileName = "ELD.json";

        internal static async Task<List<Card>> GetEldraineSet()
        {
            Console.WriteLine("Checking set json existence");
            if (!File.Exists(_fileName))
            {
                Console.WriteLine("File not found, downloading from MtgJson");
                await getMtgJsonSet("ELD.json");
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Found set json on disk");
            }

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
            };
            var json = await File.ReadAllTextAsync(_fileName);
            var eldraineSet = JsonSerializer.Deserialize<MtgJsonSet>(json, jsonSerializerOptions);
            var eldraineCards = aggregateDuplicateEntries(eldraineSet);

            return eldraineCards;
        }

        private static async Task getMtgJsonSet(string set)
        {
            string json = "";

            using var _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.mtgjson.com/json/")
            };

            using var response = await _httpClient.GetAsync(set);
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
            }

            await File.WriteAllTextAsync(_fileName, json);
        }

        private static List<Card> aggregateDuplicateEntries(MtgJsonSet completeSet)
        {
            var cardDictionary = new Dictionary<string, Card>();
            foreach (var card in completeSet.Cards.Where(c => c.IsArena))
            {
                if (!cardDictionary.ContainsKey(card.Name) && !cardDictionary.Values.Select(c => c.Number).Any(n => n == card.Number))
                {
                    cardDictionary.Add(card.Name, card);
                }
            }

            return cardDictionary.Values.ToList();
        }
    }
}
