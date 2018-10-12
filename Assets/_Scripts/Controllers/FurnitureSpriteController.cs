using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FurnitureSpriteController : MonoBehaviour {

    Dictionary<Furniture, GameObject> furnitureGOMap;

    Dictionary<string, Sprite> furnitureSprites;

    public World World { get { return WorldController.Instance.World; } }

    void Start () {

        LoadSprites();

        //Instantiate Dictionary to track data to objects
        
        furnitureGOMap = new Dictionary<Furniture, GameObject>();

     
       
       World.RegisterFurniture(OnFurnitureCreated);
        
    }

    private void LoadSprites() {
        furnitureSprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Art/Furniture");

        foreach (Sprite s in sprites) {
            //Debug.Log(s);
            furnitureSprites[s.name] = s;
        }
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
