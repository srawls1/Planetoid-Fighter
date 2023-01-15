using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : Singleton<SettingsManager>
{
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
	}

	[SerializeField] private int m_lives = 3;
	[SerializeField] private int m_maxPossibleLives;

	private Dictionary<string, float> volumes;

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

	public float GetVolume(string channel)
	{
		return volumes[channel];
	}

	public void SetVolume(string channel, float volume)
	{
		volumes[channel] = volume;
	}
}
