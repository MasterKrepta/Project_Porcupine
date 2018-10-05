using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

    public Sprite floorSprite;
    World world;
	// Use this for initialization
	void Start () {
        //CREATE world
        world = new World();
        world.RandomizeTiles();

        //Create GO for each tile for visuals
        for (int x = 0; x < world.Width; x++) {
            for (int y = 0; y < world.Height; y++) {
                GameObject tile_go = new GameObject();
                tile_go.name = "Tile_" + x + "_" + y;
                Tile tile_data = world.GetTileAt(x, y);
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y);

                //ADD renderer but dont set sprite yet
                SpriteRenderer tile_sr = tile_go.gameObject.AddComponent<SpriteRenderer>();
            }
        }
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
