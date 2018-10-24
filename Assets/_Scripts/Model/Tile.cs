using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum TileType { EMPTY, FLOOR }
public class Tile {

    TileType type = TileType.EMPTY;

    Action<Tile> cbTileChanged;
    

    Inventory inventory;
    Furniture furniture;

    public Job pendingFurnitureJob;
    
    
    World world;
    int x;
    int y;

    public TileType Type {
        get {
            return type;
        }

        set {
            TileType oldType = type;
            type = value;
            //Call the callback 
            if(cbTileChanged != null && oldType != type) 
                cbTileChanged(this);
                
        }
    }

    public int X {
        get {
            return x;
        }
    }

    public int Y {
        get {
            return y;
        }
    }

    public Furniture Furniture {
        get {
            return furniture;
        }

        protected set {
            furniture = value;
        }
    }

    public World World {
        get {
            return world;
        }

        protected set {
            world = value;
        }
    }

    public Tile(World world, int x, int y) {
        this.World = world;
        this.x = x;
        this.y = y;
    }

    public void RegisterTileChanged(Action<Tile> callback) {
        cbTileChanged += callback;
    }
    public void UnRegisterTileChanged(Action<Tile> callback) {
        cbTileChanged -= callback;
    }

   

    public bool PlaceFurniture(Furniture objInstance) {
        if (objInstance == null) {
            Furniture = null;
            return true;
        }
        if (Furniture != null) {
            Debug.LogError("Trying to assign installed object when it already has one");
            return false;
        }

        Furniture = objInstance;
        return true;
    }

    public bool IsNeighbour(Tile tile, bool diagOkay = false) {

        //One line solution - Do we have a differance of exactly one between the two coords
            return Mathf.Abs(this.X - tile.X) + Mathf.Abs(this.Y - tile.Y) == 1 ||
                (diagOkay && (Mathf.Abs(this.X - tile.X) == 1 && Mathf.Abs(this.Y - tile.Y) == 1));

        //if (this.x == tile.X && (Mathf.Abs(this.Y - tile.Y) == 1)) {
        //    return true;
        //}

        //if (this.Y == tile.Y && (Mathf.Abs(this.X - tile.X) == 1)) {
        //    return true;
        //}

        //if (diagOkay) {
        //    if (this.x == tile.X + 1 && (this.y == tile.Y + 1 || this.Y == tile.Y - 1))
        //        return true;
        //    if (this.x == tile.X - 1 && (this.y == tile.Y + 1 || this.Y == tile.Y - 1))
        //        return true;
        //}

        //return false;
        
    }
}
