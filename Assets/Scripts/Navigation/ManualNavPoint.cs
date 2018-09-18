using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualNavPoint : MonoBehaviour {

	public Vector3 target;

	// Use this for initialization
	void Start () 
	{
		if(Managers.MainManager.Instance.debugMode == false)
			this.gameObject.GetComponent<MeshRenderer>().enabled = false;

		//At Start, shoot a RayCast from the bottom of this Transform (looking for only terrain)
		RaycastHit hit;
		//int terrainLayerMask = 14;

		//If the ray hits the terrain, log this navigation point.
		if(Physics.Raycast(transform.position, -Vector3.up, out hit))
		{
			//if(hit.collider.transform.gameObject.layer == 10)
			if(hit.collider.transform.gameObject.layer == LayerMask.NameToLayer("Floor"))
				target = hit.point;

			else 
				Debug.Log("MANUAL NAV POINT ERROR AT OBJECT " + this.transform.gameObject.name);
		}		
	}
}
