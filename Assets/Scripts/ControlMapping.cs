using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new control mapping.asset", menuName = "custom/control mapping")]
public class ControlMapping : ScriptableObject
{
	[SerializeField] private ButtonMapping keyboardMouseMapping;
	[SerializeField] private ButtonMapping joystickMapping;

	public ButtonMapping GetMapping(int playerNumber)
	{
		if (playerNumber == 0)
		{
			return keyboardMouseMapping;
		}
		else
		{
			string jumpButton = ReconstructJoystickButton(playerNumber, joystickMapping.jumpButton);
			string meleeButton = ReconstructJoystickButton(playerNumber, joystickMapping.meleeButton);
			string shootButton = ReconstructJoystickButton(playerNumber, joystickMapping.shootButton);
			return new ButtonMapping(joystickMapping.realDirectionInput, jumpButton, meleeButton, shootButton);
		}
	}

	private string ReconstructJoystickButton(int playerNumber, string isolatedButtonName)
	{
		return string.Format("joystick {0} {1}", playerNumber, isolatedButtonName);
	}
}
