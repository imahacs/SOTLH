using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTest : MonoBehaviour
{
  
    public Light playerLight;
    private bool isLightOn = true;
    private bool isDead = false;

    private EnemyController[] enemies;

    private void Start()
    {
        enemies = FindObjectsOfType<EnemyController>();
    }

    private void Update()
    {

        ToggleLight();
    }



    private void ToggleLight()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isLightOn = !isLightOn;
            playerLight.enabled = isLightOn;
            NotifyEnemies();
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TriggerDeathAnimation()
    {
        if (!isDead)
        {
            isDead = true;
            // Play death animation if needed
            // animator.SetTrigger("Die");

            // Destroy the player game object
            Destroy(gameObject);

            // Additional logic for handling player death, such as game over state or respawn logic
        }
    }

    public bool IsLightOn()
    {
        return isLightOn;
    }

    private void NotifyEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.OnLightToggled();
        }
    }
}
