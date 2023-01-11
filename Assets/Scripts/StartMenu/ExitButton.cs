using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MenuItem
{
	public override void RefreshDisplay(PlayerData data)
	{
		
	}

	public override void Select()
	{
		StartCoroutine(SelectRoutine());
	}

	private IEnumerator SelectRoutine()
	{
		yield return null;
		parentMenu.CancelJoin();
	}
}
