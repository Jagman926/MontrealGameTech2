using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Improved : MonoBehaviour {

	private GameObject thePlayer;

	public float turretRotationSpeed = 0;

	public GameObject launcher;

	public GameObject prefab_bullet;

	public float bulletSpeed = 25;
	public float bulletDelay = 2.5f;

	public bool canShoot = false;

	public bool isShooting = false;

	// Use this for initialization
	void Start () 
	{
		thePlayer = Managers.PlayerManager.Instance.GetPlayer();
	}

	void Update()
	{
		RotateTowardsPlayer();
		ChangeColor();

		if(isShooting == false)
			StartCoroutine(ShootAtPlayer());

	}
	
	private void RotateTowardsPlayer()
	{
		if(canShoot)
		{
			Vector3 targetDirection = thePlayer.transform.position - this.transform.position;

			//Vector3.RotateTowards can be used to progressively rotate an object's face towards another GameObject
			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turretRotationSpeed * Time.deltaTime, 100f);

			//Move our position a step closer to the target.
			transform.rotation = Quaternion.LookRotation(newDirection);
		}
		
		else
		{
			transform.rotation = transform.parent.rotation;
		}
	}

	private void ChangeColor()
	{
		if(canShoot == true)
			this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

		else if(canShoot == false)
			this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
	}
		
	//The "ShootAtPlayer" Coroutine is here launched once on Start()
	private IEnumerator ShootAtPlayer()
	{
		if(canShoot)
		{
			isShooting = true;

			//Spawn (Instantiate) a bullet GameObject from a saved Prefab, at the position of the turret's launcher (empty object in front of the barrel, to avoid collision with turret)
			GameObject bullet = GameObject.Instantiate (prefab_bullet, launcher.transform.position, launcher.transform.rotation);
				
			//Give it velocity
			bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

			//Then wait for a given period (here, the delay we set between shots)
			yield return new WaitForSeconds(bulletDelay);

			isShooting = false;
		}
	}
}
