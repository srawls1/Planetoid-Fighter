using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class MenuItem : MonoBehaviour
{
	public enum State
	{
		Idle,
		Hovered,
		Pressed
	}

	[SerializeField] private Color baseColor;
	[SerializeField] private Color hoverColor;
	[SerializeField] private Color pressedColor;

	private EventTrigger trigger;
	protected PlayerMenu parentMenu;
	private Image image;
	private State m_state;

	public State state
	{
		get { return m_state; }
		set
		{
			m_state = value;
			switch (value)
			{
				case State.Idle: image.color = baseColor; break;
				case State.Hovered: image.color = hoverColor; break;
				case State.Pressed: image.color = pressedColor; break;
			}
		}
	}

	protected void Awake()
	{
		trigger = GetComponent<EventTrigger>();
		parentMenu = GetComponentInParent<PlayerMenu>();
		image = GetComponent<Image>();
		state = State.Idle;

		EventTrigger.Entry onEnter = new EventTrigger.Entry();
		onEnter.callback.AddListener((eventData) => MouseEntered());
		onEnter.eventID = EventTriggerType.PointerEnter;
		trigger.triggers.Add(onEnter);

		EventTrigger.Entry onClick = new EventTrigger.Entry();
		onClick.callback.AddListener((eventData) => MouseClicked());
		onClick.eventID = EventTriggerType.PointerClick;
		trigger.triggers.Add(onClick);
	}

	protected void MouseEntered()
	{
		if (parentMenu.playerData.number == 0)
		{
			parentMenu.SelectChild(this);
		}
	}

	protected void MouseClicked()
	{
		if (parentMenu.playerData.number == 0)
		{
			Select();
		}
	}

	public abstract void Select();
	public abstract void RefreshDisplay(PlayerData data);
}
