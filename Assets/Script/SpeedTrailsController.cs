using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedTrailsController : MonoBehaviour {

    public GameObject trailPrefab;
    public int trailCount;
    public float upperRadius;
    public float innerRadius;
    public float trailWidth;
    
    private List<TrailRenderer> trails;
    private bool areTrailsEnabled;

    // Use this for initialization
    void Start()
    {
        trails = new List<TrailRenderer>(trailCount);
        areTrailsEnabled = false;

        for (int i = 0; i < trailCount; ++i)
        {
            var trailSpawnPoint = Instantiate(trailPrefab);
            trailSpawnPoint.transform.parent = transform;
            TrailRenderer trail = trailSpawnPoint.GetComponent<TrailRenderer>();
            trail.startWidth = trailWidth;
            trail.endWidth = trailWidth;
            trail.enabled = false;
            trail.time = 0.5f;
            trails.Add(trail);
        }
    }

    public void StartTrails()
    {
        if (areTrailsEnabled)
        {
            return;
        }
        areTrailsEnabled = true;

        for (int i = 0; i < trailCount; ++i)
        {
            trails[i].enabled = true;
            Vector2 unitPoint = Random.insideUnitCircle;
            float radius = Random.value * (upperRadius - innerRadius) + innerRadius;
            trails[i].transform.position = transform.position + new Vector3(unitPoint.x * radius, unitPoint.y * radius, -1);
        }
    }

    public void StopTrails()
    {
        if (!areTrailsEnabled)
        {
            return;
        }
        areTrailsEnabled = false;

        trails.ForEach(trail => { trail.enabled = false; trail.Clear(); });
    }
}
