using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour {

    public GameObject cursorPrefab;
    Vector3 lastFramePos;
    Vector3 currFramePos;

    Vector3 dragStartPos;


    bool buildModeIsObjects = false;
    TileType buildModeTile = TileType.FLOOR;
    string buildModeObjType;

    List<GameObject> dragPreviewGO = new List<GameObject>();
    // Use this for initialization
    void Start () {
        SimplePool.Preload(cursorPrefab, 100);
	}
	
	// Update is called once per frame
	void Update () {
        currFramePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePos.z = 0;

        //UpdateCursor();
        UpdateDrag();
        UpdateCameraMovement();

        lastFramePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); ;
        lastFramePos.z = 0;

    }

    private void UpdateCameraMovement() {
        //Screen Drag
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) { // right or middle
            Vector3 diff = lastFramePos - currFramePos;
            Camera.main.transform.Translate(diff);
        }

        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 25f);
    }

    private void UpdateDrag() {
        //RETURN OVER UI
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        //Start of drag
        int startX = Mathf.FloorToInt(dragStartPos.x);
        int endX = Mathf.FloorToInt(currFramePos.x);
        int startY = Mathf.FloorToInt(dragStartPos.y);
        int endY = Mathf.FloorToInt(currFramePos.y);

        if (endX < startX) { // IF we are dragging from the left. 
            int temp = endX;
            endX = startX;
            startX = temp;
        }

        if (endY < startY) { // IF we are dragging from the left. 
            int temp = endY;
            endY = startY;
            startY = temp;
        }

        if (Input.GetMouseButtonDown(0)) {
            dragStartPos = currFramePos;

        }

        //Clean up old drag previews
        while (dragPreviewGO.Count > 0) {
            {
                GameObject go = dragPreviewGO[0];
                dragPreviewGO.RemoveAt(0);
                SimplePool.Despawn(go);
            }

        }
        if (Input.GetMouseButton(0)) {
            //Display a preview of drag area
            for (int x = startX; x <= endX; x++) {
                for (int y = startY; y <= endY; y++) {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null) {
                        //Display Building hint ontop of tile
                        GameObject go = SimplePool.Spawn(cursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        go.transform.SetParent(this.transform, true);
                        dragPreviewGO.Add(go);
                    }
                }
            }

        }
        // end drag
        if (Input.GetMouseButtonUp(0)) {
            for (int x = startX; x <= endX; x++) {
                for (int y = startY; y <= endY; y++) {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if (t != null) {
                        if (buildModeIsObjects == true) {
                            //Create installed object and assign

                            //FIXME: we are just assuming walls right now
                            WorldController.Instance.World.PlaceInstalledObj(buildModeObjType, t);

                    }
                        else {
                            //Change Tile 
                            t.Type = buildModeTile;
                        }
                    }
                }
            }
        }
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

    //private void UpdateCursor() {
    //    //Update Circle Pos
    //    Tile tileUnderMouse = WorldController.Instance.GetTileAtWorldCor(currFramePos);
    //    if (tileUnderMouse != null) {
    //        cursor.SetActive(true);
    //        Vector3 cursorPos = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
    //        cursor.transform.position = cursorPos;
    //    }
    //    else {
    //        cursor.SetActive(false);
    //    }
    //}


}
