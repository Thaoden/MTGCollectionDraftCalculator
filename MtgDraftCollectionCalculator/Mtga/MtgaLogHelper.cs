using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTGDraftCollectionCalculator.Mtga
{
    internal class MtgaLogHelper
    {
        private const string BEGIN_GET_PLAYER_CARDS = "<== PlayerInventory.GetPlayerCardsV3";

        public async Task<List<(int CardIndex, int CardsOwned)>> ParseMtgArenaLog()
        {
            var localLowPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString() + "Low";
            var mtgaLogFilePath = Path.Combine(localLowPath, "Wizards Of The Coast", "MTGA", "output_log.txt");
            var mtgaLogFileContent = "";
            using (var fileReader = File.OpenRead(mtgaLogFilePath))
            using (var streamReader = new StreamReader(fileReader))
            {
                mtgaLogFileContent = await streamReader.ReadToEndAsync();
            }

            var mtgaLogFileContentLines = mtgaLogFileContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var lastCardDumpBeginningIndex = Array.IndexOf(mtgaLogFileContentLines, mtgaLogFileContentLines.Last(c => c.StartsWith(BEGIN_GET_PLAYER_CARDS)));
            var lastCardDumpEndingIndex = Array.IndexOf(mtgaLogFileContentLines.Skip(lastCardDumpBeginningIndex).ToArray(), "}");
            var lastCardDump = mtgaLogFileContentLines.Skip(lastCardDumpBeginningIndex + 1).Take(lastCardDumpEndingIndex).ToList();
            var cardDumpBuilder = new StringBuilder();
            lastCardDump.ForEach(line => cardDumpBuilder.Append($"{line}"));
            var cardDumpJson = cardDumpBuilder.ToString();

            var ownedCardsDic = JsonSerializer.Deserialize<Dictionary<string, int>>(cardDumpJson);
            var returnList = ownedCardsDic.Select(item => (Int32.Parse(item.Key), item.Value)).ToList();

            return returnList;
        }
    }
}
