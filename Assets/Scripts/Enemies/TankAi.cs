using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAi : MonoBehaviour 
{

	public enum AiState{None, Idle, TargetAcquired}
	public AiState currentAiState;

	public Navigator thisNavigator;
	public FieldOfView thisFieldOfView;
	public Turret_Improved thisTurret;

	void Start()
	{
		currentAiState = AiState.None;
	}

	void Update()
	{
		RefreshState();
	}
	
	private void RefreshState()
	{
	
		if(thisFieldOfView.visibleTargets.Count != 0 || thisFieldOfView.hearableTargets.Count !=0)
		{
			if(currentAiState != AiState.TargetAcquired)
			{
				this.StopAllCoroutines();
				currentAiState = AiState.TargetAcquired;
				StartCoroutine(NewBehaviour());
			}
		}

		else
		{
			if(currentAiState != AiState.Idle)
			{
				this.StopAllCoroutines();
				currentAiState = AiState.Idle;
				StartCoroutine(NewBehaviour());
			}
		}
	}	

	private IEnumerator NewBehaviour()
	{
		if(currentAiState == AiState.Idle)
		{
			thisNavigator.StopAllMovements();
			thisTurret.canShoot = false;
			yield return StartCoroutine(thisNavigator.MoveCharacter_Patrol(Navigator.MoveType.Walk, thisNavigator.patrolPoints));
			currentAiState = AiState.None;
			RefreshState();
		}

		else if (currentAiState == AiState.TargetAcquired)
		{
			thisNavigator.StopAllMovements();
			thisTurret.canShoot = true;
			yield return null;
		}

		yield return null;
	}
}
