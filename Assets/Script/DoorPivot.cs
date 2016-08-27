using UnityEngine;
using System.Collections;

public class DoorPivot : MonoBehaviour {
    
    public float doorSpeed = 1.0f;

    bool animate = false;
    bool Opened = false;

    public void Open()
    {
        animate = true;
    }

	// Use this for initialization
	void Start () {
        gameObject.transform.rotation = Quaternion.identity;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //animate = true;
        }
        if( animate && !Opened)
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        Vector3 vec = new Vector3(); float angle = new float();

        gameObject.transform.rotation.ToAngleAxis(out angle, out vec);
        var sign = Mathf.Sign(angle + 0.0001f);
        if ( angle < 90.0f )
        {
            gameObject.transform.rotation = Quaternion.AngleAxis(angle + doorSpeed * Time.deltaTime, Vector3.up * -sign);
        }
        else if ( gameObject.transform.rotation.y >= 90.0f )
        {
            Opened = false;
        }
    }
}
