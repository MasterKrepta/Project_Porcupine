using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSpriteController : MonoBehaviour {

    //!Bare bones that piggybacks on furnitureSpriteController
    Dictionary<Job, GameObject> jobGOMap;

    FurnitureSpriteController fsc;
	// Use this for initialization
	void Start () {
        fsc = GameObject.FindObjectOfType<FurnitureSpriteController>();
        jobGOMap = new Dictionary<Job, GameObject>();
        // TODO no job queue yet
        WorldController.Instance.World.JobQueue.RegisterJobCreationCallback(OnJobCreated);
    }

    void OnJobCreated(Job j) {
        //TODO only can build furniture
        
        GameObject job_go = new GameObject();
        jobGOMap.Add(j, job_go); // Pair to dictionary

        job_go.name = "JOB_" + j.jobObjectType + "_" + j.Tile.X + "_" + j.Tile.Y;

        job_go.transform.position = new Vector3(j.Tile.X, j.Tile.Y, 0);

        job_go.transform.SetParent(this.transform, true);

        SpriteRenderer sr =  job_go.AddComponent<SpriteRenderer>();
        sr.sprite = fsc.GetSpriteForFurniture(j.jobObjectType); 
        sr.sortingLayerName = "Jobs";
        sr.color = new Color(0.5f, 1f, 0.5f, 0.25f);
        
        j.RegisterJobComplete(OnJobEnded);
        j.RegisterJobCancel(OnJobEnded);
    }

    void OnJobEnded(Job j) {
        //tODO weather completed or canceled
                
        GameObject job_go = jobGOMap[j];
        j.UnRegisterJobCancel(OnJobEnded);
        j.UnRegisterJobComplete(OnJobEnded);
        Destroy(job_go);
    }
}