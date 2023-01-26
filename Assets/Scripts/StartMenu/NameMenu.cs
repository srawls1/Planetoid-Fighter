using UnityEngine;
using TMPro;

public class NameMenu : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private TextMeshProUGUI nameText;

	#endregion // Editor Fields

	#region Private Fields

	private PlayerMenu parentMenu;

	#endregion // Private Fields

	#region Properties

	private string m_playerName;
    public string playerName
	{
		get { return m_playerName; }
		set
		{
			m_playerName = value;
			nameText.text = m_playerName;
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Awake()
	{
		parentMenu = GetComponentInParent<PlayerMenu>();
	}

	private void OnEnable()
	{
		playerName = parentMenu.playerData.name;
	}

	#endregion // Unity Functions

	#region Public Functions

	public void ConfirmName()
	{
		parentMenu.SetName(playerName);
	}

	#endregion // Public Functions
}
