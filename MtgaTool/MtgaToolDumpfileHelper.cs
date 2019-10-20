using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MTGDraftCollectionCalculator.MtgaTool
{
    internal class MtgaToolDumpfileHelper
    {
        private static readonly string _userCollectionFilename = "mtgcollection.csv";

        public async Task<List<MtgaToolCard>> ParseMtgaToolDump()
        {
            if (!File.Exists(_userCollectionFilename))
            {
                Console.WriteLine($"MTGATool dump file with filename {_userCollectionFilename} not found, assuming empty collection");
                return new List<MtgaToolCard>();
            }

            Console.WriteLine("Found MTGATool dump file, parsing it");
            var userCollectionCsv = await File.ReadAllTextAsync(_userCollectionFilename);
            var userCollection = new List<MtgaToolCard>();

            foreach (var cardEntryLine in userCollectionCsv.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                var cardEntry = cardEntryLine.Split(";");
                var card = new MtgaToolCard
                {
                    Name = cardEntry[0].Replace("\"", ""),
                    SetCode = cardEntry[1],
                    SetName = cardEntry[2],
                    Rarity = cardEntry[3],
                    Count = Int32.Parse(cardEntry[4])
                };

                if (card.SetCode == "ELD")
                {
                    userCollection.Add(card);
                }
            }

            return userCollection;
        }
    }
}
