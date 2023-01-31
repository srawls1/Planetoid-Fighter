using System.Collections;
using UnityEngine;

public class PlanetoidSceneManager : Singleton<PlanetoidSceneManager>
{
	#region Singleton Implementation

	protected override PlanetoidSceneManager GetThis()
	{
		return this;
	}

	protected override void Init()
	{
		
	}

	#endregion // Singleton Implementation

	#region Editor Fields

	[SerializeField] private SceneReference startMenu;
	[SerializeField] private SceneReference hudScene;
	[SerializeField] private SceneReference gameModeScene;
	[SerializeField] private ScreenTransitioner transitioner;
	[SerializeField] private GameObject titleInfo;
	[SerializeField] private float timeToShowTitleInfo;

	#endregion // Editor Fields

	#region Private Fields

	private bool loading;

	#endregion // Private Fields

	#region Unity Functions

	private IEnumerator Start()
	{
		float startTime = Time.time;
		// We only want to reveal the screen on startup, not hide it first
		yield return SceneLoader.instance.TransitionScenes(false, startMenu);
		float timePassed = Time.time - startTime;
		float timeToWait = timeToShowTitleInfo - timePassed;
		if (timeToWait > 0)
		{
			yield return new WaitForSecondsRealtime(timeToWait);
		}

		yield return transitioner.RevealScreen();
		titleInfo.gameObject.SetActive(false);
	}

	#endregion // Unity Functions

	#region Public Functions

	public Coroutine LoadStartMenu()
	{
		if (loading)
		{
			return null;
		}
		return SceneLoader.instance.TransitionScenes(true, startMenu);
	}

	public Coroutine LoadMap(SceneReference mapScene)
	{
		if (loading)
		{
			return null;
		}
		return StartCoroutine(LoadMapImpl(mapScene));
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator LoadMapImpl(SceneReference mapScene)
	{
		loading = true;
		yield return SceneLoader.instance.TransitionScenes(true, hudScene, gameModeScene, mapScene);
		PlayerManager.instance.StartBattle();
		loading = false;
	}

	#endregion // Private Functions
}
