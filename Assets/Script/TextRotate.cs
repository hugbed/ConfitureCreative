using UnityEngine;
using System.Collections;

public class TextRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.rotation = Quaternion.AngleAxis(Mathf.Sin(Time.timeSinceLevelLoad * 2.0f) * 20.0f, Vector3.forward);
	}
}
