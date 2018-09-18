using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour 
{
	private GameObject thePlayer;

	public float minActivationDistance = 10;
	public float turretRotationSpeed = 0;

	public bool turretIsInRange;
	public GameObject launcher;

	public GameObject prefab_bullet;

	public float bulletSpeed = 25;
	public float bulletDelay = 2.5f;

	public bool canShoot = false;

	// Use this for initialization
	void Start () 
	{
		thePlayer = Managers.PlayerManager.Instance.GetPlayer();
		StartCoroutine(ShootAtPlayer());
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Every frame, check if player is in turret's range
		//Here the argument (distance) is not already defined. A method can be passed a argument, as long as it returns the correct type (here, a float) we're looking for.
		CheckIfInRange(CalculateDistanceFromPlayer());
	}

	private float CalculateDistanceFromPlayer()
	{
		//Vector3.distance allows for quick distance calculations (float value) between 2 Transforms.
		float distanceToPlayer = Vector3.Distance(this.transform.position, thePlayer.transform.position);
		return distanceToPlayer;
	}

	private void CheckIfInRange(float currentDistanceToPlayer)
	{
		if(currentDistanceToPlayer >= minActivationDistance)
		{
			//The color of an object's material can easily be modified through code
			this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
			turretIsInRange = false;
		}
		
		else
		{
			this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
			turretIsInRange = true;
			RotateTowardsPlayer();
		}
	}

	private void RotateTowardsPlayer()
	{
		Vector3 targetDirection = thePlayer.transform.position - this.transform.position;

		//Vector3.RotateTowards can be used to progressively rotate an object's face towards another GameObject
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turretRotationSpeed * Time.deltaTime, 100f);

        //Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDirection);
	}


	/*
		COROUTINES AND PERFORMANCE:

		If you launch a coroutine via StartCoroutine(), you implicitly allocate both an instance of Unity's Coroutine class (21 Bytes) and an Enumerator (16 Bytes).
		Importantly, no allocation occurs when the coroutine yield's or resumes, so all you have to do to avoid a memory leak is to limit calls to StartCoroutine() while the game is running.
	 */

	//The "ShootAtPlayer" Coroutine is here launched once on Start()
	private IEnumerator ShootAtPlayer()
	{
		//It then loops until the GameObject on which it is attached is deactivated, or remove from Hierarchy
		while(this.gameObject.activeInHierarchy == true)
		{
			//If conditions are met... (here, the turret is in range to player)
			if(turretIsInRange)
			{
				//Spawn (Instantiate) a bullet GameObject from a saved Prefab, at the position of the turret's launcher (empty object in front of the barrel, to avoid collision with turret)
				GameObject bullet = GameObject.Instantiate (prefab_bullet, launcher.transform.position, launcher.transform.rotation);
				
				//Give it velocity
				bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

				//Then wait for a given period (here, the delay we set between shots)
				yield return new WaitForSeconds(bulletDelay);
			}		

			//If the turret is not in range, just return "null" and restart from top
			yield return null;
		}	

		//If the coroutine ever hits this line, "Break" will stop it, so it would have to be restarted again with StartCoroutine()
		//In this scenario, this will never happen, as destroying it's parent GameObject would stop it automatically.
		yield break;
	}
}
