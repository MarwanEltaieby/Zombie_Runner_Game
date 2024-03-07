using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Weapon : MonoBehaviour
{
    //Refrencing out FPS camera 
    [SerializeField] Camera FPCam; 
    //caching a refrence that tells the range of the raycast bullet
    [SerializeField] float range =  100f; 
    [SerializeField] float damage = 30f;
    [SerializeField] ParticleSystem muzzleFlashVFX;
    [SerializeField] AudioClip shootAudio;
    [SerializeField] GameObject hitVFX;
    [SerializeField] float fireRate;
    [SerializeField] GameObject bolt;
    [SerializeField] float aimDownTheSightSpeed;
    [SerializeField] Transform defaultWaponPosition;
    [SerializeField] Transform aimDownTheSightPosition; 
    [SerializeField] float hipFireSensitivity = 2f;
    [SerializeField] float adsSensitivity = 1f;
    [SerializeField] float adsWeaponZoom;   
    [SerializeField] AudioClip magInSFX;
    [SerializeField] AudioClip reloadSFX;
    float currentAmmo;
    [SerializeField] float maximumAmmo = Mathf.Infinity;
    float reservedAmmo;
    [SerializeField] float magSize;
    [SerializeField] float changingMagTime;
    [SerializeField] float pullingBoltTime;
    [SerializeField] AudioClip emptyMagShot;
    GameObject ammo; 
    TMP_Text ammoText;
    RigidbodyFirstPersonController playerController;
    float defaultFOV;
    GameObject reticle;
    bool isADS = false;
    Animator animator;
    //bool isAimingDownTheSight;
    bool isReloading = false;
    Animator boltAnimator;
    RecoilScript recoilHandler;
    float timer = 0;
    AudioSource audioSource;
    Ammo ammoSlot;

    private void Start() {
        ammoSlot = FindObjectOfType<Ammo>();
        ammo = GameObject.FindWithTag("Ammo");
        ammoText = ammo.GetComponent<TMP_Text>();
        reservedAmmo = maximumAmmo;
        currentAmmo = magSize;
        ammoText.SetText($"Ammo {currentAmmo} / {reservedAmmo}");
        defaultFOV = FPCam.fieldOfView;
        playerController = FindObjectOfType<RigidbodyFirstPersonController>();
        reticle = GameObject.FindWithTag("Reticle");
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        recoilHandler = FindObjectOfType<RecoilScript>();
    }

    

    private void Update() {
        ammoText.SetText($"Ammo {currentAmmo} / {reservedAmmo}");
        timer -= Time.deltaTime;
        //If I press the fire button then I will shoot
        if (!isReloading && currentAmmo > 0) {
            if (gameObject.CompareTag("Carbine")) {
                if (Input.GetButton("Fire1")) {
                    if (timer <= 0) {
                        Shoot();
                        timer = fireRate;
                    }
                }
            } else { 
                if (Input.GetButtonDown("Fire1")) {
                    if (timer <= 0f) {
                        Shoot();
                        timer = fireRate;
                    }
                }
            }
        } else if (currentAmmo == 0) {
            if (Input.GetButtonDown("Fire1")) {
                audioSource.PlayOneShot(emptyMagShot);
            }
        }

        if (Input.GetButton("Fire2")) {
            if (isReloading) {return;}
            StartAimDownTheSight();
        } else if (Input.GetButtonUp("Fire2")){
            if (isReloading) {return;}
            StopAimDownTheSight();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < magSize) {
            StartCoroutine(ReloadWeapon());
        } 
    }

    private IEnumerator ReloadWeapon() {
        if (reservedAmmo > 0 && !isReloading) {
            isReloading = true;
            Debug.Log("Reloading..");
            StopAimDownTheSight();
            if (gameObject.CompareTag("Carbine")) {
                audioSource.PlayOneShot(reloadSFX);
            }
            if (gameObject.CompareTag("Pistol")) {
                audioSource.PlayOneShot(magInSFX);
            }
            animator.SetTrigger("StopADS");
            animator.SetTrigger("Reload");
            yield return new WaitForSeconds(changingMagTime);
            if (gameObject.CompareTag("Pistol")) {
                audioSource.PlayOneShot(reloadSFX);
            }
            boltAnimator = boltAnimator.GetComponent<Animator>();
            boltAnimator.SetTrigger("pull bolt");
            yield return new WaitForSeconds(pullingBoltTime);
            reservedAmmo -= (magSize - currentAmmo);
            currentAmmo = magSize;
            if (reservedAmmo < 0) {
                currentAmmo += reservedAmmo;
                reservedAmmo = 0;
            }
            ammoText.SetText($"Ammo {currentAmmo} / {reservedAmmo}");
            isReloading = false;
        }
    }

    private void Shoot() {
        PlayBoltAnimation();
        PlaySoundFX();
        PlayMuzzleFlash();
        ProcessRaycast();
        ProcessRecoil();
        DeacreaseCurrentAmmo();
    }

    private void DeacreaseCurrentAmmo() {
        currentAmmo--;
        ammoText.SetText($"Ammo {currentAmmo} / {reservedAmmo}");
    }

    private void ProcessRecoil() {
        recoilHandler.ProcessRecoil();
    }

    private void PlayBoltAnimation() {
        boltAnimator = bolt.GetComponent<Animator>();
        boltAnimator.SetTrigger("shoot");
    }

    private void PlaySoundFX() {
        audioSource.PlayOneShot(shootAudio);
    }

    private void ProcessRaycast() {
        RaycastHit hit;
        //the if statement is for dealing with the null refrence
        if (Physics.Raycast(FPCam.transform.position, FPCam.transform.forward, out hit, range)) {
            // {or}
            // if (hit.transform == null) {
            //     return;
            // } else {
            CreateHitImpact(hit);
            //Accessing the EnemyHealth script from the hit target
            if (hit.transform.CompareTag("Zombie")) {
                Debug.Log(hit.transform.name);
                EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(damage);
                EnemyAi enemy = hit.transform.GetComponent<EnemyAi>();
                enemy.setIsProvoked(true);
            }
            if (hit.transform.CompareTag("Head")) {
                Debug.Log(hit.transform.name);
                GameObject parent = hit.transform.parent.gameObject;
                EnemyHealth enemyHealth = parent.transform.parent.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(damage * 2);
                EnemyAi enemy = parent.transform.parent.GetComponent<EnemyAi>();
                enemy.setIsProvoked(true);
            }
            if (hit.transform.CompareTag("Obunga")) {
                ObungaHealth obungaHealth = hit.transform.GetComponent<ObungaHealth>();
                obungaHealth.TakeDamage(damage);
            }
        } else {
            return;
        }
    }

    private void StartAimDownTheSight() {
        //isAimingDownTheSight = true;
        FPCam.fieldOfView = Mathf.Lerp(FPCam.fieldOfView, defaultFOV / adsWeaponZoom, aimDownTheSightSpeed * Time.deltaTime);
        if(!isADS) {
            playerController.mouseLook.XSensitivity = adsSensitivity;
            playerController.mouseLook.YSensitivity = adsSensitivity;
            reticle.SetActive(false);
            //animator.SetBool("isADS", true);
            animator.SetTrigger("StartADS");
            isADS = true;
        }
        // gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, aimDownTheSightPosition.localPosition, aimDownTheSightSpeed * Time.deltaTime);
    }

    private void StopAimDownTheSight() {
        //isAimingDownTheSight = false;
        FPCam.fieldOfView = Mathf.Lerp(FPCam.fieldOfView, defaultFOV, aimDownTheSightSpeed * Time.deltaTime);
        if (isADS) {
            playerController.mouseLook.XSensitivity = hipFireSensitivity;
            playerController.mouseLook.YSensitivity = hipFireSensitivity;
            reticle.SetActive(true);
            //animator.SetBool("isADS", false);
            animator.SetTrigger("StopADS");
            isADS = false;
        }
        //gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, defaultWaponPosition.localPosition, aimDownTheSightSpeed * Time.deltaTime);
    }

    //public bool getIsAimingDownTheSight() {
        //return isAimingDownTheSight;
    //}

    private void CreateHitImpact(RaycastHit hit) {
        GameObject impact = Instantiate(hitVFX, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.1f);
    }

    private void PlayMuzzleFlash() {
        muzzleFlashVFX.Play();
    }

    public void ResetReservedAmmo() {
        reservedAmmo = maximumAmmo;
        ammoText.SetText($"Ammo {currentAmmo} / {reservedAmmo}");
    }
    public void SetIsReloding(bool value) {
        isReloading = value;
    } 
}
