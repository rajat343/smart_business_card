using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamboDoorBehaviour : MonoBehaviour {

    private float curAngle = 0;
    private float desiredAngle = 0;

	// Update is called once per frame
	void Update () {
        curAngle = Mathf.LerpAngle(curAngle, desiredAngle, Time.deltaTime * 3f);
        transform.localEulerAngles = new Vector3(curAngle, 0, 0);
	}

    void OpenDoors() {
        desiredAngle = 60f;
    }

    void ClosedDoors() {
        desiredAngle = 0;
    }

    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("MainCamera")) {
            OpenDoors();
        }
    }

    private void OnTriggerExit(Collider col) {
        if (col.CompareTag("MainCamera")) {
            ClosedDoors();
        }
    }
}
