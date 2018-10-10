using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum TileType { EMPTY, FLOOR }
public class Tile {

    

    TileType type = TileType.EMPTY;

    Action<Tile> OnTileTypeChanged;
    

    Inventory inventory;
    Furniture furniture;

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
            if(OnTileTypeChanged != null && oldType != type) 
                OnTileTypeChanged(this);
                
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

    public void RegisterTileTypeChanged(Action<Tile> callback) {
        OnTileTypeChanged += callback;
    }
    public void UnRegisterTileTypeChanged(Action<Tile> callback) {
        OnTileTypeChanged -= callback;
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

}
