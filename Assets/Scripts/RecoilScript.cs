using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilScript : MonoBehaviour
{
    [SerializeField] Vector3 recoilThrowEndRange;   

    private void Start() {
        //originalRotation = transform.localEulerAngles;
    }

    public void ProcessRecoil() {
        float xRandom = Random.Range(0, recoilThrowEndRange.x);
        float yRandom = Random.Range(-recoilThrowEndRange.y, recoilThrowEndRange.y);
        float zRandom = Random.Range(-recoilThrowEndRange.z, recoilThrowEndRange.z);
        transform.localEulerAngles += new Vector3(xRandom, yRandom, zRandom);
    }
}
