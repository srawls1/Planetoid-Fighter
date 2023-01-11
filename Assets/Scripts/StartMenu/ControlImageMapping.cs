using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct InputMapping
{
	public string buttonName;
	public Sprite buttonSprite;
}

[CreateAssetMenu(fileName = "new control image mapping.asset", menuName = "custom/control image mapping")]
public class ControlImageMapping : ScriptableObject
{
	[SerializeField] private InputMapping[] buttonImageList;

	private Dictionary<string, Sprite> buttonImageMap;

	private void Awake()
	{
		buttonImageMap = new Dictionary<string, Sprite>();
		for (int i = 0; i < buttonImageList.Length; ++i)
		{
			buttonImageMap.Add(buttonImageList[i].buttonName, buttonImageList[i].buttonSprite);
		}
	}

	public Sprite GetButtonSprite(string buttonName)
	{
		if (buttonImageMap == null)
		{
			Awake();
		}

		if (IsJoystickController(buttonName))
		{
			// Hacky way to remove the player number from the joystick string
			return buttonImageMap["joystick " + IsolateJoystickButtonName(buttonName)];
		}
		else
		{
			return buttonImageMap[buttonName];
		}
	}

	public List<string> ListButtonNames(int playerNumber)
	{
		if (buttonImageMap == null)
		{
			Awake();
		}

		bool isJoystick = playerNumber != 0;
		List<string> buttons = new List<string>();
		
		if (isJoystick)
		{
			foreach (KeyValuePair<string, Sprite> pair in buttonImageMap)
			{
				if (IsJoystickController(pair.Key))
				{
					buttons.Add(ReconstructJoystickButtonName(IsolateJoystickButtonName(pair.Key), playerNumber));
				}
			}
		}
		else
		{
			foreach (KeyValuePair<string, Sprite> pair in buttonImageMap)
			{
				if (!IsJoystickController(pair.Key))
				{
					buttons.Add(pair.Key);
				}
			}
		}

		return buttons;
	}

	private bool IsJoystickController(string buttonName)
	{
		return buttonName.StartsWith("joystick", StringComparison.CurrentCultureIgnoreCase);
	}

	private string IsolateJoystickButtonName(string buttonName)
	{
		int index = buttonName.IndexOf("button", StringComparison.CurrentCultureIgnoreCase);
		if (index < 0)
		{
			return buttonName;
		}

		return buttonName.Substring(index);
	}

	private string ReconstructJoystickButtonName(string isolatedName, int playerNumber)
	{
		return string.Format("joystick {0} {1}", playerNumber, isolatedName);
	}
}
