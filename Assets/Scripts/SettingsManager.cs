using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingsManager : Singleton<SettingsManager>
{
	#region Singleton Implementation

	protected override SettingsManager GetThis()
	{
		return this;
	}

	protected override void Init()
	{
		volumes = new Dictionary<string, float>();
		volumes.Add("Master", 1f);
		volumes.Add("Music", 1f);
		volumes.Add("Sound Effects", 1f);
		volumes.Add("Voice", 1f);

		enabledPowerups = new Dictionary<Powerup, bool>();
		for (int i = 0; i < allPowerups.Count; ++i)
		{
			enabledPowerups.Add(allPowerups[i], true);
		}
	}

	#endregion // Singleton Implementation

	#region Editor Fields

	[SerializeField] private int m_lives = 3;
	[SerializeField] private int m_maxPossibleLives;
	[SerializeField] private List<Powerup> allPowerups;
	[SerializeField] private UnityEvent<string, float> m_volumeChangedEvent;

	#endregion // Editor Fields

	#region Private Fields

	private Dictionary<string, float> volumes;
	private Dictionary<Powerup, bool> enabledPowerups;

	#endregion // Private Fields

	#region Properties

	public int lives
	{
		get { return m_lives; }
		set
		{
			m_lives = Mathf.Clamp(value, 1, maxPossibleLives);
		}
	}

	public int maxPossibleLives
	{
		get { return m_maxPossibleLives; }
	}

	public UnityEvent<string, float> volumeChangedEvent
	{
		get { return m_volumeChangedEvent; }
	}

	#endregion // Properties

	#region Public Functions

	public float GetVolume(string channel)
	{
		return volumes[channel];
	}

	public void SetVolume(string channel, float volume)
	{
		volumes[channel] = volume;
		volumeChangedEvent.Invoke(channel, volume);
	}

	public bool IsPowerupEnabled(Powerup powerup)
	{
		return enabledPowerups[powerup];
	}

	public void SetPowerupEnabled(Powerup powerup, bool enabled)
	{
		enabledPowerups[powerup] = enabled;
	}

	public List<Powerup> GetAllEnabledPowerups()
	{
		List<Powerup> powerups = new List<Powerup>();
		foreach (KeyValuePair<Powerup, bool> pair in enabledPowerups)
		{
			if (pair.Value)
			{
				powerups.Add(pair.Key);
			}
		}

		return powerups;
	}

	#endregion // Public Functions
}
