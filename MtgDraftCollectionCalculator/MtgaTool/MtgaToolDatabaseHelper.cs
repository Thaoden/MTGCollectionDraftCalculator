using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTGDraftCollectionCalculator.MtgaTool
{
    internal sealed class MtgaToolDatabaseHelper : IDisposable
    {
        private readonly HttpClient _httpClient;
        private const string DATABASE_FILENAME = "database.json";

        private readonly Lazy<Task<MtgaToolDatabase>> _mtgaToolDatabase;

        #region Construction & Disposal

        public MtgaToolDatabaseHelper()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://mtgatool.com/")
            };

            _mtgaToolDatabase = new Lazy<Task<MtgaToolDatabase>>(getLatestMtgaToolDatabase);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        private async Task<MtgaToolDatabase> getLatestMtgaToolDatabase()
        {
            var dvi = await getDatabaseVersionInfo();

            if (File.Exists(DATABASE_FILENAME))
            {
                var mtb = await getMtgaToolDatabaseFromFileCache();
                Console.WriteLine($"Local cached database found, has version {mtb.Version}");

                if (mtb.Version >= dvi.Latest)
                {
                    return mtb;
                }

                Console.WriteLine("Updating database...");
            }

            return await getMtgaToolDatabaseFromServer();
        }

        private async Task<MtgaToolDatabase> getMtgaToolDatabaseFromServer()
        {
            Console.WriteLine("Getting database from server");
            string databaseJson = await fetchDatabaseFromServer();
            Console.WriteLine("Success");

            return deserializeDatabaseJson(databaseJson);
        }

        private static MtgaToolDatabase deserializeDatabaseJson(string databaseJson)
        {
            var serializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Deserialize<MtgaToolDatabase>(databaseJson, serializerOptions);
        }

        private async Task<MtgaToolDatabase> getMtgaToolDatabaseFromFileCache()
        {
            string databaseJson = await File.ReadAllTextAsync(DATABASE_FILENAME);

            return deserializeDatabaseJson(databaseJson);
        }

        private async Task<DatabaseVersionInfo> getDatabaseVersionInfo()
        {
            Console.WriteLine("Getting database version info from server");
            var serializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true
            };

            var dvi = new DatabaseVersionInfo();

            using (var response = await _httpClient.GetAsync("database/latest"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var dviJson = await response.Content.ReadAsStringAsync();
                    dvi = JsonSerializer.Deserialize<DatabaseVersionInfo>(dviJson, serializerOptions);
                }
            }

            Console.WriteLine($"Latest database version: {dvi.Latest}");

            return dvi;
        }

        private async Task<string> fetchDatabaseFromServer()
        {
            string databaseJson = "";

            using (var response = await _httpClient.GetAsync("database"))
            {
                if (response.IsSuccessStatusCode)
                {
                    databaseJson = await response.Content.ReadAsStringAsync();
                    await File.WriteAllTextAsync(DATABASE_FILENAME, databaseJson);
                }
            }

            return databaseJson;
        }

        #endregion

        internal async Task<List<DatabaseCard>> GetEldraineSetAsync()
        {
            var mtb = await _mtgaToolDatabase.Value;

            return mtb.Cards.Where(c => c.Value.Set == "Throne of Eldraine").Select(kvp => kvp.Value).ToList();
        }

        internal async Task<List<(string Name, Set Set)>> GetAllSetsDetails()
        {
            var mtb = await _mtgaToolDatabase.Value;

            return mtb.Sets.Select(kvp => (kvp.Key, kvp.Value)).ToList();
        }
    }

    public class DatabaseVersionInfo
    {
        public int Latest { get; set; }
        public string Lang { get; set; } = String.Empty;
        public double Updated { get; set; }
        public int Size { get; set; }
    }

    public class MtgaToolDatabase
    {
        public Dictionary<string, DatabaseCard> Cards { get; set; } = new Dictionary<string, DatabaseCard>();
        public bool Ok { get; set; }
        public int Version { get; set; }
        public string Language { get; set; } = String.Empty;
        public double Updated { get; set; }
        public List<object> Events { get; set; } = new List<object>();
        public List<object> EventsFormat { get; set; } = new List<object>();
        public Dictionary<string, Set> Sets { get; set; } = new Dictionary<string, Set>();
        public List<object> Abilities { get; set; } = new List<object>();
        public List<object> LimitedRankedEvents { get; set; } = new List<object>();
        public List<object> StandardRankedEvents { get; set; } = new List<object>();
        public List<object> SingleMatchEvents { get; set; } = new List<object>();
        public List<object> Archetypes { get; set; } = new List<object>();
    }

    public class Set
    {
        public JsonElement Collation { get; set; }
        public string Scryfall { get; set; } = String.Empty;
        public string Code { get; set; } = String.Empty;
        public string ArenaCode { get; set; } = String.Empty;
        public int Tile { get; set; }
        public string Release { get; set; } = String.Empty;
    }

    public class DatabaseCard
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Set { get; set; } = String.Empty;
        public int ArtId { get; set; }
        public string Type { get; set; } = String.Empty;
        public List<string> Cost { get; set; } = new List<string>();
        public int Cmc { get; set; }
        public string Rarity { get; set; } = String.Empty;
        public string Cid { get; set; } = String.Empty;
        public List<int> Frame { get; set; } = new List<int>();
        public string Artist { get; set; } = String.Empty;
        public int Dfc { get; set; }
        public bool Collectible { get; set; }
        public bool Craftable { get; set; }
        public bool Booster { get; set; }
        public JsonElement DfcId { get; set; }
        public int Rank { get; set; }
        public List<int> Rank_Values { get; set; } = new List<int>();
        public JsonElement Rank_Controversy { get; set; }
        public JsonElement Images { get; set; }
        public JsonElement Reprints { get; set; }
    }
}
