using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class WorldController : MonoBehaviour {

    
     static WorldController _instance;
    public static WorldController Instance { get; protected set; }

    public Sprite floorSprite; // FIXME
    //public Sprite wallSprite; // FIXME

    Dictionary<Tile, GameObject> tileGamobjectMap;
    Dictionary<InstalledObject, GameObject> installedObjectsMap;

    Dictionary<string, Sprite> installedObjSprites;

    public World World { get; protected set; }
	// Use this for initialization
	void Start () {

        installedObjSprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Art/InstalledObjects");

        foreach (Sprite s in sprites) {
            //Debug.Log(s);
            installedObjSprites[s.name] = s;
        }

        if(_instance != null) {
            Debug.LogError("There should be only one World Controller");
        }
        Instance = this;

        //CREATE world
        World = new World();

        World.RegisterInstalledObject(OnInstalledObjectCreated);

        //Instantiate Dictionary to track data to objects
        tileGamobjectMap = new Dictionary<Tile, GameObject>();
        installedObjectsMap = new Dictionary<InstalledObject, GameObject>();

        //Create GO for each tile for visuals
        for (int x = 0; x < World.Width; x++) {
            for (int y = 0; y < World.Height; y++) {
                Tile tile_data = World.GetTileAt(x, y);

                GameObject tile_go = new GameObject();
                tileGamobjectMap.Add(tile_data, tile_go); // Pair to dictionary

                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0) ;
                tile_go.transform.SetParent(this.transform, true);
                //ADD renderer but dont set sprite yet
                tile_go.AddComponent<SpriteRenderer>();
                
                tile_data.RegisterTileTypeChanged( OnTileTypeChanged);
            }
        }
        World.RandomizeTiles();
    }
    

    //! - not in use
    void DestroyAllTileGO() {
        while (tileGamobjectMap.Count> 0) {
            Tile tile_data = tileGamobjectMap.Keys.First();
            GameObject tile_go = tileGamobjectMap[tile_data];

            tileGamobjectMap.Remove(tile_data);

            tile_data.UnRegisterTileTypeChanged(OnTileTypeChanged);

            Destroy(tile_go);

            //AFTER this is called we would call a new function to build the next level. 
        }
    }
    void OnTileTypeChanged(Tile tile_data) {
        if(tileGamobjectMap.ContainsKey(tile_data) == false) {
            Debug.LogError("No tile data, did you not add to the dictionary or unregister a callback");
            return;
        }
        GameObject tile_go = tileGamobjectMap[tile_data];

        if (tile_go == null) {
            Debug.LogError("Returned GO is null-- did you not add to the dictionary or unregister a callback");
            return;
        }
        if (tile_data.Type == TileType.FLOOR) {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        }
        else if (tile_data.Type == TileType.EMPTY) {
            
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
        else {
            Debug.LogError("OnTileTypeChanged - unrecognized tile type");
        }
    }

    public Tile GetTileAtWorldCor(Vector3 coord) {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return WorldController.Instance.World.GetTileAt(x, y);
    }

    public void OnInstalledObjectCreated(InstalledObject obj) {
        GameObject obj_go = new GameObject();
        installedObjectsMap.Add(obj, obj_go); // Pair to dictionary

        obj_go.name = obj.ObjectType + "_" + obj.Tile.X + "_" + obj.Tile.Y;
        
        obj_go.transform.position = new Vector3(obj.Tile.X, obj.Tile.Y, 0);
        
        obj_go.transform.SetParent(this.transform, true);

        //todo  : fix me
        
        //obj_go.AddComponent<SpriteRenderer>().sprite = installedObjSprites["Wall_"] ; // TODO   : fix me
        obj_go.AddComponent<SpriteRenderer>().sprite = GetSpriteForInstalledObj(obj);

        obj.RegisterOnChanged(OnInstalledObjChanged);
    }

    Sprite GetSpriteForInstalledObj(InstalledObject obj) {
        if(obj.LinksToNeighbour == false)
            return installedObjSprites[obj.ObjectType];

        string spriteName = obj.ObjectType + "_";

        //Current Coords
        int x = obj.Tile.X;
        int y = obj.Tile.Y;

        //Check for neighbours
        Tile t;

        //North
        t =  World.GetTileAt(x, y + 1);
        if (t != null && t.InstalledObject != null && t.InstalledObject.ObjectType == obj.ObjectType) {
            spriteName += "N";
        }

        //East
        t = World.GetTileAt(x + 1, y);
        if (t != null && t.InstalledObject != null && t.InstalledObject.ObjectType == obj.ObjectType) {
            spriteName += "E";
        }
        //South
        t = World.GetTileAt(x, y - 1);
        if (t != null && t.InstalledObject != null && t.InstalledObject.ObjectType == obj.ObjectType) {
            spriteName += "S";
        }
        //West
        t = World.GetTileAt(x-1, y);
        if (t != null && t.InstalledObject != null && t.InstalledObject.ObjectType == obj.ObjectType) {
            spriteName += "W";
        }

        // ! the Sprite name is more complex
        //Debug.Log("Returning: " + spriteName);

        if(installedObjSprites.ContainsKey(spriteName) == false) {
            Debug.LogError("No sprite with name:" + spriteName);
            return null;
        }
        return installedObjSprites[spriteName];
    }

    void OnInstalledObjChanged(InstalledObject obj) {
        Debug.LogError("on installed changed Not Implimented yet");
    }
}
