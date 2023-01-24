using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : ScriptableObject
{
    [SerializeField] private string m_name;
    [SerializeField] private string m_description;
    [SerializeField] private Sprite m_sprite;
    [SerializeField] private List<Powerup> m_exclusivePowerups;

    new public string name
	{
        get { return m_name; }
	}

    public string description
	{
        get { return m_description; }
	}

    public Sprite sprite
	{
        get { return m_sprite; }
	}

    public List<Powerup> exclusivePowerups
	{
        get { return m_exclusivePowerups; }
	}

    public abstract void ApplyPowerup(GameObject character);
}
