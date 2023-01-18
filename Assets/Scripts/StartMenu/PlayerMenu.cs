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
	private MenuItem[] childItems;
	private float previousVertical;

	#endregion // Private Fields

	#region Properties

	public PlayerData playerData
	{
		get { return m_playerData; }
		set
		{
			m_playerData = value;

			text.text = value.name;
			controllerImage.sprite = value.rewiredPlayer.controllers.hasKeyboard ? keyboardIcon : controllerIcon;

			for (int i = 0; i < childItems.Length; ++i)
			{
				childItems[i].RefreshDisplay(value);
			}

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
		childItems = GetComponentsInChildren<MenuItem>();
	}

	private void Start()
	{
		SelectChild(childItems[0]);
	}

	private void OnDisable()
	{
		SelectChild(childItems[0]);
	}

	void Update()
	{
		float vertical = playerData.rewiredPlayer.GetAxisRaw("Vertical");
		if (vertical > 0.1f && previousVertical < 0.1f)
		{
			SelectPreviousChild();
		}
		if (vertical < -0.1f && previousVertical > -0.1f)
		{
			SelectNextChild();
		}
		previousVertical = vertical;

		if (playerData.rewiredPlayer.GetButtonDown("Confirm"))
		{
			GetSelectedChild().Select();
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public void SelectChild(MenuItem child)
	{
		MenuItem currentSelected = GetSelectedChild();
		if (currentSelected != null)
		{
			if (currentSelected.state == MenuItem.State.Pressed)
			{
				return;
			}

			currentSelected.state = MenuItem.State.Idle;
		}
		
		child.state = MenuItem.State.Hovered;
	}

	public void CancelJoin()
	{
		PlayerManager.instance.CancelJoin(playerData.number);
	}

	#endregion // Public Functions

	#region Private Functions

	private void SelectNextChild()
	{
		int index = GetSelectedChildIndex();
		++index;
		index %= childItems.Length;
		SelectChild(childItems[index]);
	}

	private void SelectPreviousChild()
	{
		int index = GetSelectedChildIndex();
		--index;
		if (index < 0) index += childItems.Length;
		SelectChild(childItems[index]);
	}

	private MenuItem GetSelectedChild()
	{
		int index = GetSelectedChildIndex();
		if (index < 0)
		{
			return null;
		}
		return childItems[GetSelectedChildIndex()];
	}

	private int GetSelectedChildIndex()
	{
		for (int i = 0; i < childItems.Length; ++i)
		{
			if (childItems[i].state == MenuItem.State.Hovered ||
				childItems[i].state == MenuItem.State.Pressed)
			{
				return i;
			}
		}

		return -1;
	}

	#endregion // Private Functions
}
