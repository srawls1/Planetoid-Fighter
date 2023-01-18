using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerData
{
	#region Constructor

	public PlayerData(Player rewiredPlayer, int number, Color color)
	{
		this.rewiredPlayer = rewiredPlayer;
		this.number = number;
		name = $"Player {number}";
		this.color = color;
		this.powerups = new List<Powerup>();
	}

	#endregion // Constructor

	#region Fields

	public readonly Player rewiredPlayer;
	public readonly int number;
	public string name;
	public Color color;
	public int lives;
	private List<Powerup> powerups;

	#endregion // Fields

	#region Public Functions

	public void AddPowerup(Powerup powerup)
	{
		powerups.Add(powerup);
	}

	public void ClearPowerups()
	{
		powerups.Clear();
	}

	public IReadOnlyList<Powerup> GetPowerups()
	{
		return powerups;
	}

	#endregion // Public Functions

	#region Equals and GetHashCode

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

	#endregion // Equals and GetHashCode
}
