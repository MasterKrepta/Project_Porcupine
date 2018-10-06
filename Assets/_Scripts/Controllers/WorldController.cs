using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

     static WorldController _instance;
    public static WorldController Instance { get; protected set; }

    public Sprite floorSprite;
    public World World { get; protected set; }
	// Use this for initialization
	void Start () {
        if(_instance != null) {
            Debug.LogError("There should be only one World Controller");
        }
        Instance = this;

        //CREATE world
        World = new World();
        

        //Create GO for each tile for visuals
        for (int x = 0; x < World.Width; x++) {
            for (int y = 0; y < World.Height; y++) {
                Tile tile_data = World.GetTileAt(x, y);

                GameObject tile_go = new GameObject();
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0) ;
                tile_go.transform.SetParent(this.transform, true);
                //ADD renderer but dont set sprite yet
                tile_go.AddComponent<SpriteRenderer>();

                tile_data.RegisterTileTypeChanged( (tile) => { OnTileTypeChanged(tile, tile_go); });
                
            }
        }
        World.RandomizeTiles();
    }
    

    void OnTileTypeChanged(Tile tile_data, GameObject tile_go) {
        if (tile_data.Type == Tile.TileType.FLOOR) {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        }
        else if (tile_data.Type == Tile.TileType.EMPTY) {
            
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
        else {
            Debug.LogError("OnTileTypeChanged - unrecognized tile type");
        }
    }

	// Update is called once per frame
	void Update () {
 
	}
}
