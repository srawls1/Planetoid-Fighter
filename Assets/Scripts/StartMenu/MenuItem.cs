using UnityEngine;
using UnityEngine.UI;

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
		parentMenu = GetComponentInParent<PlayerMenu>();
		image = GetComponent<Image>();
		state = State.Idle;
	}

	public abstract void Select();
	public abstract void RefreshDisplay(PlayerData data);
}
