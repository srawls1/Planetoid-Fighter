using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidewaysSelector : MenuItem
{
	[SerializeField] private Text textElement;
	[SerializeField] private string realDirectionText;
	[SerializeField] private string leftRightText;

	private float previousHorizontal;

	public override void RefreshDisplay(PlayerData data)
	{
		textElement.text = data.realDirectionInput ?
			realDirectionText : leftRightText;
	}

	public override void Select()
	{
		state = state == State.Hovered ? State.Pressed : State.Hovered;
		StartCoroutine(SelectRoutine());
	}

	public void ToggleDirectionInput()
	{
		parentMenu.ToggleRealDirection();
	}

	private IEnumerator SelectRoutine()
	{
		while (state == State.Pressed)
		{
			float horizontal = Input.GetAxisRaw("Horizontal" + parentMenu.playerNumber);
			if (horizontal > 0.1f && previousHorizontal < 0.1f)
			{
				ToggleDirectionInput();
			}
			if (horizontal < -0.1f && previousHorizontal > -0.1f)
			{
				ToggleDirectionInput();
			}

			previousHorizontal = horizontal;

			yield return null;
		}
	}
}
