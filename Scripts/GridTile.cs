using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GridTile", order = 1)]
public class GridTile : ScriptableObject
{
    public Tile tile;
    public float obstructionMultiplier; //0-1, 0 no cost, 1 unwalkable
}
