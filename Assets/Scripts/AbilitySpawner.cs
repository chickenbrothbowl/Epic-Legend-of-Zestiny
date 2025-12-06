using UnityEngine;

public class AbilitySpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public Card card;
    public float spacing = 1f;
    
    void Start()
    {
        SpawnItems();
    }
    
    void SpawnItems()
    {
        // Clear any existing items first
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (Ability ability in System.Enum.GetValues(typeof(Ability)))
        {
            if (ability == Ability.None || !card.HasAbility(ability))
                continue;

            GameObject item = Instantiate(itemPrefab, transform);
            SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
            sr.sprite = Resources.Load<Sprite>($"AbilityIcons/{ability.ToString().ToLower()}");
            item.transform.localPosition = new Vector3(i * spacing, 0, 0);
            i++;
        }

        // Center them after spawning
        float totalWidth = (i - 1) * spacing;
        foreach (Transform child in transform)
        {
            child.localPosition -= new Vector3(totalWidth / 2f, 0, 0);
        }
    }
}