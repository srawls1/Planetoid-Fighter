using System;
using UnityEngine;

[Serializable]
public class ButtonMapping
{
	public string jumpButton;
	public string meleeButton;
	public string shootButton;

	public ButtonMapping(string j, string m, string s)
	{
		jumpButton = j;
		meleeButton = m;
		shootButton = s;
	}
}

public class PlayerData
{
	public PlayerData(int n, Color c, ButtonMapping controls)
	{
		number = n;
		color = c;
		jumpButton = controls.jumpButton;
		meleeButton = controls.meleeButton;
		shootButton = controls.shootButton;
	}

	public readonly int number;
	public readonly string name;
	public readonly Color color;
	public int lives;
	public string jumpButton;
	public string meleeButton;
	public string shootButton;

	public override bool Equals(object obj)
	{
		PlayerData otherPlayerData = obj as PlayerData;
		if (otherPlayerData == null)
		{
			return false;
		}
		return number == otherPlayerData.number;
	}

	public override int GetHashCode()
	{
		return number;
	}
}
