using UnityEngine;
public class Player : MonoBehaviour
{
    public int life = 5;
    public bool isPlayer = false;
    public LifePool lifePool;

    public void DealDamage(int damage)
    {
	    if (isPlayer)
	    {
		    lifePool.MoveCounter(-1 * damage);
	    }
	    else
	    {
		    lifePool.MoveCounter(1 * damage);
	    }
    }
    
    
	void Update(){
		if (life <= 0){
			Debug.Log($"Player {gameObject.name} lost!");
		}
	}
}