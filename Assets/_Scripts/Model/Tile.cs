using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum TileType { EMPTY, FLOOR }
public class Tile {

    TileType type = TileType.EMPTY;

    Action<Tile> cbTileChanged;
    
    public float MovementCost {
        get {
            if (type == TileType.EMPTY) 
                return 0; // Unwalkable

            if (furniture == null) 
                return 1;
            
            return 1 * furniture.movementCost;
        }
    }

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
      
        
    }

    public Tile[] GetNeighbours(bool diagOkay = false) {

        Tile[] ns;

        if (diagOkay = false) {
            ns = new Tile[4]; // N E S W
        }
        else {
            ns = new Tile[8]; //N E S W NE SE SW NW
        }

        Tile n;
        n = world.GetTileAt(x, y + 1);
        ns[0] = n; // could be null
        n = world.GetTileAt(x+1, y);
        ns[1] = n; // could be null
        n = world.GetTileAt(x, y - 1);
        ns[2] = n; // could be null
        n = world.GetTileAt(x-1, y);
        ns[3] = n; // could be null
        

        if (diagOkay == true) {
            n = world.GetTileAt(x+1, y + 1);
            ns[4] = n; // could be null
            n = world.GetTileAt(x + 1, y-1);
            ns[5] = n; // could be null
            n = world.GetTileAt(x-1, y - 1);
            ns[6] = n; // could be null
            n = world.GetTileAt(x - 1, y-1);
            ns[7] = n; // could be null
            n = world.GetTileAt(x - 1, y+1);
            ns[8] = n; // could be null
        }

        return ns;
    }
}
