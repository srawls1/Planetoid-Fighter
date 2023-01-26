using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterButton : MenuItem
{
	[SerializeField] private char letter;

	private NameMenu nameMenu;

	new protected void Awake()
	{
		base.Awake();
		nameMenu = GetComponentInParent<NameMenu>();
	}

	public override void RefreshDisplay(PlayerData data)
	{
	}

	public override void Select()
	{
		if (nameMenu.playerName.Length == 0 || nameMenu.playerName.EndsWith(' '))
		{
			nameMenu.playerName += char.ToUpper(letter);
		}
		else
		{
			nameMenu.playerName += char.ToLower(letter);
		}
	}

	[ContextMenu("Update Letter")]
	public void UpdateLetter()
	{
		TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
		text.text = char.ToUpper(letter).ToString();
	}
}
