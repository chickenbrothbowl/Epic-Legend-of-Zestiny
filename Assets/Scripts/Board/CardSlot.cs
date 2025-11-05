using UnityEngine;
using UnityEngine.Rendering;

public class CardSlot : MonoBehaviour
{
    public Card currentCard;
    public int slotIndex;
    
    [Header("Border Settings")]
    public Texture2D borderTexture; // Assign in Inspector
    public float borderWidth = 0.05f;       // Line thickness
    public float borderHalfWidth = 1.5f;    // Half of card width (X-axis)
    public float borderHalfHeight = 2f;     // Half of card height (Z-axis)
    public float borderElevation = 0.01f;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.cyan;
    public float glowIntensity = 2f;
    [Min(0.001f)] public float textureTiling = 2f;
    
    private LineRenderer borderLine;
    private Material borderMaterial;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    
    public bool IsEmpty => currentCard == null;
    
    void Start()
    {
        CreateBorderFrame();
        SetBorderGlow(normalColor, 0);
    }
    
    void CreateBorderFrame()
    {
        borderLine = gameObject.AddComponent<LineRenderer>();
    
        borderMaterial = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
        borderMaterial.EnableKeyword("_EMISSION");
		borderMaterial.SetFloat("_Surface", 1f); // 0 = Opaque, 1 = Transparent
        borderMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        borderMaterial.SetInt("_ZWrite", 0);
        borderMaterial.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
        borderMaterial.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        borderMaterial.renderQueue = (int)RenderQueue.Transparent;
        borderLine.material = borderMaterial;

        if (borderTexture != null)
        {
    		borderLine.textureScale = new Vector2(textureTiling, 1);
            borderMaterial.mainTexture = borderTexture;
            borderLine.textureMode = LineTextureMode.Tile;
        }
        
    
        borderLine.loop = true;
        borderLine.widthMultiplier = borderWidth;
        borderLine.useWorldSpace = false;
        borderLine.positionCount = 4;



    
        // Now using different X and Z values for rectangle
        borderLine.SetPosition(0, new Vector3(-borderHalfWidth, borderElevation, -borderHalfHeight));
        borderLine.SetPosition(1, new Vector3(borderHalfWidth, borderElevation, -borderHalfHeight));
        borderLine.SetPosition(2, new Vector3(borderHalfWidth, borderElevation, borderHalfHeight));
        borderLine.SetPosition(3, new Vector3(-borderHalfWidth, borderElevation, borderHalfHeight));
    
        borderLine.alignment = LineAlignment.View;
    }
    
    void OnMouseEnter()
    {
        SetBorderGlow(hoverColor, glowIntensity);
    }
    
    void OnMouseExit()
    {
        SetBorderGlow(normalColor, 0);
    }
    
    public void SetBorderGlow(Color color, float intensity)
    {
        if (borderMaterial != null)
        {
            // Set the base color
            borderMaterial.color = color;
            
            // Set the emission (glow) - multiply by intensity for brightness
            borderMaterial.SetColor(EmissionColor, color * intensity);
        }
    }
    
    public void PlaceCard(Card card)
    {
        if (!IsEmpty) return;
        
        currentCard = card;
        card.transform.SetParent(transform);
        card.transform.localPosition = Vector3.zero;
        StopAllCoroutines();
    }
    
    public Card RemoveCard()
    {
        Card card = currentCard;
        currentCard = null;
        return card;
    }
}