using UnityEngine;
using System.Collections;

public class CoeurScript : MonoBehaviour {

    public float baseScale = 1.0f;
    public float sinMagnitude = 1.0f;
    public float radius = 0.0f;
    public float turns = 0.0f;
    public float rotateCoeff = 0.0f;
    public float beatFreq = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var coeff = baseScale + Mathf.Cos(Time.realtimeSinceStartup * beatFreq * 2.4666666f * Mathf.PI) * sinMagnitude;
        gameObject.transform.localScale = new Vector3(coeff, coeff, coeff);

        gameObject.transform.localPosition = new Vector3(
            Mathf.Cos(Time.realtimeSinceStartup * 0.6f + turns * 2.0f * Mathf.PI) * radius * (1.0f + 0.2f * Mathf.Sin(Time.realtimeSinceStartup * 2.0f)),
            Mathf.Sin(Time.realtimeSinceStartup * 0.6f + turns * 2.0f * Mathf.PI) * radius * (1.0f + 0.2f * Mathf.Sin(Time.realtimeSinceStartup * 2.0f)),
            0.0f
            );

        gameObject.transform.rotation = Quaternion.AngleAxis(rotateCoeff * Mathf.Sin(Time.realtimeSinceStartup * 2.0f + turns * 2.0f * Mathf.PI) * 20.0f, Vector3.forward);
    }
}
