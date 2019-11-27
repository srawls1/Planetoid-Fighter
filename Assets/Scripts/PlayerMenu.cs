using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{

	[SerializeField] private Text text;
	[SerializeField] private Image controllerImage;
	[SerializeField] private Sprite xboxController;
	[SerializeField] private Sprite keyboardIcon;

	private PlayerData m_playerData;
	private Image background;
	private MenuItem[] childItems;
	private float previousVertical;

	public PlayerData playerData
	{
		get { return m_playerData; }
		set
		{
			m_playerData = value;

			int playerNumber = value.number;
			text.text = string.Format("P{0} Joined", value);
			controllerImage.sprite = playerNumber == 0 ?
				keyboardIcon : xboxController;

			for (int i = 0; i < childItems.Length; ++i)
			{
				childItems[i].RefreshDisplay(value);
			}

			Color color = value.color;
			background.color = new Color(color.r, color.g, color.b, background.color.a);
			controllerImage.color = color;
		}
	}

	public int playerNumber
	{
		get { return playerData.number; }
	}

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
		float vertical = Input.GetAxisRaw("Vertical" + playerNumber);
		if (vertical > 0.1f && previousVertical < 0.1f)
		{
			SelectPreviousChild();
		}
		if (vertical < -0.1f && previousVertical > -0.1f)
		{
			SelectNextChild();
		}
		previousVertical = vertical;

		if (Input.GetButtonDown("Join" + playerNumber))
		{
			GetSelectedChild().Select();
		}
	}

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

	public void ToggleRealDirection()
	{
		PlayerManager.instance.ToggleRealDirection(playerNumber);
	}

	public void SetJumpButton(string buttonName)
	{
		PlayerManager.instance.SetJumpButton(playerNumber, buttonName);
	}

	public void SetMeleeButton(string buttonName)
	{
		PlayerManager.instance.SetMeleeButton(playerNumber, buttonName);
	}

	public void SetShootButton(string buttonName)
	{
		PlayerManager.instance.SetShootButton(playerNumber, buttonName);
	}

	public void CancelJoin()
	{
		PlayerManager.instance.CancelJoin(playerNumber);
	}

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
}
