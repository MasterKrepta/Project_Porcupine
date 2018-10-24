using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JobQueue {
    Queue<Job> jobQueue;

    Action<Job> cbJobCreated;
    Action<Job> cbJobCancel;

    public JobQueue() {
        jobQueue = new Queue<Job>();
    }

    public void Enqueue(Job j) {
        jobQueue.Enqueue(j);

        
        if (cbJobCreated != null) {
            cbJobCreated(j);
        }
    }
    public Job Dequeue() {
        if (jobQueue.Count ==0) {
            return null;
        }

        return jobQueue.Dequeue();
    }
    public void RegisterJobCreationCallback(Action<Job> cb) {
        cbJobCreated += cb;
    }
    public void RegisterJobCancelCallback(Action<Job> cb) {
        cbJobCancel += cb;
    }

    public void UnRegisterJobCreationCallback(Action<Job> cb) {
        cbJobCreated -= cb;
    }

    public void UnRegisterJobCancelCallback(Action<Job> cb) {
        cbJobCancel -= cb;
    }
}
