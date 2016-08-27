using UnityEngine;
using System.Collections;

public class followCam : MonoBehaviour {

    public GameObject target;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
        var targetPos = target.transform.position;
        targetPos += offset;
        gameObject.transform.position = targetPos;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        var targetPos = target.transform.position;
        targetPos += offset;

        var camPos = gameObject.transform.position;
        camPos += (targetPos - camPos) * 4.0f * Time.deltaTime;
        gameObject.transform.position = camPos;
        //Debug.Log(gameObject.transform.position);
        //gameObject.transform.position = targetPos;
    }
}
