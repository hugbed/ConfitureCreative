using System.Collections.Generic;
using UnityEngine;

public sealed class BallShooter : MonoBehaviour
{
    public GameObject Target;

    public GameObject Ball;

    [Range(1, 50)]
    public uint NumBallsPerShot;

    [Range(1, 50)]
    public float ShootingDistance;

    [Range(0, 20)]
    public float ShotStrength;

    [Range(0, 10)]
    public float SpawnSphereRadius;

    [Range(0, 10)]
    public float MinBallScale;

    [Range(0, 10)]
    public float MaxBallScale;

    [Range(0, 4)]
    public float PredictionTimeFactor;

    [Range(0, 10)]
    public float CorridorMidHeight;

    [Range(0, 1)]
    public float BaseTrajectoryHeight;

    [Range(0, 1f)]
    public float TrajectoryHeightFactor;

    private List<GameObject> balls;

    private void Start()
    {
        balls = new List<GameObject>((int)NumBallsPerShot);

        // Create balls needed for one shot
        for (int i = 0; i < NumBallsPerShot; ++i)
        {
            var ball = Instantiate(Ball);
            ball.SetActive(false);
            ball.transform.parent = transform;
            balls.Add(ball);
        }
    }

    private void Update()
    {
        // Calculate predicted target position
        if (Target == null)
        {
            Target = GameObject.Find("PaperPlane");
        }


        var predictedTargetPos = Target.transform.position;
        predictedTargetPos += Target.GetComponent<Rigidbody>().velocity * PredictionTimeFactor;
        
        // Don't shoot until predicted target position is in shooting distance on z axis
        if (Mathf.Abs((predictedTargetPos.z - transform.position.z) - ShootingDistance) > 1)
        {
            return;
        }

        foreach (var ball in balls)
        {
            // Set initial ball position
            ball.transform.position = transform.position + Random.insideUnitSphere * SpawnSphereRadius;
            ball.SetActive(true);

            // Set ball scale
            ball.transform.localScale = (Random.value * (MaxBallScale - MinBallScale) + MinBallScale) * Vector3.one;

            // Calculate force vector
            var forceDir = transform.forward;
            forceDir.y = BaseTrajectoryHeight + TrajectoryHeightFactor * predictedTargetPos.y;
                
            forceDir *= ShotStrength;

            // Give force to ball
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().AddForce(forceDir, ForceMode.Impulse);
        }
    }
}
