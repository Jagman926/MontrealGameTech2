using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FlashlightScript : MonoBehaviour
{
    private Hand hand;

    [Header("Light")]
    public bool isLit;
    public float lightIntensity;
    public AudioClip lightClip;
    private Light playerLight;

    void Start()
    {
        //Player light
        playerLight = GameObject.Find("FlashLightBeam").GetComponent<Light>();
        //
        isLit = false;
    }

    void Update()
    {
        UpdateHand();
        //While the flashlight is held
        UpdatePlayerLight();
    }

    public void ToggleLight()
    {
        //Set light on and off
        isLit = !isLit;
        //Play sound
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = lightClip;
        audio.Play();
    }

    private void UpdateHand()
    {
        hand = gameObject.GetComponent<Interactable>().attachedToHand;
    }

    private void UpdatePlayerLight()
    {
        if (hand != null && SteamVR_Input._default.inActions.Teleport.GetStateDown(hand.handType))
        {
            ToggleLight();
        }
        if (isLit)
        {
            playerLight.intensity = lightIntensity;
        }
        else
        {
            playerLight.intensity = 0;
        }
    }
}
