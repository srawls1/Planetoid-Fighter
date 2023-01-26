
public class BackspaceButton : MenuItem
{
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
		if (nameMenu.playerName.Length > 0)
		{
			nameMenu.playerName = nameMenu.playerName[0..^1];
		}
	}

	#endregion // Public Functions
}
