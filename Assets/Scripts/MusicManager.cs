using System.Collections;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
	#region Editor Fields

	[SerializeField] private float baseVolume;
	[SerializeField] private float fadeOutDuration;
	[SerializeField] private float fadeInDuration;

	#endregion // Editor Fields

	#region Private Fields

	private AudioSource musicSource;

	#endregion // Private Fields

	#region Properties

	private AudioClip m_currentMusic;
	public AudioClip currentMusic
	{
		get { return m_currentMusic; }
		set
		{
			if (m_currentMusic == value)
			{
				return;
			}

			m_currentMusic = value;
			StartCoroutine(FadeToNewSoundtrack());
		}
	}

	#endregion // Properties

	#region Singleton Implementation

	protected override MusicManager GetThis()
	{
		return this;
	}

	protected override void Init()
	{
		musicSource = GetComponent<AudioSource>();
		musicSource.ignoreListenerPause = true;
		musicSource.volume = GetDesiredVolume();
		SettingsManager.instance.volumeChangedEvent.AddListener(OnVolumeChanged);
	}

	#endregion // Singleton Implementation

	#region Private Functions

	private void OnVolumeChanged(string channel, float volume)
	{
		if (channel.Equals("Master") || channel.Equals("Music"))
		{
			musicSource.volume = GetDesiredVolume();
		}
	}

	private float GetDesiredVolume()
	{
		return baseVolume
			* SettingsManager.instance.GetVolume("Master")
			* SettingsManager.instance.GetVolume("Music");
	}

	private IEnumerator FadeToNewSoundtrack()
	{
		float maxVolume = GetDesiredVolume();
		for (float dt = 0f; dt < fadeOutDuration; dt += Time.deltaTime)
		{
			musicSource.volume = Mathf.Lerp(maxVolume, 0f, dt / fadeOutDuration);
			yield return null;
		}

		musicSource.Stop();
		musicSource.clip = currentMusic;
		musicSource.Play();

		for (float dt = 0f; dt < fadeInDuration; dt += Time.deltaTime)
		{
			musicSource.volume = Mathf.Lerp(0f, maxVolume, dt / fadeInDuration);
			yield return null;
		}
	}

	#endregion // Private Functions
}
