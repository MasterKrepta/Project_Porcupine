using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class WorldController : MonoBehaviour {

     static WorldController _instance;
    public static WorldController Instance { get; protected set; }

    public World World { get; protected set; }
	// Use this for initialization
	void OnEnable () {
        if (_instance != null) {
            Debug.LogError("There should be only one World Controller");
        }
        Instance = this;

        //CREATE world
        World = new World();
        //Center the camera
        Camera.main.transform.position = new Vector3(World.Width / 2, World.Height / 2, Camera.main.transform.position.z);
    }

    private void Update() {
        //TODO add pause and speed controls
        World.Update(Time.deltaTime);
    }

    public Tile GetTileAtWorldCor(Vector3 coord) {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return WorldController.Instance.World.GetTileAt(x, y);
    }
}
