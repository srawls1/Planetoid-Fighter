
public class BackspaceButton : MenuItem
{
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
		if (nameMenu.playerName.Length > 0)
		{
			nameMenu.playerName = nameMenu.playerName[0..^1];
		}
	}
}
