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

         var acidicToken = columns[5].Trim().ToLower();
         bool acidic = acidicToken == "1" || acidicToken == "true" || acidicToken == "yes";

         var card_data = new CardData
         {
            CardName = columns[0].Trim(),
            CardID = columns[1].Trim(),
            Cost = cost,
            Damage = damage,
            Health = health,
            Acidic = acidic
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

   public virtual void Use()
    {
   
   }
}
