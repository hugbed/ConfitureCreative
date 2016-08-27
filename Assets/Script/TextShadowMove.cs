using UnityEngine;
using System.Collections;

public class TextShadowMove : MonoBehaviour {

    public float offset = 4.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.localPosition = new Vector3(
            Mathf.Cos(Time.timeSinceLevelLoad * 2.0f) * offset,
            Mathf.Sin(Time.timeSinceLevelLoad * 2.0f) * offset,
            0.0f
            );
	}
}
