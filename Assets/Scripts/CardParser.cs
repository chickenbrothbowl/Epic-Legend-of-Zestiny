using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum Ability
{
    None        = 0,
    Acidic      = 1 << 0,
    Catch       = 1 << 1,
    Corrosive   = 1 << 2,
    Finesse     = 1 << 3,
    Flying      = 1 << 4,
    Gluttenous  = 1 << 5,
    Hardened    = 1 << 6,
    Harvest     = 1 << 7,
    Juiced      = 1 << 8,
    Juicy       = 1 << 9,
    Opportunist = 1 << 10,
    Pummel      = 1 << 11,
    Reach       = 1 << 12,
    Rotten      = 1 << 13,
    Shielded    = 1 << 14,
    Tribal      = 1 << 15,
    Vampire     = 1 << 16
}

public static class CardParser
{
    // Maps CSV column index to the corresponding Ability flag
    private static readonly Dictionary<int, Ability> ColumnToAbility = new()
    {
        { 5,  Ability.Acidic },
        { 6,  Ability.Catch },
        { 7,  Ability.Corrosive },
        { 8,  Ability.Finesse },
        { 9,  Ability.Flying },
        { 10, Ability.Gluttenous },
        { 11, Ability.Hardened },
        { 12, Ability.Harvest },
        { 13, Ability.Juiced },
        { 14, Ability.Juicy },
        { 15, Ability.Opportunist },
        { 16, Ability.Pummel },
        { 17, Ability.Reach },
        { 18, Ability.Rotten },
        { 19, Ability.Shielded },
        { 20, Ability.Tribal },
        { 21, Ability.Vampire }
    };

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
            if (columns.Length < 22)
                continue;

            int.TryParse(columns[2], out int cost);
            int.TryParse(columns[3], out int damage);
            int.TryParse(columns[4], out int health);

            // Parse all abilities in one pass
            Ability abilities = Ability.None;
            foreach (var kvp in ColumnToAbility)
            {
                if (columns[kvp.Key] == "1")
                    abilities |= kvp.Value;
            }

            var card_data = new CardData
            {
                CardName = columns[0].Trim(),
                CardID = columns[1].Trim(),
                Cost = cost,
                Damage = damage,
                Health = health,
                Abilities = abilities
            };

            cards.Add(card_data);
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
    public Ability Abilities { get; set; }

    // Helper method for checking abilities
    public bool HasAbility(Ability ability) => Abilities.HasFlag(ability);
    
    public IEnumerable<Ability> GetActiveAbilities()
    {
        foreach (Ability ability in System.Enum.GetValues(typeof(Ability)))
        {
            if (ability != Ability.None && Abilities.HasFlag(ability))
                yield return ability;
        }
    }

    public virtual void Use() { }
}