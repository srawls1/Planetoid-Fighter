using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
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
		parentMenu = GetComponentInParent<LevelMenu>();
		image = GetComponent<Image>();
		state = State.Idle;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		parentMenu.SelectChild(x, y);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		SelectLevel();
	}

	public virtual void SelectLevel()
	{
		parentMenu.LoadScene(scene);
	}
}
