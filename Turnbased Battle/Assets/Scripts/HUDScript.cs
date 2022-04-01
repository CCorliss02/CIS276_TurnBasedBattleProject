using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider healthSlider;
    public Button attackButton;

    public void SetHUD(StatsScript stat)
    {
        nameText.text = stat.Name;
        levelText.text = "Level " + stat.level;
        healthSlider.maxValue = stat.maxHealth;
        healthSlider.value = stat.currentHealth;
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }
}
