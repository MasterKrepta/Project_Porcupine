using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildModeController : MonoBehaviour {
 
    bool buildModeIsObjects = false;
    TileType buildModeTile = TileType.FLOOR;
    string buildModeObjType;

    void OnFurnitureJobComplete(string furnitureType, Tile t) {
        
    }

    public void SetMode_BuildFloor() {
        buildModeIsObjects = false;
        buildModeTile = TileType.FLOOR;
    }

    public void SetMode_Bulldoze() {
        buildModeIsObjects = false;
        buildModeTile = TileType.EMPTY;
    }

    public void SetMode_BuildFurniture(string objType) {
        buildModeIsObjects = true;
        buildModeObjType = objType;
    }

    public void DoBuild(Tile t) {
        if (buildModeIsObjects == true) {
            //Create installed object and assign

            //Check if we can build here
            string furnitureType = buildModeObjType;
            if (WorldController.Instance.World.IsFurniturePlacementValid(furnitureType, t) &&
                    t.pendingFurnitureJob == null) {

                Job j = new Job(t, (theJob) => {
                    WorldController.Instance.World.PlaceFurniture(furnitureType, theJob.Tile);
                    t.pendingFurnitureJob = null;
                });

                //TODO dont like manually setting, too easy to forget to set/clear them
                t.pendingFurnitureJob = j;
                j.RegisterJobCancel((theJob) => { theJob.Tile.pendingFurnitureJob = null; });

                WorldController.Instance.World.JobQueue.Enqueue(j);
                Debug.Log("Queue Size: " + WorldController.Instance.World.JobQueue.Count);
            }
        }
        else {
            //Change Tile 
            t.Type = buildModeTile;
        }
    }
}
