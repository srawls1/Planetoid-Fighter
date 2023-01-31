using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	#region Statics

	private static Dictionary<string, SoundManager> soundManagersByChannel;
	
	static SoundManager()
	{
		soundManagersByChannel = new Dictionary<string, SoundManager>();
	}

	public static SoundManager GetSoundManagerByChannel(string channel)
	{
		return soundManagersByChannel[channel];
	}

	#endregion // Statics

	#region Editor Fields

	[SerializeField] private float baseVolume;
	[SerializeField] private AudioClip[] testSounds;
	[SerializeField] private string channel;

	#endregion // Editor Fields

	#region Private Fields

	private AudioSource audioSource;

	#endregion // Private Fields

	#region Unity Functions

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		soundManagersByChannel[channel] = this;
	}

	private void Start()
	{
		SettingsManager.instance.volumeChangedEvent.AddListener(OnVolumeChanged);
	}

	#endregion // Unity Functions

	#region Public Functions

	public void PlaySound(AudioClip clip)
	{
		audioSource.PlayOneShot(clip);
	}

	#endregion // Public Functions

	#region Private Functions

	private void OnVolumeChanged(string channel, float volume)
	{
		if (channel.Equals("Master") || channel.Equals(this.channel))
		{
			audioSource.volume = baseVolume
				* SettingsManager.instance.GetVolume("Master")
				* SettingsManager.instance.GetVolume(channel);
		}
		if (channel.Equals(this.channel))
		{
			AudioClip testSound = testSounds[Random.Range(0, testSounds.Length)];
			audioSource.PlayOneShot(testSound);
		}
	}

	#endregion // Private Functions
}
