using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    Slider HealthBarUI;

    private void Awake()
    {
        HealthBarUI = GetComponent<Slider>();   
    }
    public void UpdateHealthBar(float health)
    {
        HealthBarUI.value = health;
    }

    public void SetMaxHealth(float max)
    {
        HealthBarUI.maxValue = max;
        HealthBarUI.value = max;
    }

    // Update is called once per frame
    void Update()
    {
        // set position above enemy
        // set it rotate equal camera rotate.
    }
}
