
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	private void Start()
	{
		PlayerSpawner.instance.RegisterSpawnPoint(this);
	}
}
