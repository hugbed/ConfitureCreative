using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DoorTrigger : MonoBehaviour {

    public DoorPivot DoorPivot;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == GameObject.Find("PaperPlane"))
        {
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
            DoorPivot.Open();
        }
    }
}
