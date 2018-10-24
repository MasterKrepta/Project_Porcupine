using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World {

    Tile[,] tiles;
    List<Character> characters;

    Dictionary<string, Furniture> furnPrototypes;
    Action<Furniture> cbFurniture;
    Action<Character> cbCharacter;
    Action<Tile> cbTileChanged;

    JobQueue jobQueue;
    //TODO will proboboly be replaced with dedicated job queue class


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

    public JobQueue JobQueue {
        get {
            return jobQueue;
        }

        set {
            jobQueue = value;
        }
    }

    public World(int width = 100, int height = 100) {
        JobQueue = new JobQueue();

        this.Width = width;
        this.Height = height;

        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                tiles[x, y] = new Tile(this, x, y);
                tiles[x, y].RegisterTileChanged(OnTileChanged);
            }
        }
        //Debug.Log("World created with " + (width * height) + " tiles");


        CreateFurnPrototypes();

        characters = new List<Character>();
        
    }

    public void Update(float deltaTime) {
        foreach (Character c in characters) {
            c.Update(deltaTime);
        }
    }

    public Character CreateCharacter(Tile t) {
        Character c = new Character(t);
        characters.Add(c);
        if (cbCharacter != null) {

        }
        cbCharacter(c);
        return c;
    }

    void CreateFurnPrototypes() {
        furnPrototypes = new Dictionary<string, Furniture>();

        furnPrototypes.Add("Wall", Furniture.CreatePrototype(
                                                                "Wall",
                                                                0, // Impassable
                                                                1, 1,
                                                                true)
            );
    }

    public void PlaceFurniture(string objType, Tile t) {
        //tODO assuming 1x1 tiles - change later
        if(furnPrototypes.ContainsKey(objType) == false) {
            Debug.LogError("Installed objs doesnt contain prototype for key " + objType);
            return;
        }
        Furniture obj = Furniture.PlaceInstance(furnPrototypes[objType], t);


        if(obj == null) {
            Debug.LogError("Failed to place object - maybe already occupied");
            return;
        }
        if (cbFurniture != null) {
            cbFurniture(obj);
        }
        
    }

    

        public void RandomizeTiles() {
        //Debug.Log("Tiles randomized");
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

    public void RegisterFurniture(Action<Furniture> callback) {
        cbFurniture += callback;
    }
    public void UnRegisterFurniture(Action<Furniture> callback) {
        cbFurniture -= callback;
    }

    public void RegisterTileChanged(Action<Tile>  callback) {
        cbTileChanged += callback;
    }
    public void UnRegisterTileChanged(Action<Tile> callback) {
        cbTileChanged -= callback;
    }

    public void RegisterCharacter(Action<Character> callback) {
        cbCharacter += callback;
    }
    public void UnRegisterCharacter(Action<Character> callback) {
        cbCharacter -= callback;
    }
    public void OnTileChanged(Tile t) {
        if (cbTileChanged == null)
            return;

        cbTileChanged(t);

    }

    public bool IsFurniturePlacementValid(string furnType, Tile t) {
        return furnPrototypes[furnType].IsValidPosition(t);
    }

    public Furniture GetFurnProto(string objType) {
        if (furnPrototypes.ContainsKey(objType) == false) {
            Debug.LogError("No Furniture with type " + objType);
        }
        return furnPrototypes[objType];
    }
}

