using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ControlRemappingMenu : MonoBehaviour
{
	#region Private Fields

	private ButtonRemapMenuItem[] remapMenuItems;
    private PlayerMenu parentMenu;

	#endregion // Private Fields

	#region Unity Functions

	private void Awake()
	{
		remapMenuItems = GetComponentsInChildren<ButtonRemapMenuItem>();
        parentMenu = GetComponentInParent<PlayerMenu>();
	}

	private void OnEnable()
	{
		Refresh();
	}

	#endregion // Unity Function

	#region Public Functions

	public void Refresh()
	{
		Player rewiredPlayer = parentMenu.playerData.rewiredPlayer;
		List<ControllerMap> controllerMaps = GetControllerMaps(rewiredPlayer);

		int remapMenuItemsIndex = 0;
		foreach (var action in ReInput.mapping.ActionsInCategory("Default"))
		{
			if (action.userAssignable && action.type == InputActionType.Button)
			{
				List<Pair<ControllerMap, ActionElementMap>> actionMappings = GetActionMappings(controllerMaps, action);
				for (int i = 0; i < actionMappings.Count; ++i)
				{
					remapMenuItems[remapMenuItemsIndex++].SetInfo(rewiredPlayer, actionMappings[i].First,
						action, actionMappings[i].Second);
				}
			}
		}

		for (; remapMenuItemsIndex < remapMenuItems.Length; ++remapMenuItemsIndex)
		{
			remapMenuItems[remapMenuItemsIndex].gameObject.SetActive(false);
		}
	}

	#endregion // Public Functions

	#region Private Functions

	private List<Pair<ControllerMap, ActionElementMap>> GetActionMappings(List<ControllerMap> controllerMaps, InputAction action)
	{
		List<Pair<ControllerMap, ActionElementMap>> actionMappings = new();
		for (int i = 0; i < controllerMaps.Count; ++i)
		{
			foreach (var actionElementMap in controllerMaps[i].ElementMapsWithAction(action.id))
			{
				if (actionElementMap.ShowInField(AxisRange.Positive))
				{
					actionMappings.Add(new Pair<ControllerMap, ActionElementMap>(controllerMaps[i], actionElementMap));
				}
			}
		}

		return actionMappings;
	}

	private List<ControllerMap> GetControllerMaps(Player player)
	{
		List<ControllerMap> maps = new List<ControllerMap>();
		foreach (Controller controller in player.controllers.Controllers)
		{
			maps.Add(player.controllers.maps.GetMap(controller, "Default", "Default"));
		}

		return maps;
	}

	#endregion // Private Functions
}
