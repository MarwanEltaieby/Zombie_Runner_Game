using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] float destructionTimer;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Weapon[] weapons = FindObjectsOfType<Weapon>();
            foreach (Weapon weapon in weapons) {
                weapon.ResetReservedAmmo();
            }
            Destroy(gameObject);
        }
    }

    private void Start() {
        Destroy(gameObject, destructionTimer);
    }
}
