using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _REF_ExampleScript : MonoBehaviour 
{
	public int lifePoints = 100;

	private float walkspeed = 3f;
	private float RunSpeed = 6.27f;

	public bool isRunning = false;

	private CharacterController playerController;

	///////////////////////////////////////////////////////////////////////////////////////////////

	//AWAKE is called ONCE when a game object is activated in a loaded Scene in Unity (on frame 0), (OR on the first frame when this class is created, BEFORE Start()
	void Awake(){}

	// START is called ONCE when a Scene is loaded in Unity (OR on the first frame when this class is instantiated, AFTER Awake())
	void Start ()
	{
		playerController = this.gameObject.GetComponent<CharacterController>();
	}
	
	// UPDATE is called every frame (so it's speed varies according to FPS)
	void Update (){}

	//FIXED UPDATE is called every fixed framerate frame - Should be used instead of Update() for physics (and RigidBody) calculations
	void FixedUpdate()
	{
		PlayerMove();
		
	}

	///////////////////////////////////////////////////////////////////////////////////////////////

	// PUBLIC METHOD could be called (executed) from another Class (Script)
	// In order to do so, PublicMethod's Parent Class ALSO needs to be public
	public void PublicMethod(){}

	//PRIVATE METHOD cannot be called from other scripts (classes), only from it's parent (or child) Classes, and methods within these classes.
	private void PrivateMethod(){}

	//This method is VOID, meaning it will not return any information, it will only perform its tasks
	// This method takes an ARGUMENT, which means we have to input data into it when we call it, or it will give and error
	private void PlayerMove()
	{
		// A VARIALBLE (here, the float 'topSpeed') INSIDE A METHOD ('PlayerMove') is always PRIVATE to that method.
		// Other Methods inside this Class cannot access it
		// Other classes cannot access it either.
		float topSpeed = 0;

		//Check IF the Class Variable 'isRunning
		if(isRunning == true)
		{
			topSpeed = RunSpeed;
		}

		else if (isRunning == false)
		{
			topSpeed = walkspeed;
		}

		// Alternative way of writing it (if there is ONLY ONE consequence to the answer)
		
		/* 
		if(isRunning)
			topSpeed = RunSpeed;

		else
			topSpeed = walkspeed;
		*/
			
		//Get current value from Axis Inputs (Keyboard or Controller)
		float verticalMovement = Input.GetAxis("Vertical");
		float horizontalMovement = Input.GetAxis("Horizontal");

		Vector3 verticalDirection = transform.forward * verticalMovement * topSpeed;
		Vector3 horizontalDirection = transform.right * horizontalMovement * topSpeed;
		
		Vector3 combinedDirection = verticalDirection + horizontalDirection;

		playerController.SimpleMove(combinedDirection);
	}

}
