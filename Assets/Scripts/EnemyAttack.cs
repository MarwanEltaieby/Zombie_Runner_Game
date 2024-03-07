using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //[SerializeField] GameObject target;
    [SerializeField] [Range (1f, 500f)] float damage = 20f;
    PlayerHealth playerHealth;

    private void Start() {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void AttackHitEvent() {
        //if (target == null) return;
        //playerHealth = target.GetComponent<PlayerHealth>();
        playerHealth.damagePlayer(damage);
    }
}
