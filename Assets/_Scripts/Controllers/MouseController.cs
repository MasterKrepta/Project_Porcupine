using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public GameObject cursor;
    Vector3 lastFramePos;
    Vector3 dragStartPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 currFramePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePos.z = 0;

        //Update Circle Pos
        Tile tileUnderMouse = GetTileAtWorldCor(currFramePos);
        if(tileUnderMouse != null) {
            cursor.SetActive(true);
            Vector3 cursorPos = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            cursor.transform.position = cursorPos;
        }
        else {
            cursor.SetActive(false);
        }

        //Get MouseClicks
        //STart of drag
        if (Input.GetMouseButtonDown(0) ){
            dragStartPos = currFramePos;

        }
        // end drag
        if (Input.GetMouseButtonUp(0) ){
            int startX = Mathf.FloorToInt(dragStartPos.x);
            int endX = Mathf.FloorToInt(currFramePos.x);
            if (endX < startX) { // IF we are dragging from the left. 
                int temp = endX;
                endX = startX;
                startX = temp;
            }

            int startY = Mathf.FloorToInt(dragStartPos.y);
            int endY = Mathf.FloorToInt(currFramePos.y);
            if (endY < startY) { // IF we are dragging from the left. 
                int temp = endY;
                endY = startY;
                startY = temp;
            }


            for (int x = startX; x <= endX; x++) {
                for (int y = startY; y <= endY; y++) {
                    Tile t = WorldController.Instance.World.GetTileAt(x, y);
                    if(t != null) {
                        t.Type = Tile.TileType.FLOOR;
                    }
                }
            }
            
        }

        //Screen Drag
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) { // right or middle
            Vector3 diff = lastFramePos - currFramePos;
            Camera.main.transform.Translate(diff);
        }
        lastFramePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); ;
        lastFramePos.z = 0;

    }

    Tile GetTileAtWorldCor(Vector3 coord) {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return WorldController.Instance.World.GetTileAt(x, y);
    }
}
