using UnityEngine;
public class Player : MonoBehaviour
{
    public int life = 5;

	void Update(){
		if (life <= 0){
			Debug.Log($"Player {gameObject.name} lost!");
		}
	}
}