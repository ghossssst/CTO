using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PerlinNoiseMap : MonoBehaviour
{
    // Start is called before the first frame update
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    public GameObject prefab_sea;
    public GameObject prefab_grassland;
    public GameObject prefab_forest;
    public GameObject prefab_mountain;
     

    public int map_x = 32;
    public int map_y = 18;

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    public float magnification = 7.0f;

    public int x_offset;
    public int y_offset;

    void Start()
    {
        CreateTileset();
        createTileGroups();
        GenerateMap();
    }


    void CreateTileset()
    {
        tileset = new Dictionary<int, GameObject>();
        tileset.Add(0, prefab_sea);
        tileset.Add(1, prefab_grassland);
        tileset.Add(2, prefab_forest);
        tileset.Add(3, prefab_mountain);
    }

    void createTileGroups()
    {
        tile_groups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> prefab_pair in tileset)
        {
                GameObject tile_group = new GameObject(prefab_pair.Value.name);
                tile_group.transform.parent = gameObject.transform;
                tile_group.transform.localPosition = new Vector3(0, 0, 0);
                tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }

    void GenerateMap()
    {
        for(int x = 0; x < map_x; x++)
        {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());

            for(int y = 0; y < map_y; y++)
            {
                int tile_id = GetIdUsingPerlin(x,y);
                noise_grid[x].Add(tile_id);
                CreateTile(tile_id, x, y);
            }
        }
    }

    int GetIdUsingPerlin(int x, int y)
    {
        //int x_offset = Random.Range(0, 10000);
        //int y_offset = Random.Range(0, 10000);    

        float raw_perlin = Mathf.PerlinNoise((x - x_offset) / magnification, (y - y_offset) / magnification);
        float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f , 1.0f);
        float scale_perlin = clamp_perlin * tileset.Count;
        if(scale_perlin == 4)
        {
            scale_perlin = 3;
        }
        return Mathf.FloorToInt(scale_perlin);
    }

    void CreateTile(int tile_id, int x, int y)
    {
        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);

        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);

        tile_grid[x].Add(tile);
    }
}
