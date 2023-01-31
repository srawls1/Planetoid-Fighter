using UnityEngine;

public class SoundtrackSetter : MonoBehaviour
{
	[SerializeField] private AudioClip soundtrack;

	private void Start()
	{
		MusicManager.instance.currentMusic = soundtrack;
	}
}
