using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NonUniformSoundSource : MonoBehaviour {

    public float VolumeFactor = 1;

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("PaperPlane");
    }

	void Update()
    {
        var delta = player.transform.position.z - transform.position.z;

        // Volume curve is different if the player sees the object or not
        float distFactor = delta > 0 ? 0.001f : 0.1f;

        GetComponent<AudioSource>().volume = Mathf.Exp(-distFactor * delta * delta) * VolumeFactor;
    }
}
