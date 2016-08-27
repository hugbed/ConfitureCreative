using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour
{
    private const int WALLS_LAYER = 8;

    public float inputSensitivity = 0.8f;

    public Boundary positionBoundary;
    public float globalSpeed = 10.0f;
    public float gravitySpeed = -0.3f;
    public float inputVerticalSpeed = 0.0f;
    public float defaultPlaneSpeed = 1.0f;
    public float planeSpeedMin = 0.5f;
    public float planeSpeedMax = 2.0f;
    // 0 : raw, 1 : smooth
    public float horizontalMoveSmoothing = 0.05f;
    public Vector3 movementTilt = new Vector3(2.0f, 1.0f, 2.0f);
    public Vector3 externalVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    public Material dry;
    public Material wet;
    public Vector3 smoothedExternalVelocity = Vector3.zero;

    public float minSpeedForTrails;

    public float wetLvl = 0.0f;

    private GameObject gameStateObject;
    private Rigidbody rb;
    private float planeSpeed;
    private Vector3 initPos;
    private float physicalDamage = 0;

    private float prevMoveHorizontal;

    public AudioClip wetDeathSound;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        planeSpeed = defaultPlaneSpeed;
        gameStateObject = GameObject.Find("GameStateObject");
        initPos = rb.position;
    }

    public void Reset()
    {
        GetComponent<SpeedTrailsController>().StopTrails();
        gameObject.transform.position = initPos;
        wetLvl = 0.0f;
        physicalDamage = 0.0f;
        transform.FindChild("Model").gameObject.SetActive(true);
        transform.FindChild("GameOverPlane").gameObject.SetActive(false);

        rb.position = initPos;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // DON'T MOVE IF NOT IN "FLYING" GAME STATE
        if (GameState.instance.currentState != GameStateEnum.Flying)
            return;

        float moveHorizontal = - inputSensitivity * Input.GetAxis("Horizontal");
        moveHorizontal = Mathf.Lerp(moveHorizontal, prevMoveHorizontal, horizontalMoveSmoothing);
        prevMoveHorizontal = moveHorizontal;

        float moveVertical = - inputSensitivity * Input.GetAxis("Vertical");

        // UP
        if (moveVertical > 0)
        {
            moveVertical = 0.0f;
            //planeSpeed -= inputVerticalSpeed;
        }

        // DOWN
        else if (moveVertical < 0)
        {
            planeSpeed += inputVerticalSpeed;
        }

        // NO VERTICAL INPUT
        else
        {
            planeSpeed = Mathf.Clamp(planeSpeed, planeSpeedMin, defaultPlaneSpeed);
        }

        planeSpeed = Mathf.Clamp(planeSpeed, planeSpeedMin, planeSpeedMax);

        Vector3 movement = new Vector3(moveHorizontal, moveVertical + gravitySpeed - wetLvl, -planeSpeed);
        
        smoothedExternalVelocity += (externalVelocity - smoothedExternalVelocity) * 2.0f * Time.deltaTime;
        rb.velocity = globalSpeed * movement + smoothedExternalVelocity;

        if (rb.velocity.magnitude > minSpeedForTrails)
        {
            GetComponent<SpeedTrailsController>().StartTrails();
        }
        else
        {
            GetComponent<SpeedTrailsController>().StopTrails();
        }

        if (rb.position.y <= positionBoundary.yMin)
        {
            if (GameState.instance.currentState != GameStateEnum.Gameover && wetLvl >= 1f)
            {
                GetComponent<AudioSource>().PlayOneShot(wetDeathSound);
            }
            GameState.instance.GameOver();
        }

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, positionBoundary.xMin, positionBoundary.xMax),
            Mathf.Clamp(rb.position.y, positionBoundary.yMin, positionBoundary.yMax),
            rb.position.z
        );
        rb.rotation = Quaternion.Euler(Mathf.Clamp(rb.velocity.y * movementTilt.x, -26, float.PositiveInfinity), rb.velocity.x * -movementTilt.y, rb.velocity.x * -movementTilt.z);
        rb.angularVelocity = Vector3.zero;

    }

    void Update()
    {
        if (wetLvl <= 0.0f)
        {
            wetLvl = 0.0f;
        }
        else
        {
            wetLvl -= Time.deltaTime / 30;
        }

        if (physicalDamage >= 1f)
        {
            transform.FindChild("Model").gameObject.SetActive(false);
            transform.FindChild("GameOverPlane").gameObject.SetActive(true);
            if (GameState.instance.currentState != GameStateEnum.Gameover)
            {
                GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
            }
            
            GameState.instance.GameOver();
        }

        var mat = transform.FindChild("Model").GetComponent<Renderer>().material;
        mat.SetFloat("_Wet", wetLvl);
        mat.SetFloat("_DisplacementFactor", physicalDamage * 3);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Contains("PaperBall"))
        {
            physicalDamage = Mathf.Clamp01(physicalDamage + 0.1f);
        }
        else if (collider.gameObject.name.Contains("CeilingFan") && collider == collider.gameObject.GetComponent<BoxCollider>())
        {
            physicalDamage = 1.0f;
        }

        else if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            physicalDamage = Mathf.Clamp01(physicalDamage + 0.2f);
        }

    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.name.Contains("courantAirChaud"))
        {
            wetLvl -= 0.01f;
        }
        if (collider.gameObject.name.Contains("waterFall"))
        {
            wetLvl += 0.05f;
        }
        if (collider.gameObject.name.Contains("Puddle"))
        {
            wetLvl += 0.04f;
        }

        else if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            physicalDamage = Mathf.Clamp01(physicalDamage + 0.1f);
        }
    }
}