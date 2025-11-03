using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class CardParser
{

   public static List<CardData> ParseCsv()
   {
      string path = Path.Combine(Application.dataPath, "Scripts/Data/lemon_document.csv");

      if (!File.Exists(path))
      {
         Debug.LogError($"CardParser: CSV file not found at '{path}'");
         return new List<CardData>();
      }

      var lines = File.ReadAllLines(path).Skip(1);
      var cards = new List<CardData>();

      foreach (var line in lines)
      {
         if (string.IsNullOrWhiteSpace(line))
            continue;

         var columns = line.Split(',');
         if (columns.Length < 6)
            continue;

         int cost = 0, damage = 0, health = 0;
         int.TryParse(columns[2], out cost);
         int.TryParse(columns[3], out damage);
         int.TryParse(columns[4], out health);

         var acidicToken = columns[5];
         bool acidic = acidicToken == "1";

         var catchToken = columns[6];
         bool catchThis = catchToken == "1"; 
         //"CATCH" renamed due to keyword conflict

         var corrosiveToken = columns[7];
         bool corrosive = corrosiveToken == "1";

         var finesseToken = columns[8];
         bool finesse = finesseToken == "1";

         var flyingToken = columns[9];
         bool flying = flyingToken == "1";

         var gluttenousToken = columns[10];
         bool gluttenous = gluttenousToken == "1";

         var hardenedToken = columns[11];
         bool hardened = hardenedToken == "1";

         var harvestToken = columns[12];
         bool harvest = harvestToken == "1";
         
         var juicedToken = columns[13];
         bool juiced = juicedToken == "1";
         
         var juicyToken = columns[14];
         bool juicy = juicyToken == "1";

         var card_data = new CardData
         {
            CardName = columns[0].Trim(),
            CardID = columns[1].Trim(),
            Cost = cost,
            Damage = damage,
            Health = health,
            Acidic = acidic,
            Catch = catchThis,
            Corrosive = corrosive,
            Finesse = finesse,
            Flying = flying,
            Gluttenous = gluttenous,
            Hardened = hardened,
            Harvest = harvest,
            Juiced = juiced,
            Juicy = juicy
         };

         cards.Add(card_data);
      }

      Debug.Log("Card Log:");
      foreach (var card_data in cards)
      {
         Debug.Log($"{card_data.CardName} {card_data.CardID} {card_data.Cost} {card_data.Damage} {card_data.Health} {card_data.Acidic}");
      }

      return cards;
   }
}


public class CardData
{
   public string CardName { get; set; } = string.Empty;
   public string CardID { get; set; } = string.Empty;
   public int Cost { get; set; }
   public int Damage { get; set; }
   public int Health { get; set; }
   public bool Acidic { get; set; }
   public bool Catch { get; set; }
   public bool Corrosive { get; set; }
   public bool Finesse { get; set; }   
   public bool Flying { get; set; }
   public bool Gluttenous { get; set; }
   public bool Hardened { get; set; }
   public bool Harvest { get; set; }
   public bool Juiced { get; set; }
   public bool Juicy { get; set; }

   public virtual void Use()
    {
   
   }
}
