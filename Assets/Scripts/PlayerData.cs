using UnityEngine;
using Rewired;

public class PlayerData
{
	public PlayerData(Player rewiredPlayer, int number, Color color)
	{
		this.rewiredPlayer = rewiredPlayer;
		this.number = number;
		name = $"Player {number}";
		this.color = color;
	}

	public readonly Player rewiredPlayer;
	public readonly int number;
	public readonly string name;
	public readonly Color color;
	public int lives;

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
