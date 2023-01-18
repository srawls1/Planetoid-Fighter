using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PowerupCard : MonoBehaviour
{
    [SerializeField] private SkewedImage powerupImage;
    [SerializeField] private SkewedImage backgroundImage;
    [SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private Color baseColor;
	[SerializeField] private Color hoverColor;
	[SerializeField] private Color pressedColor;

	public enum State
	{
		Idle,
		Hovered,
		Pressed
	}

	private State m_state;

	private Powerup m_powerup;
    public Powerup powerup
	{
        get { return m_powerup; }
        set
		{
            m_powerup = value;
			titleText.text = powerup.name;
			powerupImage.sprite = powerup.sprite;
		}
	}

	public PlayerData player
	{
		get; set;
	}

	public State state
	{
		get { return m_state; }
		set
		{
			m_state = value;
			switch (value)
			{
				case State.Idle: backgroundImage.color = baseColor; break;
				case State.Hovered: backgroundImage.color = hoverColor; break;
				case State.Pressed: backgroundImage.color = pressedColor; break;
			}
		}
	}

	protected void Awake()
	{
		state = State.Idle;
	}

	public void Select()
	{
		player.AddPowerup(powerup);
	}
}
