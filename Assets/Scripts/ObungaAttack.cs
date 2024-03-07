using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObungaAttack : MonoBehaviour
{
    //[SerializeField] GameObject target;
    [SerializeField] [Range (1f, 500f)] float damage = 20f;
    PlayerHealth playerHealth;

    private void Start() {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // public void AttackHitEvent() {
    //     //if (target == null) return;
    //     //playerHealth = target.GetComponent<PlayerHealth>();
    //     playerHealth.damagePlayer(damage);
    // }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Player")) {
            playerHealth.damagePlayer(damage);
        }
    }
}
