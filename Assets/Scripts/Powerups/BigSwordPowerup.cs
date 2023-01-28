using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScaleUpChildSpec
{
	public string gameObjectName;
	public Vector3 scaleFactor;
	public Vector3 translation;
}

[CreateAssetMenu(menuName = "Powerups/Big sword")]
public class BigSwordPowerup : Powerup
{
	[SerializeField] private List<ScaleUpChildSpec> scaleUpSpecs;

	public override void ApplyPowerup(GameObject character)
	{
		for (int i = 0; i < scaleUpSpecs.Count; ++i)
		{
			Transform child = FindChild(character.transform, scaleUpSpecs[i].gameObjectName.Split('/'));
			child.localScale = Vector3.Scale(child.localScale, scaleUpSpecs[i].scaleFactor);
			child.position += scaleUpSpecs[i].translation;
		}
	}

	private Transform FindChild(Transform parent, string[] childNames)
	{
		for (int i = 0; i < childNames.Length; ++i)
		{
			parent = parent.Find(childNames[i]);
		}
		return parent;
	}
}
