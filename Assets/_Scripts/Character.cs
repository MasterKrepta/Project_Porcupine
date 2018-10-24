using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character  {

    public float X {
        get {
            return Mathf.Lerp(currTile.X, destTile.X, movementPercentage);
        }
    }
    public float Y {
        get {
            return Mathf.Lerp(currTile.Y, destTile.Y, movementPercentage);
        }
    }


    public Tile currTile {
        get; protected set;
    }
    Tile destTile;
    float movementPercentage;

    float speed = 2f;

    public Character(Tile tile) {
        currTile = destTile = tile;
    }

    Action<Character> cbCharacterChanged;

    Job myJob;
    
    public void Update(float deltaTime) {
        //Debug.Log("Char Update");

        if (myJob == null) {
            //Grab job
            myJob =  currTile.World.JobQueue.Dequeue();

            if (myJob!=null) {
                destTile = myJob.Tile;
                myJob.RegisterJobComplete(OnJobEnded);
                myJob.RegisterJobCancel(OnJobEnded);
            }
        }

        if (currTile == destTile) {
            if (myJob!=null) {
                myJob.DoWork(deltaTime);
            }
            return;
        }
        float distToTravel = Mathf.Sqrt(Mathf.Pow(currTile.X - destTile.X, 2) + Mathf.Pow(currTile.Y - destTile.Y, 2));

        float distThisFrame = speed * deltaTime;

        float percThisFram = distThisFrame / distToTravel;

        movementPercentage += percThisFram;

        if (movementPercentage >= 1) {
            //We arrived
            currTile = destTile;
            movementPercentage = 0;
            //TODO? Should we retain overshot movement?
        }

        if (cbCharacterChanged !=null) {
            cbCharacterChanged(this);
        }

    }

    public void SetDest(Tile tile) {
        if (currTile.IsNeighbour(tile, true) == false) {
            Debug.Log("Char::SetDestinaton -- Tiles are not neighbors");
        }
        destTile = tile;
    }
    public void RegisterOnChanged(Action<Character> cb) {
        cbCharacterChanged += cb;
    }
    public void UnRegisterCharacterChanged(Action<Character> cb) {
        cbCharacterChanged -= cb;
    }

    void OnJobEnded(Job j) {
        //Job completed or cancelled
        if (j!= myJob) {
            Debug.LogError("Char told about job that isnt his");
            return;
        }
        myJob = null;

    }
}
