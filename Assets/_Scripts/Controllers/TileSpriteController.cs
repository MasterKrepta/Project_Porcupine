using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TileSpriteController : MonoBehaviour {

    public Sprite floorSprite; // FIXME
    public Sprite emptySprite; //FIxme

    Dictionary<Tile, GameObject> tileGamobjectMap;
    Dictionary<string, Sprite> furnitureSprites;

    public World World { get { return WorldController.Instance.World; } }

    void Start () {
        //Instantiate Dictionary to track data to objects
        tileGamobjectMap = new Dictionary<Tile, GameObject>();
        
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
                SpriteRenderer sr = tile_go.AddComponent<SpriteRenderer>();
                sr.sprite = emptySprite;
            }
        }
       World.RegisterTileChanged(OnTileChanged); 
       
        
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
            
            tile_go.GetComponent<SpriteRenderer>().sprite = emptySprite;
            tile_go.GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
        }
        else {
            Debug.LogError("OnTileTypeChanged - unrecognized tile type");
        }
    }
}
