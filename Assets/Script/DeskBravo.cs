using UnityEngine;
using System.Collections;

public class DeskBravo : MonoBehaviour {

    public float winDelay = 2.0f;

    private float collisionTime = 0.0f;

    void Start()
    {
        collisionTime = 0.0f;
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject == GameObject.Find("PaperPlane"))
        {
            collisionTime += Time.deltaTime;

            if (collisionTime >= winDelay)
            {
                GameState.instance.GameWin();
            }
        }
    }
}
