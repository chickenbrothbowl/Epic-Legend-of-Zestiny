using UnityEngine;
public class Player : MonoBehaviour
{
	public BattleSide battleSide;
    public int life = 5;
    public bool isPlayer = false;
    public LifePool lifePool;

    public void DealDamage(int damage, Player target)
    {
		if (isPlayer == true)
        {
            lifePool.MoveCounter(-damage);
			Debug.Log("To " + this.name);
        }

		if (isPlayer == false)
        {
            lifePool.MoveCounter(+damage);
			Debug.Log("To " + this.name);
        }
	}
    
    
	void Update(){
		if (life <= 0){
			Debug.Log($"Player {gameObject.name} lost!");
		}
	}
}