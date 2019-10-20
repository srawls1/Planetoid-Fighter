using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour {
	private const string REAL_INPUT_DIRECTION = "360 Directional Input";
	private const string LEFT_REIGHT_DIRECTION = "Left/Right";
	private const string CONTROLLER_CANCEL_INSTRUCTION = "B: exit";
	private const string KEYBOARD_CANCEL_INSTRUCTION = "esc: exit";

	private float previousHorizontal;
	private int m_playerNumber;
	private Color m_color;
	private bool m_realDirectionInput;

	[SerializeField] private Text text;
	[SerializeField] private Text directionChoiceText;
	[SerializeField] private Text cancelInstructionText;

	public int playerNumber
	{
		get { return m_playerNumber; }
		set
		{
			m_playerNumber = value;
			text.text = string.Format("P{0} Joined", value);
			cancelInstructionText.text = value == 0 ?
				KEYBOARD_CANCEL_INSTRUCTION : CONTROLLER_CANCEL_INSTRUCTION;
		}
	}

	public Color color
	{
		get { return m_color; }
		set
		{
			m_color = value;
			text.color = value;
		}
	}

	public bool realDirectionInput
	{
		get { return m_realDirectionInput; }
		set
		{
			m_realDirectionInput = value;
			directionChoiceText.text = value ?
				REAL_INPUT_DIRECTION : LEFT_REIGHT_DIRECTION;
		}
	}

	void Awake()
	{
	}

	void Update()
	{
		float horizontal = Input.GetAxisRaw("Horizontal" + playerNumber);
		if (horizontal > 0.1f && previousHorizontal < 0.1f)
		{
			PlayerManager.instance.ToggleRealDirection(playerNumber);
		}
		if (horizontal < -0.1f && previousHorizontal > -0.1f)
		{
			PlayerManager.instance.ToggleRealDirection(playerNumber);
		}

		previousHorizontal = horizontal;

		if (Input.GetButtonDown("Cancel" + playerNumber))
		{
			PlayerManager.instance.CancelJoin(playerNumber);
		}
	}
}
