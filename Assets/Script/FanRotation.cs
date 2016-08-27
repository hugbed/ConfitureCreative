using UnityEngine;
using System.Collections;

public class FanRotation : MonoBehaviour {

    public Vector3 axis;
    public float speed;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rb.angularVelocity = axis * speed;
	}
}
