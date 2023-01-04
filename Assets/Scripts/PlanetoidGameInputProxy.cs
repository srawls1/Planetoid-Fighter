using UnityEngine;

public class PlanetoidGameInputProxy : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private string attackAxisName = "Attack";
    [SerializeField] private string shootAxisName = "Shoot";
	[SerializeField] private float inputBufferTime = 0.1f;

	#endregion // Editor Fields

	#region Private Fields

	private bool attackPressed;
    private bool shootPressed;
    private float attackBufferTimeDelta;
    private float shootBufferTimeDelta;

	#endregion // Private Fields

	#region Unity Functions

	private void Update()
	{
        UpdateButtonValue(attackAxisName, ref attackPressed, ref attackBufferTimeDelta);
		UpdateButtonValue(shootAxisName, ref shootPressed, ref shootBufferTimeDelta);
	}

	#endregion // Unity Functions

	#region Public Functions

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
			pressed = Input.GetButtonDown(axisName);
		}
	}

	#endregion // Private Functions
}
