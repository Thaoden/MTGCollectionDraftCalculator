using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MtgaLogParser
{
    public class MtgaLogHelper
    {
        private const string BEGIN_GET_PLAYER_CARDS = "[UnityCrossThreadLogger]<== PlayerInventory.GetPlayerCardsV3";
        private const string BEGIN_GET_PLAYER_INVENTORY = "[UnityCrossThreadLogger]<== PlayerInventory.GetPlayerInventory";

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public MtgaLogHelper()
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<(int CardIndex, int CardsOwned)>> GetOwnedCardCollection()
        {
            var ownedCardDictionary = await parseMtgaLog<Dictionary<string, int>>(BEGIN_GET_PLAYER_CARDS);
            return ownedCardDictionary.Select(item => (Int32.Parse(item.Key), item.Value)).ToList();
        }


        public async Task<PlayerInventory> GetPlayerInventory()
        {
            return await parseMtgaLog<PlayerInventory>(BEGIN_GET_PLAYER_INVENTORY);
        }

        private async Task<T> parseMtgaLog<T>(string beginLogEntryMark)
        {
            var localLowPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString() + "Low";
            var mtgaLogFilePath = Path.Combine(localLowPath, "Wizards Of The Coast", "MTGA", "output_log.txt");
            var mtgaLogFileContent = "";
            using (var fileReader = File.OpenRead(mtgaLogFilePath))
            using (var streamReader = new StreamReader(fileReader))
            {
                mtgaLogFileContent = await streamReader.ReadToEndAsync();
            }

            var mtgaLogFileContentLines = mtgaLogFileContent
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => line.StartsWith(beginLogEntryMark))
                .ToList();
            //var lastDumpBeginningIndex = Array.IndexOf(mtgaLogFileContentLines, mtgaLogFileContentLines.Last(c => c.StartsWith(beginLogEntryMark)));
            //var lastDumpEndingIndex = Array.IndexOf(mtgaLogFileContentLines.Skip(lastDumpBeginningIndex).ToArray(), "}");
            //var lastDump = mtgaLogFileContentLines.Skip(lastDumpBeginningIndex + 1).Take(lastDumpEndingIndex).ToList();
            //var cardBuilder = new StringBuilder();
            //lastDump.ForEach(line => cardBuilder.Append($"{line}"));
            //var cardJson = cardBuilder.ToString();

            using var jsonDoc = JsonDocument.Parse(mtgaLogFileContentLines.Last().Replace(beginLogEntryMark, ""));

            var payload = jsonDoc
                .RootElement
                .GetProperty("payload")
                .GetRawText();

            return JsonSerializer.Deserialize<T>(payload, _jsonSerializerOptions);
        }
    }

    public class PlayerInventory
    {
        public string PlayerId { get; set; } = String.Empty;
        public int WcCommon { get; set; }
        public int WcUncommon { get; set; }
        public int WcRare { get; set; }
        public int WcMythic { get; set; }
        public int Gold { get; set; }
        public int Gems { get; set; }
        public int DraftTokens { get; set; }
        public int SealedTokens { get; set; }
        public int WcTrackPosition { get; set; }
        public double VaultProgress { get; set; }
        public List<Booster> Boosters { get; set; } = new List<Booster>();
        public VanityItems VanityItems { get; set; } = new VanityItems();
        public List<JsonElement> Vouchers { get; set; } = new List<JsonElement>();
        public VanitySelections VanitySelections { get; set; } = new VanitySelections();
        public string BasicLandSet { get; set; } = String.Empty;
        public List<Guid> StarterDecks { get; set; } = new List<Guid>();
        public string FirstSeenDate { get; set; } = String.Empty;
    }

    public class VanitySelections
    {
        public string AvatarSelection { get; set; } = String.Empty;
        public string? CardBackSelection { get; set; }
        public PetVariant PetSelection { get; set; } = new PetVariant();
    }

    public class VanityItems
    {
        public List<Pet> Pets { get; set; } = new List<Pet>();
        public List<Avatar> Avatars { get; set; } = new List<Avatar>();
        public List<CardBack> CardBacks { get; set; } = new List<CardBack>();
    }

    public class CardBack
    {
        public string Name { get; set; } = String.Empty;
        public List<JsonElement> Mods { get; set; } = new List<JsonElement>();
    }

    public class Avatar
    {
        public string Name { get; set; } = String.Empty;
        public List<JsonElement> Mods { get; set; } = new List<JsonElement>();
    }

    public class Pet
    {
        public string Name { get; set; } = String.Empty;
        public List<string> Variants { get; set; } = new List<string>();
    }

    public class PetVariant
    {
        public string Type { get; set; } = String.Empty;
        public string Value { get; set; } = String.Empty;
    }

    public class Booster
    {
        public int CollationId { get; set; }
        public int Count { get; set; }
    }
}
