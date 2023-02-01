using System;
using System.Collections;
using UnityEngine;

public static class ExecuteAtEndOfFrameExtension
{
	private static YieldInstruction waitInstruction = new WaitForEndOfFrame();

    public static void ExecuteAtEndOfFrame(this MonoBehaviour behavior, Action action)
	{
		behavior.StartCoroutine(ExecuteAtEndOfFrameImpl(action));
	}

	private static IEnumerator ExecuteAtEndOfFrameImpl(Action action)
	{
		yield return waitInstruction;
		action();
	}
}
