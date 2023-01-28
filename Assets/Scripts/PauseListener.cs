using UnityEngine;
using UnityEngine.Events;
using Rewired;

public class PauseListener : MonoBehaviour
{
	#region Editor Fields

	[SerializeField] private GameObject pauseScreen;
	[SerializeField] private UnityEvent<bool> m_togglePauseEvent;

	#endregion // Editor Fields

	#region Properties

	public UnityEvent<bool> togglePauseEvent
	{
		get { return m_togglePauseEvent; }
	}

	private Player m_playerWhoPaused;
	public Player playerWhoPaused
	{
		get { return m_playerWhoPaused; }
		set
		{
			if (m_playerWhoPaused != null)
			{
				m_playerWhoPaused.controllers.maps.SetMapsEnabled(false, "Menu");
			}

			m_playerWhoPaused = value;

			if (m_playerWhoPaused != null)
			{
				m_playerWhoPaused.controllers.maps.SetMapsEnabled(true, "Menu");
			}
		}
	}

	// Stop all physics (maybe set time scale to 0?)

	private bool m_paused;
	public bool paused
	{
		get { return m_paused; }
		set
		{
			m_paused = value;

			togglePauseEvent.Invoke(paused);
		}
	}

	#endregion // Properties

	#region Unity Functions

	private void Awake()
	{
		togglePauseEvent.AddListener(SetPauseScreenShowing);
		togglePauseEvent.AddListener(SetInputEnabled);
		togglePauseEvent.AddListener(SetAudioPaused);
		togglePauseEvent.AddListener(SetTimeScale);
	}

	private void Update()
	{
		// This would mean we're in a temporary slowdown that would interfere 
		// with setting timescale for pause, so the simple solution is we just
		// don't allow pausing during such a temporary slowdown.
		if (!paused && Time.timeScale != 1f)
		{
			return;
		}

		for (int i = 0; i < ReInput.players.playerCount; ++i)
		{
			Player player = ReInput.players.GetPlayer(i);
			if (player.GetButtonDown("Pause"))
			{
				paused = !paused;
				playerWhoPaused = paused ? player : null;
			}
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public void ReturnToStartMenu()
	{
		PlayerManager.instance.EndGame();
	}

	#endregion // Public Functions

	#region Private Functions

	private void SetPauseScreenShowing(bool paused)
	{
		pauseScreen.SetActive(paused);
	}

	private void SetInputEnabled(bool paused)
	{
		bool inputEnabled = !paused;
		for (int i = 0; i < ReInput.players.playerCount; ++i)
		{
			Player player = ReInput.players.GetPlayer(i);
			player.controllers.maps.SetMapsEnabled(inputEnabled, "Default");
		}

		if (!paused)
		{
			playerWhoPaused = null;
		}
	}

	private void SetAudioPaused(bool paused)
	{
		AudioListener.pause = paused;
	}

	private void SetTimeScale(bool paused)
	{
		Time.timeScale = paused ? 0f : 1f;
	}

	#endregion // Private Functions
}
