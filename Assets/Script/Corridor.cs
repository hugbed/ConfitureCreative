using UnityEngine;

public class Corridor : MonoBehaviour {

    public GameObject WALL_TILE;
    public GameObject WINDOW_TILE;
    public GameObject FLOOR_TILE;
    public GameObject CEILING_TILE;
    public GameObject BALL_THROWER;

    public uint LengthInTiles;
    public float TileSize;
    public float Width;

    public float WindowProbability;

    void Start()
    {
        CreateWall(true);
        CreateWall(false);
        CreateFloorOrCeiling(true, FLOOR_TILE);
        CreateFloorOrCeiling(false, CEILING_TILE);
    }

    private void CreateFloorOrCeiling(bool IsFloor, GameObject prefab)
    {
        for (int i = 0; i < LengthInTiles; ++i)
        {
            var tile = Instantiate(prefab);
            tile.transform.parent = transform;
            tile.transform.position = new Vector3(0, IsFloor ? 0 : 3 * TileSize, -i * TileSize);
            var scale = tile.transform.localScale;
            scale.y *= Width;
            tile.transform.localScale = scale;
        }
    }

    private void CreateWall(bool isLeftWall)
    {
        float xOffset = isLeftWall ? -Width / 2 : Width / 2;

        for (int i = 0; i < LengthInTiles; ++i)
        {
            var tile = Instantiate(WALL_TILE);
            tile.transform.parent = transform;
            tile.transform.position = new Vector3(xOffset, TileSize / 2, -i * TileSize);

            bool isWindow = Random.value < WindowProbability;
            GameObject toInstantiate = isWindow ? WINDOW_TILE : WALL_TILE;
            tile = Instantiate(toInstantiate);
            tile.transform.parent = transform;
            if (isLeftWall)
            {
                tile.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            tile.transform.position = new Vector3(xOffset, 3 * TileSize / 2, -i * TileSize);
            if (isWindow)
            {
                var thrower = Instantiate(BALL_THROWER);
                thrower.GetComponent<BallThrower>().Player = GameObject.Find("PaperPlane");
                thrower.transform.parent = transform;
                thrower.transform.position = new Vector3(xOffset, 3 * TileSize / 2, -i * TileSize);
            }


            tile = Instantiate(WALL_TILE);
            tile.transform.parent = transform;
            tile.transform.position = new Vector3(xOffset, 5 * TileSize / 2, -i * TileSize);
        }
    }
}
