using UnityEngine;
public class Player : MonoBehaviour
{
	public BattleSide battleSide;
    public int life = 5;
    public bool isPlayer = false;
    public LifePool lifePool;

    public void DealDamage(int damage)
    {
	    if (battleSide.isPlayerSide)
    	{
        	lifePool.MoveCounter(-damage);
    	}
    	else
    	{
        	lifePool.MoveCounter(+damage);
    	}
}
    
    
	void Update(){
		if (life <= 0){
			Debug.Log($"Player {gameObject.name} lost!");
		}
	}
}