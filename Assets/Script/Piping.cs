using UnityEngine;

public class Piping : MonoBehaviour
{
    public GameObject MainPipeSection;
    public GameObject SecondaryPipeSection;
    public GameObject Sprinkler;
    public GameObject Puddle;

    public float MainPipeLength;
    public float MainPipeSectionSize;
    public uint MinDistBetweenSecondaryPipes;
    public float SecondaryPipeProbability;
    public float SecondaryPipeSize;
    public float PuddleHeight;

	private void Start()
    {
        int i = 0;
        for (float currPosZ = 0; currPosZ < MainPipeLength; currPosZ += MainPipeSectionSize, ++i)
        {
            CreatePipeObject(MainPipeSection, currPosZ);

            if (i % MinDistBetweenSecondaryPipes == 0 && Random.value < SecondaryPipeProbability)
            {
                // Create secondary pipe
                var obj = CreatePipeObject(SecondaryPipeSection, currPosZ);
                obj.transform.localScale = new Vector3(1, 1, SecondaryPipeSize);
                
                // Create spinkler
                obj = CreatePipeObject(Sprinkler, currPosZ);
                var pos = obj.transform.position;
                pos.x = Random.value * SecondaryPipeSize - SecondaryPipeSize / 2;
                obj.transform.position = pos;

                // Create puddle
                obj = CreatePipeObject(Puddle, currPosZ);
                obj.transform.Rotate(Vector3.up, 360 * Random.value);
                pos = obj.transform.position;
                pos.y = PuddleHeight;
                obj.transform.position = pos;
            }
        }
	}

    private GameObject CreatePipeObject(GameObject prefab, float posZ)
    {
        var obj = Instantiate(prefab);
        var pos = transform.position;
        pos.z = -posZ;
        obj.transform.position = pos;
        obj.transform.parent = transform;
        return obj;
    }
}
