using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Turret : MonoBehaviour {

	public int thisBulletDamage = 5;
	
	void Start()
	{
		//As soon as this object is created, tell it to automatically destroy itself after X time.
		//If the bullet falls of the environment without ever colliding with anything, this prevents the game from losing performance by having countless bullets falling indefinitely.
		Destroy(this.gameObject, 10f);
	}

	//When the bullet collides with something...
	void OnCollisionEnter(Collision col)
	{
		//If it hits the Player
		if(col.gameObject.tag == "Player")
		{
			//Apply damage (through the PLAYER MANAGER)
			Managers.PlayerManager.Instance.EnemyBulletHitPlayer(thisBulletDamage);
			//And destroy this bullet, so it doesn't just roll around the level.
			Destroy(this.gameObject);
		}

		else
		{
			//If the bullet hits something else (Ex: a wall), simply destroy it.
			Destroy(this.gameObject, 5f);
		}
	}


}
