using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	public int thisCoinValue = 6;
	
	private int randomCoinRotationSpeed;

	void Start()
	{
		//Determine this coin's random rotation speed with a random range, here between 10 and 50
		randomCoinRotationSpeed = Random.Range(10, 51);
	}

	// Update is called once per frame
	void Update () 
	{
		RotateCoin();
	}

	//Giving our coins a perceptible and varied rotation makes them a bit less boring inside the scene.
	void RotateCoin()
	{
		this.transform.Rotate (0, randomCoinRotationSpeed * Time.deltaTime, 0);
	}

	//Coins that intersect with the player will communicate with the PlayerManager singleton 
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			Managers.PlayerManager.Instance.CoinCollected(thisCoinValue);
			Destroy(this.gameObject);
		}
	}
}
