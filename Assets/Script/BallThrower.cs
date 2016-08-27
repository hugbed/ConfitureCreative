using UnityEngine;
using System.Collections.Generic;
using System;

public class BallThrower : MonoBehaviour
{
    [Range(0, 100)]
    public int agressivity = 30;
    [Range(0, 100)]
    public int aiming = 25;
    public float minScale = 1;
    public float maxScale = 5;

    [Range(0, 100)]
    public float strenght = 35f;

    public float width = 1.0f;
    public float groundOffset = 0.5f;
    public float playerOffset = 10f;

    public GameObject projectile;
    public GameObject Player;
    public float MinimumActiationDistance = 15;

    System.Random pseudo;
    Dictionary<float, GameObject> balls;
    float lifeTime = 2.0f;

    void Start ()
    {
        pseudo = new System.Random(Time.time.GetHashCode());
        balls = new Dictionary<float, GameObject>();
	}
	
	void Update()
    {
        var playerPos = GameObject.Find("PaperPlane").transform.position;
        if (Vector3.Distance(playerPos, transform.position) > MinimumActiationDistance)
        {
            return;
        }

        if (pseudo.Next(500) < pseudo.Next(agressivity) )
        {
            Throw();
        }

        List<float> keyToRemove = new List<float>();
        foreach (KeyValuePair<float, GameObject> ballItem in balls)
        {
            if (Time.time > ballItem.Key + lifeTime)
            {
                Destroy(ballItem.Value);
                keyToRemove.Add(ballItem.Key);
            }
        }
        foreach (float key in keyToRemove)
        {
            balls.Remove(key);
        }
    }

    void Throw()
    {
        var ball = Instantiate(projectile);
        ball.transform.parent = this.transform;

        ball.transform.position = transform.position;

        var delta = 100 - aiming;
        var delX = pseudo.Next(delta) * width/2.0f / 100.0f - width / 2.0f / 2.0f;
        var delY = pseudo.Next(delta) * width / 2.0f / 100.0f - width / 2.0f / 2.0f;
        var deviation = new Vector3(delX, delY, 0.0f);

        var target = Player.transform.position - transform.position + deviation + new Vector3(0, playerOffset, 0 );
        target.Normalize();

        ball.GetComponent<Rigidbody>().AddForce(target * ( 7.0f + strenght / 10.0f ), ForceMode.VelocityChange );

        var scaleMod = UnityEngine.Random.value * (maxScale - minScale) + minScale;
        ball.transform.localScale = new Vector3(scaleMod, scaleMod, scaleMod);

        balls.Add(Time.time, ball);
    }
}
