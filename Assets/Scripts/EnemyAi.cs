using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    GameObject target;
    /*[SerializeField]*/ float chaseRange = Mathf.Infinity;
    [SerializeField] [Range(1f, 20f)] float turnSpeed = 5f;
    [SerializeField] AudioClip chaseSFX;
    [SerializeField] AudioClip attackSFX;
    [SerializeField] AudioClip idleSFX;
    PlayerHealth playerHealth;
    AudioSource audioSource;
    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    int audioFlag = 0;

    private void Start() {
        //getting the navmesh component from the object
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        target = GameObject.FindWithTag("Player");
    }

    private void Update() {
        //measuring the distance to target
        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);

        //Engage the target if provoked
        if (isProvoked) {
            EngageTarget();
        //if get close to the target then set provoke to be active        
        } else if (distanceToTarget <= chaseRange) {
            isProvoked = true;
        }

    }

    private void EngageTarget() {
        if (distanceToTarget >= navMeshAgent.stoppingDistance) {
            //if he is close to the radius then chase the target
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance) {
            //if he is too close attack the target
            AttackTarget();
        }
    }

    private void ChaseTarget() {
        GetComponent<Animator>().SetBool("attack", false);
        GetComponent<Animator>().SetTrigger("move");
        if (audioFlag == 2) {
            audioFlag = 0;
        }
        if (audioFlag == 0) {
            audioSource.Stop();
            audioFlag++;
        }
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(chaseSFX);
        }
        navMeshAgent.SetDestination(target.transform.position);
    }

    private void AttackTarget() {
        FaceTarget();
        GetComponent<Animator>().SetBool("attack", true);
        if (audioFlag == 1) {
            audioSource.Stop();
            audioFlag++;
        }
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(attackSFX);
        }
        if (playerHealth.getPlayerHealth() <= 0) {
            audioSource.Stop();
        }
    }

    private void FaceTarget() {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void OnDrawGizmosSelected(){
        //setting the color of the drawn object
        Gizmos.color = Color.red; 
        //setting the drawn gizmo's shape, position and its dimensions
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    public void setIsProvoked(bool isProvoked) {
        this.isProvoked = isProvoked;
    }
}
