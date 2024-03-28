using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace SD.Grids
{
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        public static List<Vector3> FindVectorPath(Grid<PathNode> grid, Vector3 startWorldPosition, Vector3 endWorldPosition, bool ignoreEndNode = false, Occupant ignoreOccupants = Occupant.None)
        {
            var start = grid.GetGridObject(startWorldPosition);
            var end = grid.GetGridObject(endWorldPosition);

            List<PathNode> path = FindNodePath(start, end, ignoreEndNode, ignoreOccupants);
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

        public static List<PathNode> FindNodePath(PathNode startNode, PathNode endNode, bool ignoreEndNode = false, Occupant ignoreOccupants = Occupant.None)
        {
            if (!endNode.IsWalkable && !ignoreEndNode) return null;
            if (endNode.Occupant != Occupant.None && !ignoreEndNode) return null;

            var grid = startNode.grid;

            var openList = new Heap<PathNode>(grid.MaxSize);
            var closedList = new HashSet<PathNode>();
            openList.Add(startNode);

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
                PathNode currentNode = openList.RemoveFirst();
                closedList.Add(currentNode);

                //Reached final node
                if (currentNode == endNode) return CalculatePath(endNode);

                foreach (PathNode neighbour in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbour)) continue;

                    //if the neighbor is the endNode and choosing to ignore whether it is walkable, add it to the closed list
                    if (neighbour == endNode && ignoreEndNode)
                    {
                        // I feel like I can just return the path here. I've reached a point adjacent to the target node
                        //Do nothing here, bypass the next if statement
                        //Debug.Log("Ignoring End Node");
                    }
                    else if (!neighbour.IsWalkable)
                    {
                        //Debug.Log("Removing unwalkable/occupied tile " + neighbour.x + "," + neighbour.y);
                        closedList.Add(neighbour);
                        continue;
                    }
                    else if (neighbour.Occupant != Occupant.None && ignoreOccupants != neighbour.Occupant)
                    {
                        closedList.Add(neighbour);
                        continue;
                    }

                    //Adding in movement cost here of the neighbor node to account for areas that are more difficult to move through
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour) + neighbour.MovementCost;

                    if (tentativeGCost < neighbour.gCost)
                    {
                        //If it's lower than the cost previously stored on the neightbor, update it
                        neighbour.cameFromNode = currentNode;
                        neighbour.gCost = tentativeGCost;
                        neighbour.hCost = CalculateDistanceCost(neighbour, endNode);

                        if (!openList.Contains(neighbour)) openList.Add(neighbour);
                        else openList.UpdateItem(neighbour);
                    }
                }
            }

            //Debug.Log("Path could not be found");
            return null;
        }

        //Returns a list of nodes that can be travelled to reach a target destination
        //*** I need to add the optimizations from Sebastian Lague's Heap 
        /*private List<PathNode> FindNodePath(int startX, int startY, int endX, int endY, bool ignoreEndNode = false, Occupant ignoreOccupants = Occupant.None)
        {
            PathNode endNode = grid.GetGridObject(endX, endY);

            if (!endNode.IsWalkable && !ignoreEndNode) return null;
            if (endNode.Occupant != Occupant.None && !ignoreEndNode) return null;
            
            PathNode startNode = grid.GetGridObject(startX, startY);

            openList = new List<PathNode> { startNode };
            var closedList = new HashSet<PathNode>();

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
                        // I feel like I can just return the path here. I've reached a point adjacent to the target node
                        //Do nothing here, bypass the next if statement
                        //Debug.Log("Ignoring End Node");
                    }
                    else if (!neighbour.IsWalkable)
                    {
                        //Debug.Log("Removing unwalkable/occupied tile " + neighbour.x + "," + neighbour.y);
                        closedList.Add(neighbour);
                        continue;
                    }
                    else if (neighbour.Occupant != Occupant.None && ignoreOccupants != neighbour.Occupant)
                    {
                        closedList.Add(neighbour);
                        continue;
                    }

                    //Adding in movement cost here of the neighbor node to account for areas that are more difficult to move through
                    int tentativeGCost = Mathf.RoundToInt(currentNode.gCost + 
                        CalculateDistanceCost(currentNode, neighbour) + neighbour.MovementCost);

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
            //Debug.Log($"Path could not be found from {startX},{startY} to {endX},{endY}");
            return null;
        }*/

        //Return a list of all neighbors, up/down/left/right
        private static List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            var grid = currentNode.grid;
            //I could also just cache this...
            List<PathNode> neighborList = new List<PathNode>();

            //North
            if (currentNode.Y + 1 < grid.GetHeight()) neighborList.Add(grid.GetGridObject(currentNode.X, currentNode.Y + 1));
            //South
            if (currentNode.Y - 1 >= 0) neighborList.Add(grid.GetGridObject(currentNode.X, currentNode.Y - 1));
            //Left
            if (currentNode.X - 1 >= 0) neighborList.Add(grid.GetGridObject(currentNode.X - 1, currentNode.Y));
            //Right
            if (currentNode.X + 1 < grid.GetWidth()) neighborList.Add(grid.GetGridObject(currentNode.X + 1, currentNode.Y));

            /*
            if (currentNode.X - 1 >= 0)
            {
                //Left Down
                if (currentNode.Y - 1 >= 0) neighborList.Add(grid.GetGridObject(currentNode.X - 1, currentNode.Y - 1));
                //Left Up
                if (currentNode.Y + 1 < grid.GetHeight()) neighborList.Add(grid.GetGridObject(currentNode.X - 1, currentNode.Y + 1));
            }
            if (currentNode.X + 1 < grid.GetWidth())
            {
                //Right Down
                if (currentNode.Y - 1 >= 0) neighborList.Add(grid.GetGridObject(currentNode.X + 1, currentNode.Y - 1));
                //Right Up
                if (currentNode.Y + 1 < grid.GetHeight()) neighborList.Add(grid.GetGridObject(currentNode.X + 1, currentNode.Y + 1));
            }
            */
            return neighborList;
        }

        private static List<PathNode> CalculatePath(PathNode endNode)
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

        private static int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.X - b.X);
            int yDistance = Mathf.Abs(a.Y - b.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        public static int GetNodeDistance_Path(PathNode fromNode, PathNode toNode)
        {
            int x = Mathf.Abs(fromNode.X - toNode.X);
            int y = Mathf.Abs(fromNode.Y - toNode.Y);
            return x + y;
        }

        /// <summary>
        /// Returns the straight line distance between two nodes. 
        /// </summary>
        public static float GetNodeDistance_Straight(PathNode fromNode, PathNode toNode)
        {
            return Mathf.Sqrt(Mathf.Pow(fromNode.X - toNode.X, 2) + Mathf.Pow(fromNode.Y - toNode.Y, 2));
        }

        /// <summary>
        /// Returns a list of nodes within the given range, not allowing diagonals.
        /// </summary>
        public static List<PathNode> GetNodesInRange(PathNode from, int range)
        {
            var grid = from.grid;
            var nodes = new List<PathNode>();

            for (int x = from.X - range; x < from.X + range + 1; x++)
            {
                for (int y = from.Y - range; y < from.Y + range + 1; y++)
                {
                    if (x < 0 || x > grid.GetWidth() - 1) continue;
                    if (y < 0 || y > grid.GetHeight() - 1) continue;

                    int xDiff = Mathf.Abs(x - from.X);
                    int yDiff = Mathf.Abs(y - from.Y);
                    if (xDiff + yDiff > range) continue;

                    var toNode = grid.GetGridObject(x, y);
                    nodes.Add(toNode);
                }
            }

            return nodes;
        }

        /// <summary>
        /// Returns a list of nodes within the given range, allowing diagonals.
        /// </summary>
        public static List<PathNode> GetArea(PathNode from, int range)
        {
            var grid = from.grid;
            var nodes = new List<PathNode>();

            for (int x = from.X - range; x < from.X + range + 1; x++)
            {
                for (int y = from.Y - range; y < from.Y + range + 1; y++)
                {
                    if (x < 0 || x > grid.GetWidth() - 1) continue;
                    if (y < 0 || y > grid.GetHeight() - 1) continue;

                    var toNode = grid.GetGridObject(x, y);
                    nodes.Add(toNode);
                }
            }

            return nodes;
        }

        /// <summary>
        /// Converts a set of Vector2Int to PathNodes. For use with Bresenham.
        /// </summary>
        public static List<PathNode> ConvertToNodes(Grid<PathNode> grid, HashSet<Vector2Int> points)
        {
            var nodes = new List<PathNode>();
            foreach(var point in points)
            {
                var node = grid.GetGridObject(point.x, point.y);
                if (node != null) nodes.Add(node);
            }
            return nodes;
        }

        /// <summary>
        /// Returns an area around the given node that can be moved to with the available range.
        /// </summary>
        public static List<PathNode> GetMovementRange(PathNode from, int range, Occupant ignoreOccupant = Occupant.None)
        {
            var area = GetArea(from, range);

            for (int i = area.Count - 1; i >= 0; i--)
            {
                var path = FindNodePath(from, area[i], false, ignoreOccupant);
                if (path == null)
                {
                    area.RemoveAt(i);
                    continue;
                }
                path.RemoveAt(0); // where the unit is
                if (GetTotalMovements(path) > range)
                {
                    area.RemoveAt(i);
                    continue;
                }
            }
            return area;
        }

        /// <summary>
        /// Returns the total movement cost for the given path.
        /// </summary>
        public static int GetTotalMovements(List<PathNode> nodes)
        {
            int cost = nodes.Count;
            nodes.ForEach(node => cost += node.MovementCost);
            Debug.Log($"Path from {nodes[0].X},{nodes[0].Y} to {nodes[nodes.Count - 1].X}," +
                $"{nodes[nodes.Count - 1].Y} is {cost}.");
            return cost;
        }

        [System.Obsolete]
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
    }
}