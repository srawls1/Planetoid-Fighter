using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
	#region Singleton Implementation

	protected override SceneLoader GetThis()
	{
		return this;
	}

	protected override void Init()
	{
		openScenes = new List<Scene>();
		for (int i = 0; i < SceneManager.sceneCount; ++i)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (scene.buildIndex != gameObject.scene.buildIndex)
			{
				openScenes.Add(scene);
			}
		}
	}

	#endregion // Singleton Implementation

	#region Editor Fields

	[SerializeField] private ScreenTransitioner transitioner;

	#endregion // Editor Fields

	#region Private Fields

	private List<Scene> openScenes;

	#endregion // Private Fields

	#region Public Functions

	public Coroutine TransitionScenes(bool withScreenTransition, params SceneReference[] scenesThatShouldBeOpen)
	{
		return StartCoroutine(TransitionScenesImpl(scenesThatShouldBeOpen, withScreenTransition));
	}

	#endregion // Public Functions

	#region Private Functions

	private IEnumerator TransitionScenesImpl(SceneReference[] scenesThatShouldBeOpen, bool withScreenTransition)
	{
		if (withScreenTransition)
		{
			yield return transitioner.HideScreen();
		}

		HashSet<int> desiredBuildIndices = ConvertSceneReferenceArrayToBuildIndexSet(scenesThatShouldBeOpen);
		yield return StartCoroutine(UnloadScenes(desiredBuildIndices));
		yield return StartCoroutine(LoadScenes(desiredBuildIndices));
		SceneManager.SetActiveScene(openScenes[openScenes.Count - 1]);

		if (withScreenTransition)
		{
			yield return transitioner.RevealScreen();
		}
	}

	private HashSet<int> ConvertSceneReferenceArrayToBuildIndexSet(SceneReference[] sceneReferences)
	{
		HashSet<int> buildIndexSet = new HashSet<int>();

		for (int i = 0; i < sceneReferences.Length; ++i)
		{
			buildIndexSet.Add(sceneReferences[i].BuildIndex);
		}

		return buildIndexSet;
	}

	private IEnumerator UnloadScenes(HashSet<int> desireSceneIndices)
	{
		List<AsyncOperation> unloadOperations = new List<AsyncOperation>();
		for (int i = openScenes.Count; i-- > 0;)
		{
			if (!desireSceneIndices.Contains(openScenes[i].buildIndex))
			{
				unloadOperations.Add(SceneManager.UnloadSceneAsync(openScenes[i]));
				openScenes.RemoveAt(i);
			}
		}
		return unloadOperations.GetEnumerator();
	}

	private IEnumerator LoadScenes(HashSet<int> desiredSceneIndices)
	{
		List<int> openSceneIndices = new List<int>();
		for (int i = 0; i < openScenes.Count; ++i)
		{
			openSceneIndices.Add(openScenes[i].buildIndex);
		}

		desiredSceneIndices.ExceptWith(openSceneIndices);
		foreach (int buildIndex in desiredSceneIndices)
		{
			yield return SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
			openScenes.Add(SceneManager.GetSceneByBuildIndex(buildIndex));
		}
	}

	#endregion // Private Functions
}
