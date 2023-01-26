using UnityEngine;
using TMPro;

public class LetterButton : MenuItem
{
	#region Editor Fields

	[SerializeField] private char letter;

	#endregion // Editor Fields

	#region Private Fields

	private NameMenu nameMenu;

	#endregion // Private Fields

	#region Unity Functions

	new protected void Awake()
	{
		base.Awake();
		nameMenu = GetComponentInParent<NameMenu>();
	}

	#endregion // Unity Functions

	#region Public Functions

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

	#endregion // Public Functions
}
