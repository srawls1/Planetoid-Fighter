using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMenuButton : MenuItem
{
	[SerializeField] private UnityEvent onClickEvent;

	public override void RefreshDisplay(PlayerData data)
	{
		
	}

	public override void Select()
	{
		onClickEvent.Invoke();
	}
}
