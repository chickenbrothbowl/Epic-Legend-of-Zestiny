using UnityEngine;

public class JuiceLevel : MonoBehaviour
{
    public Transform iceCubes;
    public int juiceAmnt;
    private int oldJuiceAmnt;

    private float fulness;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fulness = juiceAmnt / 10f;
        Vector3 scale = transform.localScale;
        scale.z = 176f * fulness;
        Vector3 icePos = iceCubes.localPosition;
        icePos.y = -6f + (6f * fulness);
        iceCubes.localPosition = icePos;
        transform.localScale = scale;
        oldJuiceAmnt = juiceAmnt;
    }

    public void SetJuice(int amount)
    {
        juiceAmnt = amount;
    }

    // Update is called once per frame
    void Update()
    {
        if (juiceAmnt != oldJuiceAmnt)
        {
            fulness = juiceAmnt / 10f;
            Vector3 scale = transform.localScale;
            scale.z = 176f * fulness;
            Vector3 icePos = iceCubes.localPosition;
            icePos.y = -6f + (6f * fulness);
            iceCubes.localPosition = icePos; 
            transform.localScale = scale;
            oldJuiceAmnt = juiceAmnt;
        }
    }
}
