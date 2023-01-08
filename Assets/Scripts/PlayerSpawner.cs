using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Singleton<PlayerSpawner>
{
	#region Editor Fields

	[SerializeField] private GameObject characterPrefab;

	#endregion // Editor Fields

	private List<SpawnPoint> spawnPoints;

	#region Singleton Implementation

	protected override void Init()
	{
		spawnPoints = new List<SpawnPoint>();
	}

	protected override PlayerSpawner GetThis()
	{
		return this;
	}

	#endregion // Singleton Implementation

	public void RegisterSpawnPoint(SpawnPoint spawnPoint)
	{
		spawnPoints.Add(spawnPoint);
	}

	public void SpawnAllPlayers()
	{

	}

	public void RespawnPlayer(PlayerData player)
	{

	}

	private void SpawnPlayer(PlayerData player, SpawnPoint spawnPoint)
	{
		GameObject character = Instantiate(characterPrefab, spawnPoint.transform.position, Quaternion.identity);
		//character.data = players[i];
		//players[i].alive = true;
	}
}
