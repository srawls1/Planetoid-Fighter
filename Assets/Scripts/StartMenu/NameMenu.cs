using UnityEngine;
using TMPro;

public class NameMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

	private PlayerMenu parentMenu;

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

	private void Awake()
	{
		parentMenu = GetComponentInParent<PlayerMenu>();
	}

	private void OnEnable()
	{
		playerName = parentMenu.playerData.name;
	}

	public void ConfirmName()
	{
		parentMenu.SetName(playerName);
	}
}
