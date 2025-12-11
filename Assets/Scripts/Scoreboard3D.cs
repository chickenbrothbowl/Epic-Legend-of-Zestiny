using UnityEngine;
using TMPro;

public class Scoreboard3D : MonoBehaviour
{
    public float cellWidth = 1f;
    public float cellHeight = 1f;
    public TMP_FontAsset customFont;
    public Material customMaterial;

    
    void Start()
    {
        CreateScoreboard();
    }
    
    void CreateScoreboard()
    {
        for (int i = 0; i <= 10; i++)
        {
            // Create a quad for the background square
            GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad);
            cell.transform.parent = transform;
            cell.transform.localPosition = new Vector3(i * cellWidth, 0, 0);
            cell.transform.localRotation = Quaternion.Euler(90, 0, 0); // Face upward
            cell.transform.localScale = new Vector3(cellWidth * 0.9f, cellHeight * 0.9f, 1);
            cell.name = "Cell_" + i;
            MeshRenderer mr = cell.GetComponent<MeshRenderer>();
            mr.material = customMaterial;
            
            // Create TextMeshPro 3D text
            GameObject textObj = new GameObject("Number");
            textObj.transform.parent = cell.transform;
            textObj.transform.localPosition = new Vector3(0, 0, -0.01f); // Slightly above quad
            textObj.transform.localRotation = Quaternion.identity;
            
            TextMeshPro tmp = textObj.AddComponent<TextMeshPro>();
            tmp.text = (i - 5).ToString();
            tmp.font = customFont; // Apply custom font
            tmp.fontSize = 10;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.black;
            
            // Adjust text size to fit cell
            RectTransform rectTransform = tmp.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);
        }
    }
}