using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ControlRemappingMenu : MonoBehaviour
{
	

    private PlayerMenu parentMenu;

	private void Awake()
	{
        parentMenu = GetComponentInParent<PlayerMenu>();
	}

	private void OnEnable()
	{
		Refresh();
	}

	private void Refresh()
	{
		var rewiredPlayer = parentMenu.playerData.rewiredPlayer;


		foreach (var action in ReInput.mapping.ActionsInCategory("Default"))
		{
			if (action.type == InputActionType.Button)
			{

			}
		}
	}
}
