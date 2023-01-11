using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Singleton<PlayerSpawner>
{
	#region Editor Fields

	[SerializeField] private GameObject characterPrefab;

	#endregion // Editor Fields

	#region Private Fields

	private List<SpawnPoint> spawnPoints;
	private List<PlayerData> spawnedPlayerDatas;
	private List<GameObject> spawnedPlayerGameObjects;

	#endregion // Private Fields

	#region Singleton Implementation

	protected override void Init()
	{
		spawnPoints = new List<SpawnPoint>();
		spawnedPlayerDatas = new List<PlayerData>();
		spawnedPlayerGameObjects = new List<GameObject>();
	}

	protected override PlayerSpawner GetThis()
	{
		return this;
	}

	#endregion // Singleton Implementation

	//<temp>
	private IEnumerator Start()
	{
		yield return null;
		yield return null;
		PlayerManager.instance.StartBattle();
	}
	//</temp>

	#region Public Functions

	public void RegisterSpawnPoint(SpawnPoint spawnPoint)
	{
		spawnPoints.Add(spawnPoint);
	}

	public void SpawnAllPlayers(List<PlayerData> playerDatas)
	{
		for (int i = 0; i < playerDatas.Count && i < spawnPoints.Count; ++i)
		{
			SpawnPlayer(playerDatas[i], spawnPoints[i]);
		}
		if (spawnPoints.Count < playerDatas.Count)
		{
			Debug.LogError("There are more players than spawn points. Was not able to spawn all of them.");
		}
	}

	public void RespawnPlayer(PlayerData player)
	{
		int playerIndex = spawnedPlayerDatas.IndexOf(player);
		spawnedPlayerDatas.RemoveAt(playerIndex);
		spawnedPlayerGameObjects.RemoveAt(playerIndex);
		SpawnPoint farthestSpawnPoint = GetFarthestSpawnPoint();
		SpawnPlayer(player, farthestSpawnPoint);
	}

	#endregion // Public Functions

	#region Private Functions

	private SpawnPoint GetFarthestSpawnPoint()
	{
		float farthestDistance = 0f;
		SpawnPoint farthestSpawnPoint = null;
		for (int spawnPointIndex = 0; spawnPointIndex < spawnPoints.Count; ++spawnPointIndex)
		{
			float closestCharacterToSpawnPointDistance = float.MaxValue;

			for (int characterIndex = 0; characterIndex < spawnedPlayerGameObjects.Count; ++characterIndex)
			{
				float characterToSpawnPointDistance = Vector3.Distance(
					spawnPoints[spawnPointIndex].transform.position,
					spawnedPlayerGameObjects[characterIndex].transform.position);
				if (characterToSpawnPointDistance < closestCharacterToSpawnPointDistance)
				{
					closestCharacterToSpawnPointDistance = characterToSpawnPointDistance;
				}
			}

			if (closestCharacterToSpawnPointDistance > farthestDistance)
			{
				farthestDistance = closestCharacterToSpawnPointDistance;
				farthestSpawnPoint = spawnPoints[spawnPointIndex];
			}
		}

		return farthestSpawnPoint;
	}

	private void SpawnPlayer(PlayerData player, SpawnPoint spawnPoint)
	{
		GameObject character = Instantiate(characterPrefab, spawnPoint.transform.position, Quaternion.identity);
		spawnedPlayerDatas.Add(player);
		spawnedPlayerGameObjects.Add(character);
		PlayerCharacter playerCharacter = character.GetComponent<PlayerCharacter>();
		playerCharacter.player = player;
	}

	#endregion // Private Functions
}
