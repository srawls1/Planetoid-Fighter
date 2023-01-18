using UnityEngine;
using Rewired;

public class PlanetoidGameInputProxy : PlayerInputProxy
{
	#region Editor Fields

	[SerializeField] private string horizontalAxisName = "Horizontal";
	[SerializeField] private string verticalAxisName = "Vertical";
	[SerializeField] private string jumpAxisName = "Jump";
	[SerializeField] private string attackAxisName = "Attack";
    [SerializeField] private string shootAxisName = "Shoot";
	[SerializeField] private float inputBufferTime = 0.1f;

	#endregion // Editor Fields

	#region Private Fields

	private bool attackPressed;
    private bool shootPressed;
    private float attackBufferTimeDelta;
    private float shootBufferTimeDelta;
	private Vector2 lastCorrectedInput;

	#endregion // Private Fields

	#region Properties

	public Player rewiredPlayer { get; set; }

	#endregion // Properties

	#region Unity Functions

	new protected void Update()
	{
		if (rewiredPlayer == null)
		{
			return;
		}

		Vector2 realInput = rewiredPlayer.GetAxis2DRaw(horizontalAxisName, verticalAxisName);
		Vector3 right = Vector3.Cross(transform.up, Vector3.forward);
		lastCorrectedInput = Vector2.right * Vector2.Dot(right, realInput);

		UpdateButtonValue(jumpAxisName, ref jumpPressed, ref jumpBufferTimeDelta);
        UpdateButtonValue(attackAxisName, ref attackPressed, ref attackBufferTimeDelta);
		UpdateButtonValue(shootAxisName, ref shootPressed, ref shootBufferTimeDelta);
	}

	#endregion // Unity Functions

	#region Public Functions

	public override bool JumpHeld()
	{
		if (rewiredPlayer == null)
		{
			return false;
		}
		return rewiredPlayer.GetButton(jumpAxisName);
	}

	public override Vector2 Movement()
	{
		if (rewiredPlayer == null)
		{
			return Vector2.zero;
		}
		//return rewiredPlayer.GetAxis2DRaw(horizontalAxisName, verticalAxisName);
		return lastCorrectedInput;
	}

	public override Vector2 Look()
	{
		return Vector2.zero;
	}

	public bool Attack()
	{
		return attackPressed;
	}

	public bool Shoot()
	{
		return shootPressed;
	}

	public void ResetAttack()
	{
		attackPressed = false;
	}

	public void ResetShoot()
	{
		shootPressed = false;
	}

	#endregion // Public Functions

	#region Private Functions

	private void UpdateButtonValue(string axisName, ref bool pressed, ref float bufferTimeDelta)
	{
		if (pressed)
		{
			bufferTimeDelta -= Time.deltaTime;
			if (bufferTimeDelta <= 0f)
			{
				bufferTimeDelta = 0f;
				pressed = false;
			}
		}
		else
		{
			bufferTimeDelta = inputBufferTime;
			pressed = rewiredPlayer.GetButtonDown(axisName);
		}
	}

	#endregion // Private Functions
}
