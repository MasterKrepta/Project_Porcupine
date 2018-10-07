using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Walls and furniture
public class InstalledObject {

    Tile tile;

    string objectType;

    float movementCost = 1f;

    int width;
    int height ;

    protected InstalledObject() {

    }

    static public InstalledObject CreatePrototype(string objectType, float movementCost, int width = 1, int height = 1) {
        InstalledObject obj = new InstalledObject();
        obj.objectType = objectType;
        obj.movementCost = movementCost;
        obj.width = width;
        obj.height = height;

        return obj;
    }

    static public InstalledObject PlaceInstance(InstalledObject proto, Tile tile) {
        InstalledObject obj = new InstalledObject();
        obj.objectType = proto.objectType;
        obj.movementCost = proto.movementCost;
        obj.width = proto.width;
        obj.height = proto.height;

        obj.tile = tile;

        if (tile.PlaceObject(obj) == false) {
            return null;
        } 

        return obj;
        //TODO: Assuming 1x1
        
    }
}
