using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TranslateFloorUp : MonoBehaviour {
    public GameObject objectToGoThrough1;
    public GameObject objectToGoThrough2;

    public Transform cameraTransform;    
    public GameObject cinemachine;

    private bool skipOneFrame = false;

    // Update is called once per frame
    void Update() {
        if(skipOneFrame) {
            skipOneFrame = false;
        } else if (  ! cinemachine.activeSelf ) {
            cinemachine.SetActive(true);
        }
    }

    public void Translate() {
        objectToGoThrough1.SetActive(false);
        objectToGoThrough2?.SetActive(false);
        GetComponent<CharacterController>().Move(Vector3.up * 4.17f);
        //playerVelocity.y = 0;
        objectToGoThrough1.SetActive(true);
        objectToGoThrough2?.SetActive(true);

        if(cameraTransform != null) {
            cinemachine.SetActive(false);
            Vector3 cameraPosition = cameraTransform.position;
            cameraPosition.y += 4.17f;
            cameraTransform.position = cameraPosition;
            skipOneFrame = true;
        }
    }

}
