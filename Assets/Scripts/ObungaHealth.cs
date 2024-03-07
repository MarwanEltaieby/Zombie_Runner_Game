using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObungaHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    //[SerializeField] AudioClip deathSFX;
    //AudioSource audioSource;
    Animator animator;
    ObungaAi obungaAi;
    NavMeshAgent navMeshAgent;
    //bool isDead = false;
    BoxCollider boxCollider;
    //[SerializeField] GameObject zombieHead;
    [SerializeField] GameObject maxAmmoPickUp;
    [SerializeField] [Range(0f, 100f)] float luckPercentage;
    float luckMultiPlier = 10;
    static float luck;

    private void Start() {
        luck = luckMultiPlier * luckPercentage;
        //audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        obungaAi = GetComponent<ObungaAi>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        boxCollider = GetComponent<BoxCollider>();
    }

    //create a public method which reduces the hit points by the amount of damage
    public void TakeDamage(float damage) {
        hitPoints -= damage;
        // if the hit points is less than or equal 0, then die
        if (hitPoints <= 0) {
            // if (isDead) return; 
            // animator.SetTrigger("die");
            // audioSource.Stop();
            // audioSource.PlayOneShot(deathSFX);
            // obungaAi.enabled = false;
            // navMeshAgent.enabled = false;
            // boxCollider.enabled = false;
            // zombieHead.GetComponent<SphereCollider>().enabled = false;
            // isDead = true;
            EnemySpawnManager.IncrementKillCounter();
            // this.enabled = false;
            int randomNum = Random.Range(0, 1000);
            if (luck > randomNum) {
                Instantiate(maxAmmoPickUp, transform.position, Quaternion.identity);
                luck = luckMultiPlier * luckPercentage;
            } else {
                luck++;
            }
            Destroy(gameObject);
        }
    }
}
