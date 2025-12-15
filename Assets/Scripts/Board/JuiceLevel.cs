using UnityEngine;
using TMPro;


public class JuiceLevel : MonoBehaviour
{
    [Header("References")]
    public Transform iceCubes;
	public TextMeshPro text;
	public GameStateManager gsm;
    
    [Header("Settings")]
    public int juiceAmnt;
    public float lerpSpeed = 5f;
    
    [Header("Scale Settings")]
    public float maxScale = 176f;
    
    [Header("Ice Position Settings")]
    public float minIceY = -6f;
    public float maxIceY = 0f;
    
    private float targetFullness;
    private float currentFullness;
    private Vector3 targetScale;
    private Vector3 targetIcePos;

    void Start()
    {
        // Initialize to current juice amount
        currentFullness = juiceAmnt / 10f;
        targetFullness = currentFullness;
        
        UpdateTargets();
        
        // Set initial values immediately (no lerp on start)
        transform.localScale = targetScale;
        iceCubes.localPosition = targetIcePos;
    }

    public void SetJuice(int amount)
    {
        juiceAmnt = Mathf.Clamp(amount, 0, 10);
        targetFullness = juiceAmnt / 10f;
        UpdateTargets();
		text.text = $"{juiceAmnt}/{gsm.maxJuiceAmnt}";
    }

	public int GetJuice()
	{
    	return juiceAmnt;
	}

    void Update()
    {
        // Smoothly lerp towards target fullness
        if (Mathf.Abs(currentFullness - targetFullness) > 0.001f)
        {
            currentFullness = Mathf.Lerp(currentFullness, targetFullness, Time.deltaTime * lerpSpeed);
            
            // Calculate current scale and position based on lerped fullness
            Vector3 scale = transform.localScale;
            scale.z = maxScale * currentFullness;
            transform.localScale = scale;
            
            Vector3 icePos = iceCubes.localPosition;
            icePos.y = Mathf.Lerp(minIceY, maxIceY, currentFullness);
            iceCubes.localPosition = icePos;
        }
    }
    
    private void UpdateTargets()
    {
        targetScale = transform.localScale;
        targetScale.z = maxScale * targetFullness;
        
        targetIcePos = iceCubes.localPosition;
        targetIcePos.y = Mathf.Lerp(minIceY, maxIceY, targetFullness);
    }
}