using System.Collections.Generic;
using UnityEngine;

namespace SD.PathingSystem
{
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public static Pathfinding instance { get; private set; }

        private Grid<PathNode> grid;
        private List<PathNode> openList; //nodes to search
        private List<PathNode> closedList; //already searched

        public Pathfinding(int width, int height, float cellSize)
        {
            instance = this;

            var origin = Vector2.zero;
            origin.x -= width / 2f * cellSize;
            origin.y -= height / 2f * cellSize;
            grid = new Grid<PathNode>(width, height, cellSize, origin, (Grid<PathNode> g, int x, int y) 
                => new PathNode(g, x, y, NodeGlobal(origin, cellSize,x,y)));

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

            List<PathNode> path = FindNodePath(startX, startY, endX, endY, ignoreEndNode);
            if (path == null) return null;
            else
            {
                List<Vector3> vectorPath = new List<Vector3>();
                foreach (PathNode node in path)
                {
                    vectorPath.Add(grid.GetNodePosition(node.X, node.Y));
                }
                return vectorPath;
            }
        }

        //Returns a list of nodes that can be travelled to reach a target destination
        //*** I need to add the optimizations from Sebastian Lague's Heap 
        public List<PathNode> FindNodePath(int startX, int startY, int endX, int endY, bool ignoreEndNode = false)
        {
            PathNode startNode = grid.GetGridObject(startX, startY);
            //Debug.Log("Start: " + startNode.x + "," + startNode.y);
            PathNode endNode = grid.GetGridObject(endX, endY);
            //Debug.Log("End: " + endNode.x + "," + endNode.y);

            openList = new List<PathNode> { startNode };
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    PathNode pathNode = grid.GetGridObject(x, y);
                    pathNode.gCost = int.MaxValue;
                    pathNode.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);

                if (currentNode == endNode)
                {
                    //Reached final node
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighbour in GetNeighbourList(currentNode))
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
                    int tentativeGCost = Mathf.RoundToInt((currentNode.gCost + CalculateDistanceCost(currentNode, neighbour)) * (1 / neighbour.MovementModifier));

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
        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            //I could also just cache this...
            List<PathNode> neighborList = new List<PathNode>();

            //North
            if (currentNode.Y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.X, currentNode.Y + 1));
            //South
            if (currentNode.Y - 1 >= 0) neighborList.Add(GetNode(currentNode.X, currentNode.Y - 1));
            //Left
            if (currentNode.X - 1 >= 0) neighborList.Add(GetNode(currentNode.X - 1, currentNode.Y));
            //Right
            if (currentNode.X + 1 < grid.GetWidth()) neighborList.Add(GetNode(currentNode.X + 1, currentNode.Y));

            if (currentNode.X - 1 >= 0)
            {
                //Left Down
                if (currentNode.Y - 1 >= 0) neighborList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));
                //Left Up
                if (currentNode.Y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
            }
            if (currentNode.X + 1 < grid.GetWidth())
            {
                //Right Down
                if (currentNode.Y - 1 >= 0) neighborList.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));
                //Right Up
                if (currentNode.Y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
            }

            return neighborList;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>
            {
                endNode
            };
            PathNode currentNode = endNode;
            while (currentNode.cameFromNode != null)
            {
                //Start at the end and work backwards
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.X - b.X);
            int yDistance = Mathf.Abs(a.Y - b.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];

            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].fCost < lowestFCostNode.fCost)
                    lowestFCostNode = pathNodeList[i];
            }

            return lowestFCostNode;
        }

        public PathNode GetNode(int x, int y)
        {
            return grid.GetGridObject(x, y);
        }

        public PathNode GetNode(Vector3 worldPosition)
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

        public int GetNodeDistance_Path(PathNode fromNode, PathNode toNode)
        {
            int x = Mathf.Abs(fromNode.X - toNode.X);
            int y = Mathf.Abs(fromNode.Y - toNode.Y);
            return x + y;
        }

        public float GetNodeDistance_Straight(PathNode fromNode, PathNode toNode)
        {

            return Mathf.Sqrt(Mathf.Pow(fromNode.X - toNode.X, 2) + Mathf.Pow(fromNode.Y - toNode.Y, 2));
        }

        public List<PathNode> GetNodesInRange_Circle(int x, int y, int range)
        {
            var fromNode = grid.GetGridObject(x, y);
            return GetNodesInRange_Circle(fromNode, range);
        }

        public List<PathNode> GetNodesInRange_Circle(PathNode fromNode, int range)
        {
            var nodes = new List<PathNode>();

            for (int x = fromNode.X - range; x < fromNode.X + range + 1; x++)
            {
                for (int y = fromNode.Y - range; y < fromNode.Y + range + 1; y++)
                {
                    if (x < 0 || x > grid.GetWidth() - 1) continue;
                    if (y < 0 || y > grid.GetHeight() - 1) continue;

                    var toNode = grid.GetGridObject(x, y);
                    if (GetNodeDistance_Straight(fromNode, toNode) <= range) nodes.Add(toNode);
                }
            }
            return nodes;
        }

        public List<PathNode> GetNodesInRange_Square(int x, int y, int range)
        {
            var fromNode = grid.GetGridObject(x, y);
            return GetNodesInRange_Square(fromNode, range);
        }

        public List<PathNode> GetNodesInRange_Square(PathNode fromNode, int range)
        {
            var nodes = new List<PathNode>();

            for (int x = fromNode.X - range; x < fromNode.X + range + 1; x++)
            {
                for (int y = fromNode.Y - range; y < fromNode.Y + range + 1; y++)
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