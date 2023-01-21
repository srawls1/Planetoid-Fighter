using System.Collections.Generic;
using UnityEngine;

public class RandomLevelButton : LevelMenuButton
{
	[SerializeField] private List<SceneReference> possibleScenes;

	public override void SelectLevel()
	{
		SceneReference randomScene = possibleScenes[Random.Range(0, possibleScenes.Count)];
		parentMenu.LoadScene(randomScene);
	}
}
