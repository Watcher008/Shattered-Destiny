using System.Collections.Generic;
using UnityEngine;

namespace SD.PathingSystem
{
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public static Pathfinding instance { get; private set; }

        private Grid<WorldMapNode> grid;
        private List<WorldMapNode> openList; //nodes to search
        private List<WorldMapNode> closedList; //already searched

        public Pathfinding(Vector3 offset, int width, int height, float cellSize)
        {
            instance = this;

            var origin = offset;
            origin.x -= width / 2f * cellSize;
            origin.y -= height / 2f * cellSize;
            grid = new Grid<WorldMapNode>(width, height, cellSize, origin, (Grid<WorldMapNode> g, int x, int y) 
                => new WorldMapNode(g, x, y, (Vector2)origin + new Vector2(x * cellSize, y * cellSize)));

            var go = new GameObject("Origin");
            go.transform.position = origin;
        }

        //Converts a list of nodes into a list of Vector3's
        public List<Vector3> FindVectorPath(Vector3 startWorldPosition, Vector3 endWorldPosition, bool ignoreEndNode = false)
        {
            grid.GetXY(startWorldPosition, out int startX, out int startY);
            grid.GetXY(endWorldPosition, out int endX, out int endY);

            List<WorldMapNode> path = FindNodePath(startX, startY, endX, endY, ignoreEndNode);
            if (path == null) return null;
            else
            {
                List<Vector3> vectorPath = new List<Vector3>();
                foreach (WorldMapNode node in path)
                {
                    vectorPath.Add(grid.GetNodePosition(node.x, node.y));
                }
                return vectorPath;
            }
        }

        //Returns a list of nodes that can be travelled to reach a target destination
        public List<WorldMapNode> FindNodePath(int startX, int startY, int endX, int endY, bool ignoreEndNode = false)
        {
            WorldMapNode startNode = grid.GetGridObject(startX, startY);
            //Debug.Log("Start: " + startNode.x + "," + startNode.y);
            WorldMapNode endNode = grid.GetGridObject(endX, endY);
            //Debug.Log("End: " + endNode.x + "," + endNode.y);

            openList = new List<WorldMapNode> { startNode };
            closedList = new List<WorldMapNode>();

            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    WorldMapNode pathNode = grid.GetGridObject(x, y);
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                WorldMapNode currentNode = GetLowestFCostNode(openList);

                if (currentNode == endNode)
                {
                    //Reached final node
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (WorldMapNode neighbour in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbour)) continue;

                    //if the neighbor is the endNode and choosing to ignore whether it is walkable, add it to the closed list
                    if (neighbour == endNode && ignoreEndNode)
                    {
                        //Do nothing here, bypass the next if statement
                        //Debug.Log("Ignoring End Node");
                    }
                    else if (!neighbour.isWalkable || neighbour.isOccupied)
                    {
                        //Debug.Log("Removing unwalkable/occupied tile " + neighbour.x + "," + neighbour.y);
                        closedList.Add(neighbour);
                        continue;
                    }

                    //Adding in movement cost here of the neighbor node to account for areas that are more difficult to move through
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour) + neighbour.movementCost;

                    if (tentativeGCost < neighbour.gCost)
                    {
                        //If it's lower than the cost previously stored on the neightbor, update it
                        neighbour.cameFromNode = currentNode;
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = CalculateDistanceCost(neighbour, endNode);
                        neighbour.CalculateFCost();

                        if (!openList.Contains(neighbour)) openList.Add(neighbour);
                    }
                }
            }

            //Out of nodes on the openList
            Debug.Log("Path could not be found");
            return null;
        }

        //Return a list of all neighbors, up/down/left/right
        private List<WorldMapNode> GetNeighbourList(WorldMapNode currentNode)
        {
            //I could also just cache this...
            List<WorldMapNode> neighborList = new List<WorldMapNode>();

            //North
            if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x, currentNode.y + 1));
            //South
            if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x, currentNode.y - 1));
            //Left
            if (currentNode.x - 1 >= 0) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y));
            //Right
            if (currentNode.x + 1 < grid.GetWidth()) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y));

            if (currentNode.x - 1 >= 0)
            {
                //Left Down
                if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
                //Left Up
                if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
            }
            if (currentNode.x + 1 < grid.GetWidth())
            {
                //Right Down
                if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
                //Right Up
                if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
            }

            return neighborList;
        }

        private List<WorldMapNode> CalculatePath(WorldMapNode endNode)
        {
            List<WorldMapNode> path = new List<WorldMapNode>();
            path.Add(endNode);
            WorldMapNode currentNode = endNode;
            while (currentNode.cameFromNode != null)
            {
                //Start at the end and work backwards
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculatePathMoveCost(List<WorldMapNode> nodes)
        {
            int cost = 0;
            //Skip the first node in the list, this is where the character is
            for (int i = 1; i < nodes.Count; i++)
            {
                cost += nodes[i].movementCost + 1;
            }
            return cost;
        }

        private int CalculateDistanceCost(WorldMapNode a, WorldMapNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private WorldMapNode GetLowestFCostNode(List<WorldMapNode> pathNodeList)
        {
            WorldMapNode lowestFCostNode = pathNodeList[0];

            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].fCost < lowestFCostNode.fCost)
                    lowestFCostNode = pathNodeList[i];
            }

            return lowestFCostNode;
        }

        public WorldMapNode GetNode(int x, int y)
        {
            return grid.GetGridObject(x, y);
        }

        public WorldMapNode GetNode(Vector3 worldPosition)
        {
            return grid.GetGridObject(worldPosition);
        }

        public Vector3 GetNodePosition(int x, int y)
        {
            return grid.GetNodePosition(x, y);
        }

        public void TrySetWaterNode(int x, int y)
        {
            if (GetNode(x,y) != null)
            {
                Debug.Log("Water node located at " + grid.GetNodePosition(x, y));
            }
            else
            {
                //Debug.Log("No Node located at " + grid.GetNodePosition(x, y));
            }
        }

        public void SetNodeTerrain(Vector3 worldPosition, TerrainType terrain)
        {
            var node = GetNode(worldPosition);
            if (node != null) node.SetTerrain(terrain);
            else Debug.LogWarning("Node not found at " + worldPosition);
        }

        public float GetCellSize()
        {
            return grid.GetCellSize();
        }
    }
}