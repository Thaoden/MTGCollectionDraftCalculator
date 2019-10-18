using System;
using System.Collections.Generic;
using System.Text;

namespace MTGDraftCollectionCalculator
{
    public class MtgJsonSet
    {
        public int BaseSetSize { get; set; }
        public string Block { get; set; } = String.Empty;
        public List<object> BoosterV3 { get; set; } = new List<object>();
        public List<Card> Cards { get; set; } = new List<Card>();
        public string Code { get; set; } = String.Empty;
        public string CodeV3 { get; set; } = String.Empty;
        public bool IsForeignOnly { get; set; }
        public bool IsFoilOnly { get; set; }
        public bool IsOnlineOnly { get; set; }
        public bool IsPartialPreview { get; set; }
        public string KeyruneCode { get; set; } = String.Empty;
        public string McmName { get; set; } = String.Empty;
        public int McmId { get; set; }
        public Meta Meta { get; set; } = new Meta();
        public string MtgoCode { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string ParentCode { get; set; } = String.Empty;
        public string ReleaseDate { get; set; } = String.Empty;
        public int TcgPlayerId { get; set; }
        public List<Token> Tokens { get; set; } = new List<Token>();
        public int TotalSetSize { get; set; }
        public Dictionary<string, List<string>> Translations { get; set; } = new Dictionary<string, List<string>>();
        public string Type { get; set; } = String.Empty;
    }

    public class Card
    {
        public string Artist { get; set; } = String.Empty;
        public string BorderColor { get; set; } = String.Empty;
        public List<string> ColorIdentity { get; set; } = new List<string>();
        public List<string> ColorIndicator { get; set; } = new List<string>();
        public List<string> Colors { get; set; } = new List<string>();
        public float ConvertedManaCost { get; set; }
        public int Count { get; set; }
        public string DuelDeck { get; set; } = String.Empty;
        public int EdhrecRank { get; set; }
        public float FaceConvertedManaCost { get; set; }
        public string FlavorText { get; set; } = String.Empty;
        public List<ForeignData> ForeignData { get; set; } = new List<ForeignData>();
        public string FrameEffect { get; set; } = String.Empty;
        public string FrameVersion { get; set; } = String.Empty;
        public string Hand { get; set; } = String.Empty;
        public bool HasFoil { get; set; }
        public bool HasNoDeckLimit { get; set; }
        public bool HasNonFoil { get; set; }
        public bool IsAlternativ { get; set; }
        public bool IsArena { get; set; }
        public bool IsFullArt { get; set; }
        public bool IsMtgo { get; set; }
        public bool IsOnlineOnly { get; set; }
        public bool IsOversized { get; set; }
        public bool IsPaper { get; set; }
        public bool IsPromo { get; set; }
        public bool IsReprint { get; set; }
        public bool IsReserved { get; set; }
        public bool IsStarter { get; set; }
        public bool IsStorySpotlight { get; set; }
        public bool IsTextless { get; set; }
        public bool IsTimeshifted { get; set; }
        public string Layout { get; set; } = String.Empty;
        public LeadershipSkills LeadershipSkills { get; set; } = new LeadershipSkills();
        public Legalities Legalities { get; set; } = new Legalities();
        public string Life { get; set; } = String.Empty;
        public string Loyalty { get; set; } = String.Empty;
        public string ManaCost { get; set; } = String.Empty;
        public int McmId { get; set; }
        public int McmMetaId { get; set; }
        public int McmArenaId { get; set; }
        public int MtgoFoilId { get; set; }
        public int MtgoId { get; set; }
        public int MtgoStocksId { get; set; }
        public int MultiverseId { get; set; }
        public string Name { get; set; } = String.Empty;
        public List<string> Names { get; set; } = new List<string>();
        public string Number { get; set; } = String.Empty;
        public string OriginalText { get; set; } = String.Empty;
        public string OriginalType { get; set; } = String.Empty;
        public string Power { get; set; } = String.Empty;
        public Prices Prices { get; set; } = new Prices();
        public List<string> Printings { get; set; } = new List<string>();
        public PurchaseUrls PurchaseUrls { get; set; } = new PurchaseUrls();
        public string Rarity { get; set; } = String.Empty;
        public List<string> ReverseRelated { get; set; } = new List<string>();
        public List<Ruling> Rulings { get; set; } = new List<Ruling>();
        public string ScryfallId { get; set; } = String.Empty;
        public string ScryfallOracleId { get; set; } = String.Empty;
        public string ScryfallIllustrationId { get; set; } = String.Empty;
        public string Side { get; set; } = String.Empty;
        public List<string> Subtypes { get; set; } = new List<string>();
        public List<string> Supertypes { get; set; } = new List<string>();
        public int TcgPlayerProductId { get; set; }
        public string Text { get; set; } = String.Empty;
        public string Toughness { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty;
        public List<string> Types { get; set; } = new List<string>();
        public string Uuid { get; set; } = String.Empty;
        public List<string> Variations { get; set; } = new List<string>();
        public string Watermark { get; set; } = String.Empty;
    }

