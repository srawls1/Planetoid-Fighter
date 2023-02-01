using System;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[CreateAssetMenu]
public class InputElementSpriteMapping : ScriptableObject, ISerializationCallbackReceiver
{
	#region Helper Classes

	[Serializable]
	private class InputElementSpritePair
	{
		public string inputElementName;
		public Sprite sprite;
	}

	[Serializable]
	private class ControllerSpecificSpriteMapping
	{
		public ControllerType controllerType;
		public InputElementSpritePair[] elementSpritePairs;
	}

	#endregion // Helper Classes

	#region Editor Fields

	[SerializeField] private ControllerSpecificSpriteMapping[] controllerSpriteMaps;

	#endregion // Editor Fields

	#region Private Fields

	private Dictionary<ControllerType, Dictionary<string, Sprite>> controllerSpriteDictionary;

	#endregion // Private Fields

	#region Public Functions

	public Sprite GetSprite(ControllerType controllerType, string inputElementName)
	{
		if (!controllerSpriteDictionary.TryGetValue(controllerType, out Dictionary<string, Sprite> elementSpriteDictionary))
		{
			return null;
		}

		if (!elementSpriteDictionary.TryGetValue(inputElementName, out Sprite sprite))
		{
			return null;
		}

		return sprite;
	}

	public void OnAfterDeserialize()
	{
		controllerSpriteDictionary = new Dictionary<ControllerType, Dictionary<string, Sprite>>();

		for (int controllerIndex = 0; controllerIndex < controllerSpriteMaps.Length; ++controllerIndex)
		{
			ControllerSpecificSpriteMapping elementMapping = controllerSpriteMaps[controllerIndex];
			Dictionary<string, Sprite> elementSpriteDictionary = new Dictionary<string, Sprite>();

			for (int elementIndex = 0; elementIndex < elementMapping.elementSpritePairs.Length; ++elementIndex)
			{
				InputElementSpritePair inputSpritePair = elementMapping.elementSpritePairs[elementIndex];
				elementSpriteDictionary.Add(inputSpritePair.inputElementName, inputSpritePair.sprite);
			}

			controllerSpriteDictionary.Add(elementMapping.controllerType, elementSpriteDictionary);
		}
	}

	public void OnBeforeSerialize()
	{
		
	}

	#endregion // Public Functions
}
