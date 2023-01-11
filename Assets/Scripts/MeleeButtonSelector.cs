using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeButtonSelector : MenuItem
{
	[SerializeField] private ControlImageMapping controlImages;
	[SerializeField] private Image buttonImage;

	public override void RefreshDisplay(PlayerData data)
	{
		//buttonImage.sprite = controlImages.GetButtonSprite(data.meleeButton);
	}

	public override void Select()
	{
		state = State.Pressed;
		StartCoroutine(SelectRoutine());
	}

	private IEnumerator SelectRoutine()
	{
		List<string> buttonsToCheck = controlImages.ListButtonNames(parentMenu.playerNumber);
		yield return null;

		while (state == State.Pressed)
		{
			for (int i = 0; i < buttonsToCheck.Count; ++i)
			{
				if (Input.GetKeyDown(buttonsToCheck[i]))
				{
					parentMenu.SetMeleeButton(buttonsToCheck[i]);
					state = State.Hovered;
					break;
				}
			}

			yield return null;
		}
	}
}
