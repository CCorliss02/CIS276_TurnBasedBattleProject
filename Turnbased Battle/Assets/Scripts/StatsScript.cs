using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsScript : MonoBehaviour
{
    public string Name;
    public int level;
    public int attackStat;
    public int maxHealth;
    public int currentHealth;
    public int healAmount;
    public int magicAttackStat;
    public int magicPoints;

    public bool Damage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public void Heal(int heal)
    {
        currentHealth += heal;
    }
}