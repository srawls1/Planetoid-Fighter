using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{
	public PlayerData(int n, Color c)
	{
		number = n;
		color = c;
	}

	public int number;
	public Color color;
}

public class PlayerManager : MonoBehaviour
{
	#region Singleton Instance

	private static PlayerManager m_instance;
	public static PlayerManager instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = FindObjectOfType<PlayerManager>();
				m_instance.Initialize();
			}

			return m_instance;
		}
	}

	void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
			Initialize();
		}
		else if (m_instance != this)
		{
			Debug.LogWarning("More than one player manager in the scene. Destroying one.");
			Destroy(gameObject);
		}
	}

	#endregion // Singleton Instance

	private enum Phase
	{
		Joining,
		Spawning,
		Playing
	}

	[SerializeField] private Color[] playerColors;
	[SerializeField] private int numControllers;

	private bool[] hasPlayerJoined;
	private Phase phase;
	private List<PlayerData> players;

	public delegate void PlayerJoinedDelegate(PlayerData player);
	public event PlayerJoinedDelegate OnPlayerJoined;

	public delegate void GameWonDelegate(PlayerData player);
	public event GameWonDelegate OnGameWon;

	private void Initialize()
	{
		DontDestroyOnLoad(m_instance.gameObject);
		hasPlayerJoined = new bool[numControllers];
		players = new List<PlayerData>();
	}

	public void StartListeningForJoin()
	{
		for (int i = 0; i < hasPlayerJoined.Length; ++i)
		{
			hasPlayerJoined[i] = false;
		}

		players.Clear();
		phase = Phase.Joining;
		StartCoroutine(ListenForJoin());
	}

	public void StopListeningForJoin()
	{
		phase = Phase.Spawning;
	}

	public void SpawnCharacters(CharacterController prefab, List<Vector2> positions)
	{
		for (int i = 0; i < players.Count; ++i)
		{
			CharacterController character = Instantiate(prefab, positions[i], Quaternion.identity);
			character.playerNumber = players[i].number;
			character.color = players[i].color;
		}

		phase = Phase.Playing;
	}

	public void OnPlayerDied(CharacterController character)
	{
		int index = players.FindIndex((playerData) => playerData.number == character.playerNumber);
		players.RemoveAt(index);
		if (players.Count == 1)
		{
			if (OnGameWon != null)
			{
				OnGameWon(players[0]);
			}
		}
	}

	private IEnumerator ListenForJoin()
	{
		while (phase == Phase.Joining)
		{
			for (int i = 0; i < numControllers; ++i)
			{
				if (!hasPlayerJoined[i] && Input.GetButtonDown("Join" + i))
				{
					hasPlayerJoined[i] = true;
					PlayerData player = new PlayerData(i, playerColors[players.Count]);
					players.Add(player);
					if (OnPlayerJoined != null)
					{
						OnPlayerJoined(player);
					}
				}
			}

			yield return null;
		}
	}
}
