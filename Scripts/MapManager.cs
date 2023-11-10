using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
     bool waitingForStart;
     bool waitingForEnd;
     public Vector3Int startCell;
     public Vector3Int endCell;
     public List<Vector3Int> pathTiles = new ();
     public List<Vector3Int> visitedTiles = new ();

     [SerializeField] TextMeshProUGUI sizeText;
     [SerializeField] TextMeshProUGUI mousePositionText;
     [SerializeField] Slider slider;
     [SerializeField] public MapGenerator mapGenerator;
     [SerializeField] Camera camera;
     [SerializeField] public Tile startTile;
     [SerializeField] Tile endTile;
     [SerializeField] Tile pathTile;
     [SerializeField] public Tile visitTile;
     [SerializeField] public Tile unvisitedTile;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        StopAllCoroutines();
        if (IsValidVector(startCell))
        {
            mapGenerator.tm.SetTile(startCell, mapGenerator.tileTypes[0].tile);
        }
        if (IsValidVector(endCell))
        {
            mapGenerator.tm.SetTile(endCell, mapGenerator.tileTypes[0].tile);
        }
        foreach (var tile in pathTiles)
        {
            mapGenerator.tm.SetTile(tile,mapGenerator.tileTypes[0].tile);
        }
        foreach (var tile in visitedTiles)
        {
            mapGenerator.tm.SetTile(tile,mapGenerator.tileTypes[0].tile);
        }

        pathTiles = new List<Vector3Int>();
        visitedTiles = new List<Vector3Int>();
        
        startCell = new Vector3Int(-1, -1, -1);
        endCell = new Vector3Int(-1, -1, -1);
        waitingForStart = true;
        waitingForEnd = true;
    }

    private void Update()
    {
        CheckClick();
        mousePositionText.text = "Mouse Position: " + mapGenerator.tm.WorldToCell(camera.ScreenToWorldPoint(Input.mousePosition));
    } 

     void CheckClick()
    {
        if (Input.GetMouseButtonDown(0) && waitingForStart)
        {
            Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tempCell = mapGenerator.tm.WorldToCell(mousePosition);
            if (tempCell.x < mapGenerator.sizeX && tempCell.y < mapGenerator.sizeY && tempCell.x >= 0 && tempCell.y >= 0
                && mapGenerator.graph[tempCell.x, tempCell.y] == 0)
            {
                startCell = tempCell;
                mapGenerator.tm.SetTile(startCell, startTile);
                waitingForStart = false;
                waitingForEnd = true;
            }
        }
        else if (Input.GetMouseButtonDown(0) && waitingForEnd)
        {
            Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tempCell = mapGenerator.tm.WorldToCell(mousePosition);
            if (tempCell.x < mapGenerator.sizeX && tempCell.y < mapGenerator.sizeY && tempCell.x >= 0 && tempCell.y >= 0
                && mapGenerator.graph[tempCell.x, tempCell.y] == 0)
            {
                endCell = tempCell;
                mapGenerator.tm.SetTile(endCell, endTile);
                waitingForEnd = false;
            }
        }
    }

     public void DoAStar()
     {
         if (!IsValidVector(startCell) || !IsValidVector(endCell))
         {
             return;
         }
         Pathfinding pathfinding = new Pathfinding(mapGenerator);
         StartCoroutine(pathfinding.AStar(startCell, endCell,this));
         foreach (var tile in pathTiles)
         {
             mapGenerator.tm.SetTile(tile, pathTile);
         }
     }
     
     public void DoDijkstras()
     {
         if (!IsValidVector(startCell) || !IsValidVector(endCell))
         {
             return;
         }
         Pathfinding pathfinding = new Pathfinding(mapGenerator);
         StartCoroutine(pathfinding.Dijkstras(startCell, endCell,this));
         foreach (var tile in pathTiles)
         {
             mapGenerator.tm.SetTile(tile, pathTile);
         }
     }

     public void SetPathTiles()
     {
         foreach (var tile in pathTiles)
         {
             mapGenerator.tm.SetTile(tile, pathTile);
         }
     }

     public void SetMapScale()
     {
         int size = (int) slider.value;

         mapGenerator.sizeX = size;
         mapGenerator.sizeY = size;

         sizeText.text = "Size: " + (int) slider.value + "x" + (int) slider.value;
         
     }

     
    bool IsValidVector(Vector3Int v)
    {
        return !v.Equals(new Vector3Int(-1, -1, -1));
    }
}
