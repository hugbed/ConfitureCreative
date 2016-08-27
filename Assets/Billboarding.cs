using UnityEngine;
using System.Collections;

public class Billboarding : MonoBehaviour {

    public Material billboardMat;
	// Use this for initialization
	void Start () {
        GetComponent<BillboardRenderer>().billboard = new BillboardAsset();
        GetComponent<BillboardRenderer>().billboard.material = billboardMat;
        Debug.Log("club Med");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
