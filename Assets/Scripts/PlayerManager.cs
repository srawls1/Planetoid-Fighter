using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{
	public PlayerData(int n, Color c, bool d)
	{
		number = n;
		color = c;
		realDirectionInput = d;
	}

	public int number;
	public Color color;
	public bool realDirectionInput;
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

	[SerializeField] private List<Color> playerColors;
	[SerializeField] private int numControllers;
	[SerializeField] private float gameEndZoomSize;
	[SerializeField] private float gameEndZoomTime;
	[SerializeField] private float gameEndRestDuration;
	[SerializeField] private float gameEndPauseWait;
	[SerializeField] private float gameEndPauseDuration;

	private bool[] hasPlayerJoined;
	private Phase phase;
	private List<PlayerData> players;

	public delegate void PlayersChangedDelegate(List<PlayerData> player);
	public event PlayersChangedDelegate OnPlayersChanged;

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
		// if (OnPlayersChanged != null)
		// {
		// 	OnPlayersChanged(players);
		// }

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
			character.realDirectionInput = players[i].realDirectionInput;
			playerColors.Add(players[i].color);

		}

		phase = Phase.Playing;
	}

	public void OnPlayerDied(CharacterController character)
	{
		int index = players.FindIndex((playerData) => playerData.number == character.playerNumber);
		players.RemoveAt(index);
		if (players.Count == 1)
		{
			StartCoroutine(GameEndJuice(character));
		}
	}

	public void ToggleRealDirection(int playerNumber)
	{
		int index = players.FindIndex((p) => p.number == playerNumber);
		PlayerData player = players[index];
		player.realDirectionInput = !player.realDirectionInput;
		players[index] = player;
		if (OnPlayersChanged != null)
		{
			OnPlayersChanged(players);
		}
	}

	public void CancelJoin(int playerNumber)
	{
		int index = players.FindIndex((player) => player.number == playerNumber);
		if (index >= 0)
		{
			hasPlayerJoined[playerNumber] = false;
			playerColors.Add(players[index].color);
			players.RemoveAt(index);
			if (OnPlayersChanged != null)
			{
				OnPlayersChanged(players);
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
					PlayerData player = new PlayerData(i, playerColors[0], false);
					playerColors.RemoveAt(0);
					players.Add(player);
					if (OnPlayersChanged != null)
					{
						OnPlayersChanged(players);
					}
				}
			}

			yield return null;
		}
	}

	private IEnumerator GameEndJuice(CharacterController player)
	{
		PlayerData winner = players[0];
		Coroutine zoom = CameraMovement.instance.PanAndZoom(player.transform.position,
			gameEndZoomSize, gameEndZoomTime, gameEndRestDuration);
		Coroutine slowDown = StartCoroutine(SlowDownRoutine());

		yield return zoom;
		yield return slowDown;

		if (OnGameWon != null)
		{
			OnGameWon(winner);
		}
	}

	private IEnumerator SlowDownRoutine()
	{
		yield return new WaitForSeconds(gameEndPauseWait);
		Time.timeScale = 0f;
		yield return new WaitForSecondsRealtime(gameEndPauseDuration);
		Time.timeScale = 1f;
	}
}
