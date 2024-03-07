using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    PlayerHealth playerHealth;
    [SerializeField] Canvas gameOverCanvass;

    private void Start() {
        gameOverCanvass.gameObject.SetActive(false);
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void HandleDeath() {
        gameOverCanvass.gameObject.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        EnemyAi[] enemyAis = FindObjectsOfType<EnemyAi>();
        foreach (EnemyAi enemyAi in enemyAis) {
            enemyAi.enabled = false;
        }
    }

}
