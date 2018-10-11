using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class WorldController : MonoBehaviour {

    
     static WorldController _instance;
    public static WorldController Instance { get; protected set; }

    public Sprite floorSprite; // FIXME
    public Sprite emptySprite; //FIxme
    //public Sprite wallSprite; // FIXME

    Dictionary<Tile, GameObject> tileGamobjectMap;
    Dictionary<Furniture, GameObject> furnitureGOMap;

    Dictionary<string, Sprite> furnitureSprites;

    public World World { get; protected set; }
	// Use this for initialization
	void OnEnable () {

        LoadSprites();

        if (_instance != null) {
            Debug.LogError("There should be only one World Controller");
        }
        Instance = this;

        //CREATE world
        World = new World();

        World.RegisterFurniture(OnFurnitureCreated);

        //Instantiate Dictionary to track data to objects
        tileGamobjectMap = new Dictionary<Tile, GameObject>();
        furnitureGOMap = new Dictionary<Furniture, GameObject>();

        //Create GO for each tile for visuals
        for (int x = 0; x < World.Width; x++) {
            for (int y = 0; y < World.Height; y++) {
                Tile tile_data = World.GetTileAt(x, y);

                GameObject tile_go = new GameObject();
                tileGamobjectMap.Add(tile_data, tile_go); // Pair to dictionary

                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                tile_go.transform.SetParent(this.transform, true);
                //ADD renderer but dont set sprite yet
                tile_go.AddComponent<SpriteRenderer>().sprite = emptySprite;
                

                World.RegisterTileChanged(OnTileChanged); //! WAS TILE DATA - if this works fine delete the comment
            }
        }
        //World.RandomizeTiles();

        //Center the camera
        Camera.main.transform.position = new Vector3(World.Width / 2, World.Height / 2, Camera.main.transform.position.z);
    }

    private void LoadSprites() {
        furnitureSprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Art/Furniture");

        foreach (Sprite s in sprites) {
            //Debug.Log(s);
            furnitureSprites[s.name] = s;
        }
    }


    //! - not in use
    void DestroyAllTileGO() {
        while (tileGamobjectMap.Count> 0) {
            Tile tile_data = tileGamobjectMap.Keys.First();
            GameObject tile_go = tileGamobjectMap[tile_data];

            tileGamobjectMap.Remove(tile_data);

            tile_data.UnRegisterTileChanged(OnTileChanged);

            Destroy(tile_go);

            //AFTER this is called we would call a new function to build the next level. 
        }
    }
    void OnTileChanged(Tile tile_data) {
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
            tile_go.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
        }
        else if (tile_data.Type == TileType.EMPTY) {
            
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
            tile_go.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
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

    public void OnFurnitureCreated(Furniture furn) {
        GameObject furn_go = new GameObject();
        furnitureGOMap.Add(furn, furn_go); // Pair to dictionary

        furn_go.name = furn.ObjectType + "_" + furn.Tile.X + "_" + furn.Tile.Y;
        
        furn_go.transform.position = new Vector3(furn.Tile.X, furn.Tile.Y, 0);
        
        furn_go.transform.SetParent(this.transform, true);

        //todo  : fix me
        
        //obj_go.AddComponent<SpriteRenderer>().sprite = installedObjSprites["Wall_"] ; // TODO   : fix me
        furn_go.AddComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(furn);
        furn_go.GetComponent<SpriteRenderer>().sortingLayerName = "Wall";

        furn.RegisterOnChanged(OnFurnitureChanged);
    }

    void OnFurnitureChanged(Furniture furn) {
        
        //Make sure furniture graphics are correct
        if (furnitureGOMap.ContainsKey(furn) == false) {
            Debug.LogError("OnFurnitureChanged: -- trying to change visuals for furniture not in map");
            return;
        }

        GameObject furn_go = furnitureGOMap[furn];
        furn_go.GetComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(furn);
        furn_go.GetComponent<SpriteRenderer>().sortingLayerName = "Wall";
    }

    Sprite GetSpriteForFurniture(Furniture furn) {
        if(furn.LinksToNeighbour == false)
            return furnitureSprites[furn.ObjectType];

        string spriteName = furn.ObjectType + "_";

        //Current Coords
        int x = furn.Tile.X;
        int y = furn.Tile.Y;

        //Check for neighbours
        Tile t;

        //North
        t =  World.GetTileAt(x, y + 1);
        if (t != null && t.Furniture != null && t.Furniture.ObjectType == furn.ObjectType) {
            spriteName += "N";
        }

        //East
        t = World.GetTileAt(x + 1, y);
        if (t != null && t.Furniture != null && t.Furniture.ObjectType == furn.ObjectType) {
            spriteName += "E";
        }
        //South
        t = World.GetTileAt(x, y - 1);
        if (t != null && t.Furniture != null && t.Furniture.ObjectType == furn.ObjectType) {
            spriteName += "S";
        }
        //West
        t = World.GetTileAt(x-1, y);
        if (t != null && t.Furniture != null && t.Furniture.ObjectType == furn.ObjectType) {
            spriteName += "W";
        }

        // ! the Sprite name is more complex
        //Debug.Log("Returning: " + spriteName);

        if(furnitureSprites.ContainsKey(spriteName) == false) {
            Debug.LogError("No sprite with name:" + spriteName);
            return null;
        }
        return furnitureSprites[spriteName];
    }

  
}
