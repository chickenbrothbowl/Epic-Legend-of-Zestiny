using UnityEngine;
public class LemonSpawner : MonoBehaviour
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
        int numberOfItems = card.cost;
        // Clear any existing items first
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        float totalWidth = (numberOfItems - 1) * spacing;
        float startX = -totalWidth / 2f;
    
        for (int i = 0; i < numberOfItems; i++)
        {
            GameObject item = Instantiate(itemPrefab, transform);
            item.transform.localPosition = new Vector3(startX + (i * spacing), 0, 0);
        }
    }
}