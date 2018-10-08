using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World {

    Tile[,] tiles;

    Dictionary<string, InstalledObject> installedPrototypes;
    Action<InstalledObject> OnInstalledObject;
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


        CreateInstalledObjPrototypes();
    }

    void CreateInstalledObjPrototypes() {
        installedPrototypes = new Dictionary<string, InstalledObject>();

        

        installedPrototypes.Add("Wall", InstalledObject.CreatePrototype(
                                                                "Wall",
                                                                0, // Impassable
                                                                1, 1)
            );
    }

    public void PlaceInstalledObj(string objType, Tile t) {
        //tODO assuming 1x1 tiles - change later
        if(installedPrototypes.ContainsKey(objType) == false) {
            Debug.LogError("Installed objs doesnt contain prototype for key " + objType);
            return;
        }
        InstalledObject obj = InstalledObject.PlaceInstance(installedPrototypes[objType], t);


        if(obj == null) {
            Debug.LogError("Failed to place object - maybe already occupied");
            return;
        }
        if (OnInstalledObject != null) {
            OnInstalledObject(obj);
        }
        
    }

    

        public void RandomizeTiles() {
        Debug.Log("Tiles randomized");
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (UnityEngine.Random.Range(0, 2) == 0) {
                    tiles[x, y].Type = TileType.EMPTY;
                }
                else {
                    tiles[x, y].Type = TileType.FLOOR;
                }

            }

        }
    }

    public Tile GetTileAt(int x, int y) {
        if (x > Width || x < 0 || y > Height || y < 0) {
            //Debug.LogWarning("Tile (" + x + "," + y + ") is out of range.");
            return null;
        }
        return tiles[x, y];
    }

    public void RegisterInstalledObject(Action<InstalledObject> callback) {
        OnInstalledObject += callback;
    }
    public void UnRegisterInstalledObject(Action<InstalledObject> callback) {
        OnInstalledObject -= callback;
    }
}

