using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public GridTile[] tileTypes;
    [SerializeField] MapManager mapManager;
    public int[,] graph;
    public int sizeX, sizeY;
    public int seed;
    System.Random rng;
    [HideInInspector] public Tilemap tm;
    
    public void generateMap()
    {
        seed = rng.Next();
        mapManager.Reset();
        
        transform.localScale = new Vector3((float)10 / sizeX, (float)10 / sizeY, 0);
        graph = new int[sizeX, sizeY];
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (rng.NextDouble() < 0.6)
                {
                    graph[x, y] = 0;
                    tm.SetTile(new Vector3Int(x,y), tileTypes[0].tile);
                }
                else 
                {
                    graph[x, y] = 1;
                    tm.SetTile(new Vector3Int(x,y), tileTypes[1].tile);
                }
            }
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        tm = GetComponent<Tilemap>();
        rng = new System.Random(seed);
        generateMap();
    }
}
