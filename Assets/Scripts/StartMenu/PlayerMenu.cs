using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMenu : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private Image controllerImage;
	[SerializeField] private Sprite controllerIcon;
	[SerializeField] private Sprite keyboardIcon;

	#endregion // Editor Fields

	#region Private Fields

	private PlayerData m_playerData;
	private Image background;

	#endregion // Private Fields

	#region Properties

	public PlayerData playerData
	{
		get { return m_playerData; }
		set
		{
			m_playerData = value;

			text.text = value.name;
			controllerImage.sprite = value.rewiredPlayer.controllers.hasKeyboard ?
				keyboardIcon : controllerIcon;

			Color color = value.color;
			background.color = new Color(color.r, color.g, color.b, background.color.a);
			controllerImage.color = color;
		}
	}

	#endregion // Properties

	#region Unity Functions

	void Awake()
	{
		background = GetComponent<Image>();
	}

	#endregion // Unity Functions

	#region Public Functions

	public void SetColor(Color color)
	{
		playerData.color = color;
		playerData = playerData;
	}

	public void SetName(string name)
	{
		playerData.name = name;
		playerData = playerData;
	}

	public void CancelJoin()
	{
		PlayerManager.instance.CancelJoin(playerData.number);
	}

	#endregion // Public Functions
}
