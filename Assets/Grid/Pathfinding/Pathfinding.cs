using System.Collections.Generic;
using UnityEngine;

namespace SD.PathingSystem
{
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public static Pathfinding instance { get; private set; }

        private Grid<WorldNode> grid;
        private List<WorldNode> openList; //nodes to search
        private List<WorldNode> closedList; //already searched

        public Pathfinding(int width, int height, float cellSize)
        {
            instance = this;

            var origin = Vector2.zero;
            origin.x -= width / 2f * cellSize;
            origin.y -= height / 2f * cellSize;
            grid = new Grid<WorldNode>(width, height, cellSize, origin, (Grid<WorldNode> g, int x, int y) 
                => new WorldNode(g, x, y, NodeGlobal(origin, cellSize,x,y)));

            var go = new GameObject("Origin");
            go.transform.position = origin;
        }

        private Vector2 NodeGlobal(Vector3 origin, float cellSize, int x, int y)
        {
            return (Vector2)origin + new Vector2(x * cellSize, y * cellSize);
        }

        //Converts a list of nodes into a list of Vector3's
        public List<Vector3> FindVectorPath(Vector3 startWorldPosition, Vector3 endWorldPosition, bool ignoreEndNode = false)
        {
            grid.GetXY(startWorldPosition, out int startX, out int startY);
            grid.GetXY(endWorldPosition, out int endX, out int endY);

            List<WorldNode> path = FindNodePath(startX, startY, endX, endY, ignoreEndNode);
            if (path == null) return null;
            else
            {
                List<Vector3> vectorPath = new List<Vector3>();
                foreach (WorldNode node in path)
                {
                    vectorPath.Add(grid.GetNodePosition(node.x, node.y));
                }
                return vectorPath;
            }
        }

        //Returns a list of nodes that can be travelled to reach a target destination
        //*** I need to add the optimizations from Sebastian Lague's Heap 
        public List<WorldNode> FindNodePath(int startX, int startY, int endX, int endY, bool ignoreEndNode = false)
        {
            WorldNode startNode = grid.GetGridObject(startX, startY);
            //Debug.Log("Start: " + startNode.x + "," + startNode.y);
            WorldNode endNode = grid.GetGridObject(endX, endY);
            //Debug.Log("End: " + endNode.x + "," + endNode.y);

            openList = new List<WorldNode> { startNode };
            closedList = new List<WorldNode>();

            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    WorldNode pathNode = grid.GetGridObject(x, y);
                    pathNode.gCost = int.MaxValue;
                    pathNode.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);

            while (openList.Count > 0)
            {
                WorldNode currentNode = GetLowestFCostNode(openList);

                if (currentNode == endNode)
                {
                    //Reached final node
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (WorldNode neighbour in GetNeighbourList(currentNode))
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

                        if (!openList.Contains(neighbour)) openList.Add(neighbour);
                    }
                }
            }

            //Out of nodes on the openList
            Debug.Log("Path could not be found");
            return null;
        }

        //Return a list of all neighbors, up/down/left/right
        private List<WorldNode> GetNeighbourList(WorldNode currentNode)
        {
            //I could also just cache this...
            List<WorldNode> neighborList = new List<WorldNode>();

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

        private List<WorldNode> CalculatePath(WorldNode endNode)
        {
            List<WorldNode> path = new List<WorldNode>
            {
                endNode
            };
            WorldNode currentNode = endNode;
            while (currentNode.cameFromNode != null)
            {
                //Start at the end and work backwards
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculatePathMoveCost(List<WorldNode> nodes)
        {
            int cost = 0;
            //Skip the first node in the list, this is where the character is
            for (int i = 1; i < nodes.Count; i++)
            {
                cost += nodes[i].movementCost + 1;
            }
            return cost;
        }

        private int CalculateDistanceCost(WorldNode a, WorldNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private WorldNode GetLowestFCostNode(List<WorldNode> pathNodeList)
        {
            WorldNode lowestFCostNode = pathNodeList[0];

            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].fCost < lowestFCostNode.fCost)
                    lowestFCostNode = pathNodeList[i];
            }

            return lowestFCostNode;
        }

        public WorldNode GetNode(int x, int y)
        {
            return grid.GetGridObject(x, y);
        }

        public WorldNode GetNode(Vector3 worldPosition)
        {
            return grid.GetGridObject(worldPosition);
        }

        public Vector3 GetNodeCenter(int x, int y)
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

        public static bool PositionIsValid(int x, int y)
        {
            return instance.NodeIsValid(x, y);
        }

        private bool NodeIsValid(int x, int y)
        {
            return grid.GetGridObject(x, y) != null;
        }

        public static bool PositionIsValid(Vector3 worldPosition)
        {
            return instance.NodeIsValid(worldPosition);
        }

        private bool NodeIsValid(Vector3 worldPosition)
        {
            return grid.GetGridObject(worldPosition) != null;
        }

        public int GetNodeDistance_Path(WorldNode fromNode, WorldNode toNode)
        {
            int x = Mathf.Abs(fromNode.x - toNode.x);
            int y = Mathf.Abs(fromNode.y - toNode.y);
            return x + y;
        }

        public float GetNodeDistance_Straight(WorldNode fromNode, WorldNode toNode)
        {

            return Mathf.Sqrt(Mathf.Pow(fromNode.x - toNode.x, 2) + Mathf.Pow(fromNode.y - toNode.y, 2));
        }

        public List<WorldNode> GetNodesInRange_Circle(int x, int y, int range)
        {
            var fromNode = grid.GetGridObject(x, y);
            return GetNodesInRange_Circle(fromNode, range);
        }

        public List<WorldNode> GetNodesInRange_Circle(WorldNode fromNode, int range)
        {
            var nodes = new List<WorldNode>();

            for (int x = fromNode.x - range; x < fromNode.x + range + 1; x++)
            {
                for (int y = fromNode.y - range; y < fromNode.y + range + 1; y++)
                {
                    if (x < 0 || x > grid.GetWidth() - 1) continue;
                    if (y < 0 || y > grid.GetHeight() - 1) continue;

                    var toNode = grid.GetGridObject(x, y);
                    if (GetNodeDistance_Straight(fromNode, toNode) <= range) nodes.Add(toNode);
                }
            }
            return nodes;
        }

        public List<WorldNode> GetNodesInRange_Square(int x, int y, int range)
        {
            var fromNode = grid.GetGridObject(x, y);
            return GetNodesInRange_Square(fromNode, range);
        }

        public List<WorldNode> GetNodesInRange_Square(WorldNode fromNode, int range)
        {
            var nodes = new List<WorldNode>();

            for (int x = fromNode.x - range; x < fromNode.x + range + 1; x++)
            {
                for (int y = fromNode.y - range; y < fromNode.y + range + 1; y++)
                {
                    if (x < 0 || x > grid.GetWidth() - 1) continue;
                    if (y < 0 || y > grid.GetHeight() - 1) continue;

                    var toNode = grid.GetGridObject(x, y);
                    nodes.Add(toNode);
                }
            }
            return nodes;
        }
    }
}