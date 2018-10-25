using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Walls and furniture
public class Furniture
{

    Tile _tile;

    string objectType;

    public float movementCost = 1f;

    int width;
    int height;

    bool linksToNeighbour = false;

    Action<Furniture> OnChanged;
    Func< Tile,bool> funcPosValidation;

    private string _objectType;

    protected Furniture() {

    }

    public Tile Tile {
        get {
            return _tile;
        }

        protected set {
            _tile = value;
        }
    }

    public string ObjectType {
        get {
            return _objectType;
        }

        protected set {
            _objectType = value;
        }
    }

    public bool LinksToNeighbour {
        get {
            return linksToNeighbour;
        }

        protected set {
            linksToNeighbour = value;
        }
    }

    Func<Tile, bool> FuncPosValidation {
        get {
            return funcPosValidation;
        }

        set {
            funcPosValidation = value;
        }
    }

    static public Furniture CreatePrototype(string objectType, float movementCost, int width = 1, int height = 1, bool linksToNeighbor = false) {
        Furniture obj = new Furniture();

        obj.ObjectType = objectType;
        obj.movementCost = movementCost;
        obj.width = width;
        obj.height = height;
        obj.LinksToNeighbour = linksToNeighbor;

        obj.FuncPosValidation = obj.__IsValidPos;

        return obj;
    }

    static public Furniture PlaceInstance(Furniture proto, Tile tile) {

        if (proto.FuncPosValidation(tile) == false) {
            Debug.LogError("Place instance - pos validity returned false");
            return null;
        }
               
        //! We know we can place on a valid position
        
        Furniture obj = new Furniture();

        obj.ObjectType = proto.ObjectType;
        obj.movementCost = proto.movementCost;
        obj.width = proto.width;
        obj.height = proto.height;
        obj.LinksToNeighbour = proto.LinksToNeighbour;

        obj.Tile = tile;

        if (tile.PlaceFurniture(obj) == false) {
            return null;
        }

        if (obj.LinksToNeighbour) {

            Tile t;
            int x = tile.X;
            int y = tile.Y;

            //This type links to neighbors so inform our neighbors
            t = tile.World.GetTileAt(x, y + 1);
            if (t != null && t.Furniture != null && t.Furniture.ObjectType == obj.ObjectType) {
                t.Furniture.OnChanged(t.Furniture);
            }

            //East
            t = tile.World.GetTileAt(x + 1, y);
            if (t != null && t.Furniture != null && t.Furniture.ObjectType == obj.ObjectType) {
                t.Furniture.OnChanged(t.Furniture);
            }
            //South
            t = tile.World.GetTileAt(x, y - 1);
            if (t != null && t.Furniture != null && t.Furniture.ObjectType == obj.ObjectType) {
                t.Furniture.OnChanged(t.Furniture);
            }
            //West
            t = tile.World.GetTileAt(x - 1, y);
            if (t != null && t.Furniture != null && t.Furniture.ObjectType == obj.ObjectType) {
                t.Furniture.OnChanged(t.Furniture);
            }
        }

        return obj;
        //TODO: Assuming 1x1

    }
    //TODO these shouldnt be called directly
    public bool  __IsValidPos(Tile t) {
        //Check if tile is floor and doesnt contain furniture
        if (t.Type != TileType.FLOOR) {
            return false;
        }
        if (t.Furniture !=null) {
            return false;
        }
        return true;
    }

    public bool __IsValidPos_Door(Tile t) {
        //Check for a wall on east and west side, or north south
        return true;
    }

    public void RegisterOnChanged(Action<Furniture> callback) {
        OnChanged += callback;
    }
    public void UnRegisterOnChanged(Action<Furniture> callback) {
        OnChanged -= callback;
    }

    public bool IsValidPosition(Tile t) {
        return FuncPosValidation(t);
    }

    
}
