using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_TileGraph {

    public Dictionary<Tile, Path_Node<Tile>> nodes;

    public Path_TileGraph(World world) {

        nodes = new Dictionary<Tile, Path_Node<Tile>>();

        //loop all tiles , create a node for each one
        for (int x = 0; x < world.Width; x++) {
            for (int y = 0; y < world.Height; y++) {

                Tile t = world.GetTileAt(x, y);

                if (t.MovementCost > 0) { // if unwalkable
                    Path_Node<Tile> n = new Path_Node<Tile>();
                    n.data = t;
                    nodes.Add(t, n);
                }
                
            }
        }

        //create edges for each
        foreach (Tile t in nodes.Keys) {

            Path_Node<Tile> n = nodes[t];

            List<Path_Edge<Tile>> edges = new List<Path_Edge<Tile>>();
            //Get neighbors
            Tile[] neighbours = t.GetNeighbours(true); //? some array spots could be null

            //if walkable create edge
            for (int i = 0; i < neighbours.Length; i++) {
                if (neighbours[i] != null && neighbours[i].MovementCost >0) {
                    Path_Edge<Tile> e = new Path_Edge<Tile>();
                    e.cost = neighbours[i].MovementCost;
                    e.node = nodes[neighbours[i]];

                    edges.Add(e);
                }
            }

            n.edges = edges.ToArray();
        
        }

        Debug.Log("Pathfinding: " + nodes.Count + " Nodes Created");
    }

}
