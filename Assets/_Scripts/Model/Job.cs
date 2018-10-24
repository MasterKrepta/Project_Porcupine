using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Job  {

    //Holds info for a Queued up job, like 
    // Placing furniture, moving inventory, working at a desk/Fighting

    Tile tile;
    float jobTime;

    //TODO THIS IS AWEFUL - change in the future
    public string jobObjectType;

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

    public Job(Tile tile, string jobObjectType, Action<Job> cbJobComplete, float jobTime = 1f) {
        this.Tile = tile;
        this.jobObjectType = jobObjectType;
        this.cbJobComplete += cbJobComplete;
        this.jobTime = jobTime;
    }

    public void RegisterJobComplete(Action<Job> cb) {
        cbJobComplete += cb;
    }
    public void RegisterJobCancel(Action<Job> cb) {
        cbJobCancel += cb;
    }

    public void UnRegisterJobComplete(Action<Job> cb) {
        cbJobComplete -= cb;
    }
    public void UnRegisterJobCancel(Action<Job> cb) {
        cbJobCancel -= cb;
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
