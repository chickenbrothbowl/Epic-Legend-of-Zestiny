using UnityEngine;
public class PersistentStateManager : MonoBehaviour
{
    public static PersistentStateManager Instance { get; private set; }
    [SerializeField]
    public DeckAsset playerDeckAsset;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}