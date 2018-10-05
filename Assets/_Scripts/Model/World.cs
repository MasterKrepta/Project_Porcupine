using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

    Tile[,] tiles;
    int width;
    int height;


    public int Width {
        get {
            return width;
        }

        set {
            width = value;
        }
    }

    public int Height {
        get {
            return height;
        }

        set {
            height = value;
        }
    }
    public World(int width = 100, int height = 100) {
        this.Width = width;
        this.Height = height;

        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                tiles[x, y] = new Tile(this, x, y);
            }
        }
        Debug.Log("World created with " + (width * height) + " tiles");
    }

    public void RandomizeTiles() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (Random.Range(0, 2) == 0) {
                    tiles[x, y].Type = Tile.TileType.EMPTY;
                }
                else {
                    tiles[x, y].Type = Tile.TileType.FLOOR;
                }

            }

        }
    }

    public Tile GetTileAt(int x, int y) {
        if(x > Width || x < 0) {
            Debug.LogError("Tile (" + x + "," + y + ") is out of range.");
            return null;
        }

        return tiles[x, y];
    }
}
