using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map
{
    public class MapPlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelecting = false;
        public float enterNodeDelay = 1f;
        public MapManager mapManager;
        public MapView view;

        public static MapPlayerTracker Instance;

        public bool Locked { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SelectNode(MapNode mapNode)
        {
            if (Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                Vector2Int currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                Node currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();

            DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
        }
        private DeckAsset pendingPlayerDeck;
        private DeckAsset pendingEnemyDeck;

        void LoadGameAgainstDeck(DeckAsset enemyDeck)
        {
            LoadGameWithDecks(PersistentStateManager.Instance.playerDeckAsset, enemyDeck);
        }

        void LoadGameWithDecks(DeckAsset playerDeck, DeckAsset enemyDeck)
        {
            pendingPlayerDeck = playerDeck;
            pendingEnemyDeck = enemyDeck;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("Battle");
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameObject.Find("Deck").GetComponent<Deck>().deckAsset = pendingPlayerDeck;
            
            
            GameObject.Find("EnemyDeck").GetComponent<Deck>().deckAsset = pendingEnemyDeck;
    
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public static DeckAsset GetDeck(string deckName)
        {
            DeckAsset deckAsset = Resources.Load<DeckAsset>($"Data/EnemyDecks/{deckName}");
            Debug.Log(deckAsset);
            return deckAsset;
        }

        private static void EnterNode(MapNode mapNode)
        {
            // we have access to blueprint name here as well
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false
            DeckAsset enemyDeck;
            switch (mapNode.Node.nodeType)
            {
                
                case NodeType.MinorEnemy:
                    int randomIndex = UnityEngine.Random.Range(0, 2); // Max is exclusive!
                    switch (randomIndex)
                    {
                        case 0:
                            enemyDeck = GetDeck("goblins 1");
                            break;
                        case 1:
                            enemyDeck = GetDeck("WildAnimals");
                            break;
                        default:
                            enemyDeck = GetDeck("goblins 1");
                            break;
                    }
                    Instance.LoadGameAgainstDeck(enemyDeck);
                    break;
                case NodeType.EliteEnemy:
                    enemyDeck = GetDeck("Limoncello");
                    Instance.LoadGameAgainstDeck(enemyDeck);
                    break;
                case NodeType.RestSite:
                    SceneManager.LoadScene("Treasure");
                    break;
                case NodeType.Treasure:
                    SceneManager.LoadScene("Treasure");
                    break;
                case NodeType.Store:
                    SceneManager.LoadScene("Store");
                    break;
                case NodeType.Boss:
                    enemyDeck = GetDeck("Limoncello");
                    Instance.LoadGameAgainstDeck(enemyDeck);
                    break;
                case NodeType.Mystery:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}