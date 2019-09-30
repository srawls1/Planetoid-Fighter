using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerSpawner : MonoBehaviour
{
	[SerializeField] private Text winText;
	[SerializeField] private CharacterController characterPrefab;
	[SerializeField] private Transform[] spawnLocations;
	[SerializeField] private float returnToMenuDelay;
	[SerializeField] private string sceneName;

	private void Awake()
	{
		List<Vector2> spawnPositions = new List<Vector2>(spawnLocations.Length);
		for (int i = 0; i < spawnLocations.Length; ++i)
		{
			spawnPositions.Add(spawnLocations[i].position);
		}

		PlayerManager.instance.SpawnCharacters(characterPrefab, spawnPositions);
		PlayerManager.instance.OnGameWon += OnGameWon;
	}

	private void OnDestroy()
	{
		PlayerManager.instance.OnGameWon -= OnGameWon;
	}

	private void OnGameWon(PlayerData player)
	{
		winText.gameObject.SetActive(true);
		winText.text = string.Format("P{0} Wins", player.number);
		winText.color = player.color;
		StartCoroutine(DelayedReturnToMenu());
	}

	private IEnumerator DelayedReturnToMenu()
	{
		yield return new WaitForSeconds(returnToMenuDelay);
		SceneManager.LoadScene(sceneName);
	}
}
