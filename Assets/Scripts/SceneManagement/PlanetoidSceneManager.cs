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

	#endregion // Editor Fields

	#region Unity Functions

	private IEnumerator Start()
	{
		// We only want to reveal the screen on startup, not hide it first
		yield return SceneLoader.instance.TransitionScenes(false, startMenu);
		yield return transitioner.RevealScreen();
	}

	#endregion // Unity Functions

	#region Public Functions

	public Coroutine LoadStartMenu()
	{
		return SceneLoader.instance.TransitionScenes(true, startMenu);
	}

	public Coroutine LoadMap(SceneReference mapScene)
	{
		return StartCoroutine(LoadMapImpl(mapScene));
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator LoadMapImpl(SceneReference mapScene)
	{
		yield return SceneLoader.instance.TransitionScenes(true, hudScene, gameModeScene, mapScene);
		PlayerManager.instance.StartBattle();
	}

	#endregion // Private Functions
}
