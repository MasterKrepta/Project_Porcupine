using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Job  {

    //Holds info for a Queued up job, like 
    // Placing furniture, moving inventory, working at a desk/Fighting

    Tile tile;
    float jobTime;


    Action<Job> cbJobComplete;
    Action<Job> cbJobCancel;

    public Tile Tile {
        get {
            return tile;
        }

        set {
            tile = value;
        }
    }

    public Job(Tile tile, Action<Job> cbJobComplete, float jobTime = 1f) {
        this.Tile = tile;
        this.cbJobComplete += cbJobComplete;
    }

    public void RegisterJobComplete(Action<Job> cb) {
        cbJobComplete += cb;
    }
    public void RegisterJobCancel(Action<Job> cb) {
        cbJobCancel += cb;
    }

    public void DoWork(float workTime) {
        jobTime -= workTime;

        if (jobTime <= 0) {
            if (cbJobComplete != null) 
                cbJobComplete(this);
        }
    }

    public void CancelJob() {
        if (cbJobCancel != null)
            cbJobCancel(this);
    }
}
