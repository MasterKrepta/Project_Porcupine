using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Walls and furniture
public class InstalledObject
{

    Tile _tile;

    string objectType;

    float movementCost = 1f;

    int width;
    int height;

    bool linksToNeighbour = false;

    Action<InstalledObject> OnChanged;
    private string _objectType;

    protected InstalledObject() {

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

    static public InstalledObject CreatePrototype(string objectType, float movementCost, int width = 1, int height = 1, bool linksToNeighbor = false) {
        InstalledObject obj = new InstalledObject();

        obj.ObjectType = objectType;
        obj.movementCost = movementCost;
        obj.width = width;
        obj.height = height;
        obj.LinksToNeighbour = linksToNeighbor;

        return obj;
    }

    static public InstalledObject PlaceInstance(InstalledObject proto, Tile tile) {
        InstalledObject obj = new InstalledObject();

        obj.ObjectType = proto.ObjectType;
        obj.movementCost = proto.movementCost;
        obj.width = proto.width;
        obj.height = proto.height;
        obj.LinksToNeighbour = proto.LinksToNeighbour;

        obj.Tile = tile;

        if (tile.PlaceObject(obj) == false) {
            return null;
        }

        return obj;
        //TODO: Assuming 1x1

    }

    public void RegisterOnChanged(Action<InstalledObject> callback) {
        OnChanged += callback;
    }
    public void UnRegisterOnChanged(Action<InstalledObject> callback) {
        OnChanged -= callback;
    }
}
