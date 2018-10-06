using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile {

    public enum TileType {EMPTY, FLOOR}

    TileType type = TileType.EMPTY;

    Action<Tile> OnTileTypeChanged;

    LooseObject looseObject;
    InstalledObject installedObject;

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

    public Tile(World world, int x, int y) {
        this.world = world;
        this.x = x;
        this.y = y;
    }

    public void RegisterTileTypeChanged(Action<Tile> callback) {
        OnTileTypeChanged += callback;
    }
    public void UnRegisterTileTypeChanged(Action<Tile> callback) {
        OnTileTypeChanged -= callback;
    }

}