    public class ForeignData
    {
        public string FlavorText { get; set; } = String.Empty;
        public string Language { get; set; } = String.Empty;
        public int MultiverseId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Text { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty;
    }

    public class LeadershipSkills
    {
        public bool Brawl { get; set; }
        public bool Commander { get; set; }
        public bool Oathbreaker { get; set; }
    }

    public class Legalities
    {
        public string Brawl { get; set; } = String.Empty;
        public string Commander { get; set; } = String.Empty;
        public string Duel { get; set; } = String.Empty;
        public string Future { get; set; } = String.Empty;
        public string Frontier { get; set; } = String.Empty;
        public string Legacy{ get; set; } = String.Empty;
        public string Modern{ get; set; } = String.Empty;
        public string Pauper { get; set; } = String.Empty;
        public string Penny { get; set; } = String.Empty;
        public string Standard { get; set; } = String.Empty;
        public string Vintage { get; set; } = String.Empty;
    }

    public class Prices
    {
        public Dictionary<string, float> Mtgo { get; set; } = new Dictionary<string, float>();
        public Dictionary<string, float> MtgoFoil { get; set; } = new Dictionary<string, float>();
        public Dictionary<string, float> Paper { get; set; } = new Dictionary<string, float>();
        public Dictionary<string, float> PaperFoil { get; set; } = new Dictionary<string, float>();
    }

    public class PurchaseUrls
    {
        public string Cardmarket { get; set; } = String.Empty;
        public string TcgPlayer { get; set; } = String.Empty;
        public string MtgStocks { get; set; } = String.Empty;
    }

    public class Ruling
    {
        public string Date { get; set; } = String.Empty;
        public string Text { get; set; } = String.Empty;
    }

    public class Meta
    {
        public string Date { get; set; } = String.Empty;
        public string PricesDate { get; set; } = String.Empty;
        public string Version { get; set; } = String.Empty;
    }

    public class Token
    {
        public string Artist { get; set; } = String.Empty;
        public string BorderColor { get; set; } = String.Empty;
        public List<string> ColorIdentity { get; set; } = new List<string>();
        public List<string> ColorIndicator { get; set; } = new List<string>();
        public List<string> Colors { get; set; } = new List<string>();
        public bool IsOnlineOnly { get; set; }
        public string Layout { get; set; } = String.Empty;
        public string Loyalty { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public List<string> Names { get; set; } = new List<string>();
        public string Number { get; set; } = String.Empty;
        public string Power { get; set; } = String.Empty;
        public List<string> ReverseRelated { get; set; } = new List<string>();
        public string ScryfallId { get; set; } = String.Empty;
        public string ScryfallOracleId { get; set; } = String.Empty;
        public string ScryfallIllustrationId { get; set; } = String.Empty;
        public string Side { get; set; } = String.Empty;
        public string Text { get; set; } = String.Empty;
        public string Toughness { get; set; } = String.Empty;
        public string Type { get; set; } = String.Empty;
        public string Uuid { get; set; } = String.Empty;
        public string Watermark { get; set; } = String.Empty;
    }
}
