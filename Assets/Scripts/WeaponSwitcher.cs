using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField]int currentWeapon = 0;
    int weaponIndex = 0;

    private void Start() {
        SetWeaponActive();
    }

    private void Update() {
        int previousWeapon = currentWeapon;
        ProcessKeyInput();
        if (previousWeapon != currentWeapon) {
            SetWeaponActive();
        }
    }

    private void ProcessKeyInput() {
       if (Input.GetKeyDown(KeyCode.Alpha1)) {
            currentWeapon = 0;
       } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            currentWeapon = 1;
       }
    }

    private void SetWeaponActive() {
        weaponIndex = 0;

        foreach (Transform weapon in transform) {
            if (weaponIndex == currentWeapon) {
                weapon.gameObject.SetActive(true);
                weapon.gameObject.GetComponent<Weapon>().SetIsReloding(false);
            } else {
                weapon.gameObject.SetActive(false);
            }
            weaponIndex++;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ammo Pickup")) {
            foreach (Transform weapon in transform) {
                if (weaponIndex != currentWeapon) {
                    weapon.gameObject.SetActive(true);
                    weapon.gameObject.GetComponent<Weapon>().ResetReservedAmmo();
                    weapon.gameObject.SetActive(false);
                    print("I am not active");
                } else {
                    weapon.gameObject.GetComponent<Weapon>().ResetReservedAmmo();
                    print("active");
                }
            }
        }
    }
}
