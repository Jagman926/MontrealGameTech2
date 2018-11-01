using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
	public class PlayerManager : Singleton <PlayerManager>
	{
		public GameObject thePlayer;

		public int totalCollectedCoins = 0;
        public int maxPlayerHealth = 100;
		public int currPlayerHealth = 100;

		//Coins the player collects will call this method.
		public void CoinCollected(int thisCoinValue)
		{
			totalCollectedCoins += thisCoinValue;
			Managers.UiManager.Instance.UpdateCoinUi(totalCollectedCoins);
		}

		//Enemy bullets that collide with this player will call this method
		public void EnemyBulletHitPlayer(int bulletDamage)
		{
			currPlayerHealth -= bulletDamage;

			if(currPlayerHealth <= 0)
            {
                //The player has a limited health pool. 
                PlayerDeath();
            }

            //Update health UI
            Managers.UiManager.Instance.UpdateHealthUi(currPlayerHealth, maxPlayerHealth);
        }

		void PlayerDeath()
		{
            //Set health UI to 0
            Managers.UiManager.Instance.UpdateHealthUi(0, maxPlayerHealth);
            //No capsule is eternal.
            Debug.Log("The Player Has Died - Whoops.");
			//Set the Player's reference to NULL to make sure his next apparition (if he or she respawns) can be acquired and refreshed by GetPlayer()
			thePlayer = null;
		}

		//Other classes will use this common method to gather a link to the Player GameObject (instead of each class searching for the PLayer GameObject by name)
		public GameObject GetPlayer()
		{
			//This method will only search for the player by GameObject name (resource intensive, especially in larger scenes) IF it has not already been identified (or reset on PlayerDeath())
			if(thePlayer == null)
				thePlayer = GameObject.FindGameObjectWithTag("Player");

			return thePlayer;
		}		
	}
}
	



