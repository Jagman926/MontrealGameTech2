using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{
	//Use HEADERS to neatly organize public variables in the Unity's Inspector Window
	[Header("Player Movement Modifiers")]

	public float currentMoveSpeed = 0;
    public float maxMoveSpeed = 0;
	public float currentXRotationSpeed = 0;
	public float currentYRotationSpeed = 0;
    private float yawCam = 0.0f;

	[Header("Player Gravity Modifiers")]

	public float jumpForce;
	public float fallForce;

    [Header("Player Sprint Modifiers")]

    public float sprintSpeed = 0;
    public float maxPlayerStamina = 100;
    public float currPlayerStamina = 100;
    public float staminaDepletionRate = 1.0f;
    public float staminaRechargeRate = 1.0f;

    [Header("Player SoundRadius")]

    public float soundRadius = 0;
    public float soundTravelDistance = 10;

	[Header("Light")]

	public bool isLit;
	private Light playerLight;

    [Header("Is Player Grounded")]

	public bool isGrounded;
	public float groundCheckDistance;
	private int groundLayerMask = 1 << 12;

	//////////////////////////////////////

	private Rigidbody thisRigidBody;

	void Start()
	{
		thisRigidBody = this.gameObject.GetComponent<Rigidbody>();
		//Hide the cursor on screen
		Cursor.visible = false;
		//Player light
		playerLight = GameObject.Find("FlashLightLight").GetComponent<Light>();

	}
	
	//Every frame....(Best used for anything not physics related)
	void Update()
	{
        UpdateSprint();
		UpdateGrounding();
		UpdatePlayerLight();
		BasicPlayerRotate();
        UpdateSoundRadius();
	}

	//Every FIXED frame....(Best used for everything physics related)
	void FixedUpdate()
	{
        BasicPlayerMove();
        BasicPlayerJump();
		BasicPlayerFall();
	}

	//On Update(), check if Player is using Input
	private void BasicPlayerMove()
	{
		//Get a value (between -1 and 1, on either controller joystick OR WASD)
		float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");

        //Add force to player while buttons are pressed
        if ((xMove != 0) || (yMove != 0))
            thisRigidBody.AddForce((((transform.forward * yMove) + (transform.right * xMove)) * currentMoveSpeed), ForceMode.Acceleration);
    }

	private void UpdateGrounding()
	{
		RaycastHit hit;
        //Check if raycast hits floor layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, groundCheckDistance, groundLayerMask))
        {
			isGrounded = true;
        }
	}

    private void UpdateSprint()
    {
        //While sprint key held down
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //If there is enough stamina
            if (currPlayerStamina > 0)
            {
                //Deplete stamina
                currPlayerStamina -= staminaDepletionRate;
                //Set movement speed to sprint speed
                currentMoveSpeed = sprintSpeed;
            }
			else
			{
				currentMoveSpeed = maxMoveSpeed;
				Debug.Log("Out of Stamina");
			}
        }
        //If not pressing button
        else
        {
            //Recharge stamina and set move speed
            RechargeStamina();
        }

        //Update stamina UI
        Managers.UiManager.Instance.UpdateStaminaUi(currPlayerStamina, maxPlayerStamina);
    }

    private void RechargeStamina()
    {
        //Recharge stamina while less than max stamina
        if(currPlayerStamina < maxPlayerStamina)
            currPlayerStamina += staminaRechargeRate;
        //Set movement speed to normal speed
        currentMoveSpeed = maxMoveSpeed;
    }

    private void UpdateSoundRadius()
    {
        soundRadius = thisRigidBody.velocity.magnitude;
    }

	private void UpdatePlayerLight()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			isLit = !isLit;
		}
		if(isLit)
		{
			playerLight.intensity = 20;
		}
		else
		{
			playerLight.intensity = 0;
		}
	}

	//On Update(), check if Player is rotating (using Input)
	private void BasicPlayerRotate()
	{
		float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * currentXRotationSpeed;

		if(mouseX != 0)
		{
			 //Rotate the Player accordingly
			transform.Rotate(0, mouseX, 0);
		}
		//Rotate player camera
		yawCam -= currentYRotationSpeed * Input.GetAxis("Mouse Y");
		//Clamp between 90 degree angles (gives 180 view vertically)
		yawCam = Mathf.Clamp(yawCam, -90.0f, 90.0f);
		//Transform camera angles
		Transform cameraTransform = transform.GetChild(0).transform;
        cameraTransform.eulerAngles = new Vector3(yawCam, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
	}

	//On Fixed Update, check if player can jump, and is jumping
	private void BasicPlayerJump()
	{
		if(isGrounded)
		{
			if(Input.GetButton("Jump"))
			{
				Vector3 jump = new Vector3(0, jumpForce, 0);
				
				// Here, we use a physics transformation by adding a force to our player
				// The type of force applied can be modified using the "ForceMode" argument 
				thisRigidBody.AddForce(jump, ForceMode.Impulse);
				//Set grounded to false
				isGrounded = false;
			}
		}
	}

	//Adding feedback to the fall is just one of the many ways to easily improve the gamefeel of our prototype.
	private void BasicPlayerFall()
	{
		if(!isGrounded)
		{
			//You can measure and monitor a Rigidbody's current velocity (positive or negative value) on any axis
			//Debug.Log(thisRigidBody.velocity.y);

			if(thisRigidBody.velocity.y < 0)
			{
				Vector3 fall = new Vector3(0, fallForce, 0);
				thisRigidBody.AddForce(fall, ForceMode.Acceleration);
			}
		}
	}
}
