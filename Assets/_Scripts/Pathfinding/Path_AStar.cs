using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using System.Linq;

public class Path_AStar {

    Queue<Tile> path;

    public Path_AStar(World world, Tile tileStart, Tile tileEnd) {
        //Check for valid graph
        if (world.tileGraph == null) {
            world.tileGraph = new Path_TileGraph(world);
        }
        //Dictionary of all walkable nodes
        Dictionary<Tile, Path_Node<Tile>> nodes = world.tileGraph.nodes;

        Path_Node<Tile> start = nodes[tileStart];
        Path_Node<Tile> goal = nodes[tileEnd];

        if (nodes.ContainsKey(tileStart) == false) {
            Debug.LogError("Path_Astar: Starting tile not in list of nodes");
            return;
        }
        if (nodes.ContainsKey(tileEnd) == false) {
            Debug.LogError("Path_Astar: Ending tile not in list of nodes");
            return;
        }

        //Astar
        List<Path_Node<Tile>> ClosedSet = new List<Path_Node<Tile>>();

        //List<Path_Node<Tile>> OpenSet = new List<Path_Node<Tile>>();
        //OpenSet.Add(start);
        SimplePriorityQueue<Path_Node<Tile>> OpenSet = new SimplePriorityQueue<Path_Node<Tile>>();
        OpenSet.Enqueue(start, 0);

        Dictionary<Path_Node<Tile>, Path_Node<Tile>> Came_From = new Dictionary<Path_Node<Tile>, Path_Node<Tile>>();

        Dictionary<Path_Node<Tile>, float> g_score = new Dictionary<Path_Node<Tile>, float>();
        foreach (Path_Node<Tile> n in nodes.Values) {
            g_score[n] = Mathf.Infinity;
        }
        g_score[start] = 0; // Node we are on

        Dictionary<Path_Node<Tile>, float> f_score = new Dictionary<Path_Node<Tile>, float>();
        foreach (Path_Node<Tile> n in nodes.Values) {
            f_score[n] = Mathf.Infinity;
        }
        f_score[start] = heruristic_cost_estimate(start, goal); // Node we are on

        while (OpenSet.Count > 0) {
            Path_Node<Tile> current = OpenSet.Dequeue(); // This will remove the node

            if (current == goal) {
                ReconstructPath(Came_From, current);
                return;
            }

            ClosedSet.Add(current);
            foreach (Path_Edge<Tile> edge_Neighbor in current.edges) {
                Path_Node<Tile> neighbor = edge_Neighbor.node;

                if (ClosedSet.Contains(neighbor)) {
                    continue; // ignore this completed neighbor
                }

                float tentative_g_score = g_score[current] + dist_Between(current, neighbor);

                if (OpenSet.Contains(neighbor) && tentative_g_score >= g_score[neighbor]) {
                    continue;
                }

                Came_From[neighbor] = current;
                g_score[neighbor] = tentative_g_score;
                f_score[neighbor] = g_score[neighbor] + heruristic_cost_estimate(neighbor, goal);

                if (OpenSet.Contains(neighbor) == false) {
                    OpenSet.Enqueue(neighbor, f_score[neighbor]);
                }

            }
        }
        //If we are here it means we have gone through all of open set without current == goal
        //Will only happen if there is no path from start to goal
    }

    float heruristic_cost_estimate(Path_Node<Tile> a, Path_Node<Tile> b) {
        return Mathf.Sqrt(
            Mathf.Pow(a.data.X - b.data.X, 2) +
            Mathf.Pow(a.data.Y - b.data.Y, 2)
            );
    }

    float dist_Between(Path_Node<Tile> a, Path_Node<Tile> b) {

        if (Mathf.Abs(a.data.X - b.data.X) + Mathf.Abs(a.data.Y - b.data.Y) == 1) {
            return 1f;
        }
        if (Mathf.Abs(a.data.X - b.data.X) == 1 && Mathf.Abs(a.data.Y - b.data.Y) == 1)  {
            return 1.41421356237f; // Thid is know diags
        }
        //Just do the math
        return Mathf.Sqrt(
            Mathf.Pow(a.data.X - b.data.X, 2) +
            Mathf.Pow(a.data.Y - b.data.Y, 2)
            );
    }

    void ReconstructPath(
        Dictionary<Path_Node<Tile>, Path_Node<Tile>> Came_From,
        Path_Node<Tile> current) {
        //Current is the goal at this point
        Queue<Tile> total_path = new Queue<Tile>();

        total_path.Enqueue(current.data);

        while (Came_From.ContainsKey(current)) {
            current = Came_From[current];
            total_path.Enqueue(current.data);
        }

        //This will end when we get the starting tile
        path = new Queue<Tile>(total_path.Reverse());
    }

    public Tile GetNextTile() {
        return null;
    }
}