using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

	public float viewRadius;
    public float soundRadius;
	[Range(0, 360)] public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;
	public float fovRefreshDelay = 0;

	public List<Transform> visibleTargets = new List<Transform>();
    public List<Transform> hearableTargets = new List<Transform>();

    void Start()
	{
		StartCoroutine(FindTargetsWithDelay(fovRefreshDelay));
	}
	
	//Method that takes in an angle and returns a direction to that angle
	public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
	{

		//If the angle is NOT global (meaning LocalRotation from our character is taken into account)
		if(!angleIsGlobal)
			//Add the character's LocalRotation angle into the AngleInDegrees Calculation
			angleInDegrees += transform.eulerAngles.y;

		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	void FindVisibleTargets()
	{
		//Start by clearing any old list of targets
		visibleTargets.Clear();

		//Find all targets in view Radius
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        //For each of them...
        for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			//Identify its Transform
			Transform target = targetsInViewRadius[i].transform;

			//Check if the Transform's current direction (from self) is inside the View Angle
			Vector3 directionToTarget = (target.position - transform.position).normalized;
			if(Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
			{	
				//If so, calculate distance to target
				float distanceToTarget = Vector3.Distance(transform.position, target.position);
				//Then send a RayCast that checks to see if it first collides with any vision blocking obstacle
				if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
				{
					//If not and raycast collides with target, add it to the list of visible targets
					visibleTargets.Add(target);
				}
			}
		}
	}

    void FindHearableTargets()
    {
        //Start by clearing any old list of targets
        hearableTargets.Clear();

        //Find all target in sound Radius
        Collider[] targetsInSoundRadius = Physics.OverlapSphere(transform.position, soundRadius, targetMask);

        //For each of them...
        for (int i = 0; i < targetsInSoundRadius.Length; i++)
        {
            //Identify its Transform
            Transform target = targetsInSoundRadius[i].transform;
            //If so, calculate distance to target
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            //If the AI is in range of the player's sound
            if (distanceToTarget <= (target.GetComponent<PlayerMovement>().soundRadius))
            {
                //Then send a RayCast that checks to see if it first collides with any blocking obstacle
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    //If not and raycast collides with target, add it to the list of hearable targets
                    hearableTargets.Add(target);
                }
            }
        }
    }

	IEnumerator FindTargetsWithDelay(float delay)
	{
		while(true)
		{
			//Wait for the next FOV Refrest
			yield return new WaitForSeconds (delay);

			FindVisibleTargets();
            FindHearableTargets();
		}
	}
}
