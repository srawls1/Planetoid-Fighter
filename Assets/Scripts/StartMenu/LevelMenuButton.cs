using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelMenuButton : MonoBehaviour
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
	[SerializeField] private SceneReference scene;

	private EventTrigger trigger;
	protected LevelMenu parentMenu;
	private Image image;
	private State m_state;

	public int x;
	public int y;

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
		parentMenu = GetComponentInParent<LevelMenu>();
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
		parentMenu.SelectChild(x, y);
	}

	protected void MouseClicked()
	{
		SelectLevel();
	}

	public void SelectLevel()
	{
		parentMenu.LoadScene(scene);
	}
}
