using System.Collections;
using UnityEngine;

public class WipeoverScreenTransitioner : ScreenTransitioner
{
	#region Editor Fields

	[SerializeField] private GameObject wiper;
	[SerializeField] private Transform preWipePosition;
	[SerializeField] private Transform fullyWipedPosition;
	[SerializeField] private Transform postWipePosition;
	[SerializeField] private float wipeTimeSeconds;

	#endregion // Editor Fields

	#region ScreenTransitioner Implementation

	public override Coroutine HideScreen()
	{
		return StartCoroutine(Wipe(preWipePosition.position, fullyWipedPosition.position));
	}

	public override Coroutine RevealScreen()
	{
		return StartCoroutine(Wipe(fullyWipedPosition.position, postWipePosition.position));
	}

	#endregion // ScreenTransitioner Implementation

	#region Private Functions

	private IEnumerator Wipe(Vector2 startPosition, Vector2 endPosition)
	{
		wiper.transform.position = startPosition;

		for (float dt = 0f; dt < wipeTimeSeconds; dt += Time.unscaledDeltaTime)
		{
			wiper.transform.position = Vector2.Lerp(startPosition, endPosition, dt / wipeTimeSeconds);
			yield return null;
		}

		wiper.transform.position = endPosition;
	}

	#endregion // Private Functions
}
