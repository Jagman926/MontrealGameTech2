using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour 
{

	private NavMeshAgent navAgent;

	public List<ManualNavPoint> patrolPoints;

	public List<Vector3> moveDestinations = new List<Vector3>();

	public enum MoveType {Walk, Run}

	public float walkSpeed = 5;
	public float runSpeed = 10;

	public bool isMoving = false;

	// Use this for initialization
	void Start () 
	{
		navAgent = GetNavAgent();
		//StartCoroutine(MoveCharacter_Patrol(MoveType.Walk, patrolPoints));
	}
	
	// Update is called once per frame
	void Update () {
		//MoveOnMouseClick();
	}

	/*
	private void MoveOnMouseClick()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit))
			{
					navAgent.SetDestination(hit.point);
			}
		}
	}
	 */

	//////////////////////////////////////////////////////////////////////////////////////////

	public IEnumerator MoveCharacter_Patrol(MoveType movetype, List<ManualNavPoint> manualNavPoints)
	{
		//Character will be moving
		isMoving = true;

		//Create list of positions from our ManualNavPoint targets
		List<Vector3> patrolDestinations = new List<Vector3>();
		
		foreach (ManualNavPoint point in manualNavPoints)
		{
			patrolDestinations.Add (point.target);
		}
		
		yield return StartCoroutine(CreateMoveOrder(movetype, patrolDestinations));
	}

	//Moving the character (When input is a single Waypoint)
	public IEnumerator MoveCharacter(MoveType moveType, Vector3 singleWaypoint)
	{
		//Character will be moving
		isMoving = true;

		//Create a list of points, then add the single point to it
		List<Vector3> waypoints = new List<Vector3>();
		waypoints.Add(singleWaypoint);

		//Move on to creating Move Order
		yield return StartCoroutine(CreateMoveOrder(moveType, waypoints));
	}

	//Moving the Character (When input is a series of waypoints)
	public IEnumerator MoveCharacter(MoveType moveType, List<Vector3> multipleWayPoints)
	{
		//Character will be moving
		isMoving = true;

		//Move on to creating Move Order
		yield return StartCoroutine(CreateMoveOrder(moveType, multipleWayPoints));
	}

	//////////////////////////////////////////////////////////////////////////////////////////

	private IEnumerator CreateMoveOrder(MoveType desiredMoveType, List<Vector3> destinationPoints)
	{
		GetNavAgent();

		//Set the agent's Maximum speed (depending on movetype provided by Move Order)
		navAgent.speed = DetermineMovementSpeed(desiredMoveType);

		//Make sure to disable "Auto breaking", so that the deer does not slow-down at each waypoint (if there are more than one provided)
		navAgent.autoBraking = false;

		//For each Waypoint in the list
		for (int i = 0 ; i < destinationPoints.Count ; i++)
		{
			//Send a move order to the waypoint, and wait for the character to reach a certain distance
			yield return StartCoroutine(MoveToNextWayPoint(destinationPoints[i]));
		}

		//Then clear the path
		navAgent.ResetPath();

		//Move order is complete - Set Moving to false
		isMoving = false;

		yield return null;		
	}

	private IEnumerator MoveToNextWayPoint(Vector3 nextDesination)
	{
		//Set destination to current WayPoint
		navAgent.SetDestination(nextDesination);

		//While the Agent is still calculating path, wait
		while(navAgent.pathPending)
			yield return null;

		//Once the character has a valid path to the waypoint, and is not within a certain distance of the waypoint
		while(navAgent.remainingDistance > 0.25f)
			yield return null;
	}

	private float DetermineMovementSpeed(MoveType desiredMoveType)
	{
		float newSpeed = 0;

		if(desiredMoveType == MoveType.Walk)
			newSpeed = walkSpeed;
		
		if(desiredMoveType == MoveType.Run)
			newSpeed = runSpeed;

		return newSpeed;
	}

	private NavMeshAgent GetNavAgent()
	{
		if(navAgent == null)
			navAgent = this.gameObject.GetComponent<NavMeshAgent>();

		return navAgent;
	}

	public void StopAllMovements()
	{
		this.StopAllCoroutines();
		navAgent.ResetPath();
		isMoving = false;
	}
}
