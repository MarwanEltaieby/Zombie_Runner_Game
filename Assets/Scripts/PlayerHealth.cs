using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] [Range(1f, 500f)] float playerHealth = 100;
    DeathHandler deathHandler;
    [SerializeField] GameObject image;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] float healthRegenerationCooldown = 5f;
    [SerializeField] float healthRegenerationCooldownAterHit = 5f;
    //[SerializeField] TMP_Text healthText;
    GameObject healthBar; 
    float playerHitPoints;
    float regenCooldown;
    Animator imageAnimator;
    AudioSource audioSource;

    private void Start() {
        healthBar = GameObject.FindWithTag("Health Bar");
        healthBar.GetComponent<Slider>().maxValue = playerHealth;
        healthBar.GetComponent<Slider>().value = playerHealth;
        playerHitPoints = playerHealth;
        regenCooldown = healthRegenerationCooldown;
        deathHandler = GetComponent<DeathHandler>();
        imageAnimator = image.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //healthText.SetText("HP " + playerHitPoints);
    }

    private void Update() {
        regenCooldown -= Time.deltaTime;
        if (regenCooldown <= 0 && playerHitPoints < playerHealth) {
            playerHitPoints += 20f;
            if (playerHitPoints > playerHealth) {
                playerHitPoints = playerHealth;
            }
            healthBar.GetComponent<Slider>().value = playerHitPoints;
            //healthText.SetText("HP " + playerHitPoints);
            regenCooldown = healthRegenerationCooldown;
        }
    }

    public void damagePlayer(float damage) {
        playerHitPoints -= damage;
        if (playerHitPoints <= 0) {
            healthBar.GetComponent<Slider>().value = playerHitPoints;
            //healthText.SetText("HP " + playerHitPoints);
            audioSource.PlayOneShot(deathSFX);
            deathHandler.HandleDeath();
        } else {
            healthBar.GetComponent<Slider>().value = playerHitPoints;
            //healthText.SetText("HP " + playerHitPoints);
            regenCooldown = healthRegenerationCooldownAterHit;
            imageAnimator.SetTrigger("hit");
            audioSource.PlayOneShot(hitSFX);
            Debug.Log(playerHealth + ", " + playerHitPoints);
        }
    }

    public float getPlayerHealth() {
        return playerHealth;
    }
}
