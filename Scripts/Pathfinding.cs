using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pathfinding
{
    private MapGenerator mapGenerator;
    private static Vector3Int upLeft = new Vector3Int(-1, 1, 0);
    private static Vector3Int upRight = new Vector3Int(1, 1, 0);
    private static Vector3Int downLeft = new Vector3Int(-1, -1, 0);
    private static Vector3Int downRight = new Vector3Int(1, -1, 0);
    private int[,] graph;
    
    public Pathfinding(MapGenerator mapGenerator)
    {
        this.mapGenerator = mapGenerator;
        graph = mapGenerator.graph;
    }

    //TODO: Fix node costs not updating, cost calculation issues
    public IEnumerator AStar(Vector3Int start, Vector3Int end, MapManager mapManager)
    {
        List<Vector3Int> finalNodes = new List<Vector3Int>();
        PriorityQueue<AStarNode, double> unvisitedNodes = new PriorityQueue<AStarNode, double>();
        HashSet<Vector3Int> visitedNodes = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, AStarNode> nodes = new Dictionary<Vector3Int, AStarNode>();
        
        for (int x = 0; x < graph.GetLength(0); x++)
        {
            for (int y = 0; y < graph.GetLength(1); y++)
            {
                if(graph[x,y] == 1) continue;
                nodes[new Vector3Int(x, y)] = new AStarNode(new Vector3Int(x, y), null,  Double.MaxValue, end); 
            }
        }
        
        AStarNode currentNode = new AStarNode(start, null, 0, end);
        nodes[currentNode.cell] = currentNode;
        unvisitedNodes.Enqueue(currentNode, 0);
        do
        {
            currentNode = unvisitedNodes.Dequeue();
            visitedNodes.Add(currentNode.cell);
            
            //cr stuff
            mapManager.visitedTiles.Add(currentNode.cell);
            if(currentNode.cell != end && currentNode.cell != start) mapGenerator.tm.SetTile(currentNode.cell, mapManager.visitTile);
            
            yield return new WaitForSeconds(0.01f);
            
            Vector3Int up = currentNode.cell + Vector3Int.up;
            Vector3Int down = currentNode.cell + Vector3Int.down;
            Vector3Int left = currentNode.cell + Vector3Int.left;
            Vector3Int right = currentNode.cell + Vector3Int.right;
            Vector3Int topLeft = currentNode.cell + upLeft;
            Vector3Int topRight = currentNode.cell + upRight;
            Vector3Int bottomLeft = currentNode.cell + downLeft;
            Vector3Int bottomRight = currentNode.cell + downRight;

            if (InBounds(up) && !visitedNodes.Contains(up))
            {
                double tempDistance = nodes[currentNode.cell].GCost + 1;
                if (tempDistance < nodes[up].GCost)
                {
                    nodes[up].GCost = tempDistance;
                    unvisitedNodes.Enqueue(nodes[up], tempDistance + nodes[up].HCost);
                    if(up != end && up != start) mapGenerator.tm.SetTile(up, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(up);
                    nodes[up].previousNode = currentNode;
                }
            }

            if (InBounds(down) && !visitedNodes.Contains(down))
            {
                double tempDistance = nodes[currentNode.cell].GCost + 1;
                if (tempDistance < nodes[down].GCost)
                {
                    nodes[down].GCost = tempDistance;
                    unvisitedNodes.Enqueue(nodes[down], tempDistance + nodes[down].HCost);
                    if(down != end && down != start) mapGenerator.tm.SetTile(down, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(down);
                    nodes[down].previousNode = currentNode;
                }
            }

            if (InBounds(left) && !visitedNodes.Contains(left))
            {
                double tempDistance = nodes[currentNode.cell].GCost + 1;
                if (tempDistance < nodes[left].GCost)
                {
                    nodes[left].GCost = tempDistance;
                    unvisitedNodes.Enqueue(nodes[left], tempDistance + nodes[left].HCost);
                    if(left != end && left != start) mapGenerator.tm.SetTile(left, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(left);
                    nodes[left].previousNode = currentNode;
                }
            }

            if (InBounds(right) && !visitedNodes.Contains(right))
            {
                double tempDistance = nodes[currentNode.cell].GCost + 1;
                if (tempDistance < nodes[right].GCost)
                {
                    nodes[right].GCost = tempDistance;
                    unvisitedNodes.Enqueue(nodes[right], tempDistance + nodes[right].HCost);
                    if(right != end && right != start) mapGenerator.tm.SetTile(right, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(right);
                    nodes[right].previousNode = currentNode;
                }
            }

            if (InBounds(topLeft) && !visitedNodes.Contains(topLeft))
            {
                double tempDistance = nodes[currentNode.cell].GCost + 1.414;
                if (tempDistance < nodes[topLeft].GCost)
                {
                    nodes[topLeft].GCost = tempDistance;
                    unvisitedNodes.Enqueue(nodes[topLeft], tempDistance + nodes[topLeft].HCost);
                    if(topLeft != end && topLeft != start) mapGenerator.tm.SetTile(topLeft, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(topLeft);
                    nodes[topLeft].previousNode = currentNode;
                }
            }

            if (InBounds(topRight) && !visitedNodes.Contains(topRight))
            {
                double tempDistance = nodes[currentNode.cell].GCost + 1.414;
                if (tempDistance < nodes[topRight].GCost)
                {
                    nodes[topRight].GCost = tempDistance;
                    unvisitedNodes.Enqueue(nodes[topRight], tempDistance + nodes[topRight].HCost);
                    if(topRight != end && topRight != start) mapGenerator.tm.SetTile(topRight, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(topRight);
                    nodes[topRight].previousNode = currentNode;
                }
            }

            if (InBounds(bottomLeft) && !visitedNodes.Contains(bottomLeft))
            {
                double tempDistance = nodes[currentNode.cell].GCost + 1.414;
                if (tempDistance < nodes[bottomLeft].GCost)
                {
                    nodes[bottomLeft].GCost = tempDistance;
                    unvisitedNodes.Enqueue(nodes[bottomLeft], tempDistance + nodes[bottomLeft].HCost);
                    if(bottomLeft != end && bottomLeft != start) mapGenerator.tm.SetTile(bottomLeft, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(bottomLeft);
                    nodes[bottomLeft].previousNode = currentNode;
                }
            }
        
            if (InBounds(bottomRight) && !visitedNodes.Contains(bottomRight))
            {
                double tempDistance = nodes[currentNode.cell].GCost + 1.414;
                if (tempDistance < nodes[bottomRight].GCost)
                {
                    nodes[bottomRight].GCost = tempDistance;
                    unvisitedNodes.Enqueue(nodes[bottomRight], tempDistance + nodes[bottomRight].HCost);
                    if(bottomRight != end && bottomRight != start) mapGenerator.tm.SetTile(bottomRight, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(bottomRight);
                    nodes[bottomRight].previousNode = currentNode;
                }
            }
            
        } 
        while (unvisitedNodes.Count > 0 && !currentNode.cell.Equals(end));
        
        AStarNode final = nodes[currentNode.cell].previousNode;
        while (!final.cell.Equals(start))
        {
            finalNodes.Add(final.cell);
            final = nodes[final.cell].previousNode;
        }
        
        //cr stuff
        mapManager.pathTiles = finalNodes;
        mapManager.SetPathTiles();

    }

    public class AStarNode
    {
        public Vector3Int cell;
        public AStarNode previousNode;
        //use distance squared
        public double GCost;
        public double HCost;
        
        public AStarNode(Vector3Int cell, AStarNode previousNode, double GCost, Vector3Int endCell)
        {
            this.cell = cell;
            this.previousNode = previousNode;
            /*GCost = (cell.x - startCell.x) * (cell.x - startCell.x) +
                    (cell.y - startCell.y) * (cell.y - startCell.y);
            HCost = (cell.x - endCell.x) * (cell.x - endCell.x) +
                    (cell.y - endCell.y) * (cell.y - endCell.y);*/
            this.GCost = GCost;
            HCost = Vector3Int.Distance(cell, endCell);
        }
        
    }

    public IEnumerator Dijkstras(Vector3Int start, Vector3Int end, MapManager mapManager)
    {
        List<Vector3Int> finalNodes = new List<Vector3Int>();
        PriorityQueue<Vector3Int, float> unvisitedNodes = new PriorityQueue<Vector3Int, float>();
        HashSet<Vector3Int> visitedNodes = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, DijkstraNode> nodeDistances = new Dictionary<Vector3Int, DijkstraNode>();
        for (int x = 0; x < graph.GetLength(0); x++)
        {
            for (int y = 0; y < graph.GetLength(1); y++)
            {
                if(graph[x,y] == 1) continue;
                nodeDistances[new Vector3Int(x, y)] = new DijkstraNode(new Vector3Int(-1, -1, -1) ,1000000); 
            }
        }

        Vector3Int currentNode = start;
        nodeDistances[currentNode].distance = 0;
        unvisitedNodes.Enqueue(currentNode,0);
        do
        {
            Vector3Int minNode = unvisitedNodes.Dequeue();
            
            //cr stuff
            mapManager.visitedTiles.Add(minNode);
            if(minNode != end && minNode != start) mapGenerator.tm.SetTile(minNode, mapManager.visitTile);
            
            visitedNodes.Add(currentNode);
            currentNode = minNode;

            yield return new WaitForSeconds(0.01f);
            
            Vector3Int up = currentNode + Vector3Int.up;
            if (InBounds(up) && !visitedNodes.Contains(up))
            {
                float tempDistance = nodeDistances[currentNode].distance + 1;
                if (tempDistance < nodeDistances[up].distance)
                {
                    nodeDistances[up].distance = tempDistance;
                    unvisitedNodes.Enqueue(up, tempDistance);
                    if(up != end && up != start) mapGenerator.tm.SetTile(up, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(up);
                    nodeDistances[up].previousNode = currentNode;
                }
            }

            Vector3Int down = currentNode + Vector3Int.down;
            if (InBounds(down) && !visitedNodes.Contains(down))
            {
                float tempDistance = nodeDistances[currentNode].distance + 1;
                if (tempDistance < nodeDistances[down].distance)
                {
                    nodeDistances[down].distance = tempDistance;
                    unvisitedNodes.Enqueue(down, tempDistance);
                    if(down != end && down != start) mapGenerator.tm.SetTile(down, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(down);
                    nodeDistances[down].previousNode = currentNode;
                }
            }

            Vector3Int left = currentNode + Vector3Int.left;
            if (InBounds(left) && !visitedNodes.Contains(left))
            {
                float tempDistance = nodeDistances[currentNode].distance + 1;
                if (tempDistance < nodeDistances[left].distance)
                {
                    nodeDistances[left].distance = tempDistance;
                    unvisitedNodes.Enqueue(left, tempDistance);
                    if(left != end && left != start) mapGenerator.tm.SetTile(left, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(left);
                    nodeDistances[left].previousNode = currentNode;
                }
            }
            Vector3Int right = currentNode + Vector3Int.right;
            if (InBounds(right) && !visitedNodes.Contains(right))
            {
                float tempDistance = nodeDistances[currentNode].distance + 1;
                if (tempDistance < nodeDistances[right].distance)
                {
                    nodeDistances[right].distance = tempDistance;
                    unvisitedNodes.Enqueue(right, tempDistance);
                    if(right != end && right != start) mapGenerator.tm.SetTile(right, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(right);
                    nodeDistances[right].previousNode = currentNode;
                }
            }
            Vector3Int topLeft = currentNode + upLeft;
            if (InBounds(topLeft) && !visitedNodes.Contains(topLeft))
            {
                float tempDistance = nodeDistances[currentNode].distance + 1.4f;
                if (tempDistance < nodeDistances[topLeft].distance)
                {
                    nodeDistances[topLeft].distance = tempDistance;
                    unvisitedNodes.Enqueue(topLeft, tempDistance);
                    if(topLeft != end && topLeft != start) mapGenerator.tm.SetTile(topLeft, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(topLeft);
                    nodeDistances[topLeft].previousNode = currentNode;
                }
            }
            Vector3Int topRight = currentNode + upRight;
            if (InBounds(topRight) && !visitedNodes.Contains(topRight))
            {
                float tempDistance = nodeDistances[currentNode].distance + 1.4f;
                if (tempDistance < nodeDistances[topRight].distance)
                {
                    nodeDistances[topRight].distance = tempDistance;
                    unvisitedNodes.Enqueue(topRight, tempDistance);
                    if(topRight != end && topRight != start) mapGenerator.tm.SetTile(topRight, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(topRight);
                    nodeDistances[topRight].previousNode = currentNode;
                }
            }
            Vector3Int bottomLeft = currentNode + downLeft;
            if (InBounds(bottomLeft) && !visitedNodes.Contains(bottomLeft))
            {
                float tempDistance = nodeDistances[currentNode].distance + 1.4f;
                if (tempDistance < nodeDistances[bottomLeft].distance)
                {
                    nodeDistances[bottomLeft].distance = tempDistance;
                    unvisitedNodes.Enqueue(bottomLeft, tempDistance);
                    if(bottomLeft != end && bottomLeft != start) mapGenerator.tm.SetTile(bottomLeft, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(bottomLeft);
                    nodeDistances[bottomLeft].previousNode = currentNode;
                }
            }
            Vector3Int bottomRight = currentNode + downRight;
            if (InBounds(bottomRight) && !visitedNodes.Contains(bottomRight))
            {
                float tempDistance = nodeDistances[currentNode].distance + 1.4f;
                if (tempDistance < nodeDistances[bottomRight].distance)
                {
                    nodeDistances[bottomRight].distance = tempDistance;
                    unvisitedNodes.Enqueue(bottomRight, tempDistance);
                    if(bottomRight != end && bottomRight != start) mapGenerator.tm.SetTile(bottomRight, mapManager.unvisitedTile);
                    mapManager.visitedTiles.Add(bottomRight);
                    nodeDistances[bottomRight].previousNode = currentNode;
                }
            }
        } while (unvisitedNodes.Count > 0 && !currentNode.Equals(end));

        Vector3Int final = nodeDistances[currentNode].previousNode;
        while (!final.Equals(start))
        {
            finalNodes.Add(final);
            final = nodeDistances[final].previousNode;
        }
        
        //cr stuff
        mapManager.pathTiles = finalNodes;
        mapManager.SetPathTiles();
    }
    

    class DijkstraNode
    {
        public Vector3Int previousNode;
        public float distance;

        public DijkstraNode(Vector3Int previousNode, float distance)
        {
            this.previousNode = previousNode;
            this.distance = distance;
        }
    }
    
    bool InBounds(Vector3Int position)
    {
        if (position.x < mapGenerator.sizeX && position.y < mapGenerator.sizeY && position.x >= 0 && position.y >= 0
            && graph[position.x, position.y] == 0)
            return true;
        return false;
    }
}
