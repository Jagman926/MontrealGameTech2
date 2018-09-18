﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
	public class UiManager : Singleton <UiManager>
	{
		private Text textField_Coins;
        private Image healthBar_Player;
        private Image staminaBar_Player;

		void Start()
		{
			textField_Coins = GameObject.Find("TextField_Coins").GetComponent<Text>();
            healthBar_Player = GameObject.Find("HealthBar_Fill_Player").GetComponent<Image>();
            staminaBar_Player = GameObject.Find("StaminaBar_Fill_Player").GetComponent<Image>();
		}

		//When the PlayerManager increases the player's coin inventory, it will call this method
		//Notice how this UI element (text field) is NOT updated every frame through Update(), as it only needs to change when the coin value is modified.
		public void UpdateCoinUi(int currentCoins)
		{ 
			//The currentCoins variable (INT) needs to be converted to string in order to display it as text in a UI Text Field.
			textField_Coins.text = ("Coins: " + currentCoins.ToString());
		}

        //When the player takes damage, it will call this message
        //Since fill works on a basis of 0-1, it will adjust the health to fit that constraint
        public void UpdateHealthUi(float playerHealth, float playerHealthMax)
        {
            healthBar_Player.fillAmount = (playerHealth / playerHealthMax);
        }

        public void UpdateStaminaUi(float playerStamina, float playerStaminaMax)
        {
            staminaBar_Player.fillAmount = (playerStamina / playerStaminaMax);
        }
	}
}
