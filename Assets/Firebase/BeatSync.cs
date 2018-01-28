using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class BeatSync : MonoBehaviour
{
    private float syncInterval = 0.5f;
    private string trailName = "test";
    private int trackId = 0;

    private Trail trailData;
    private Guid trailId;
    private DatabaseReference dbRef;
    private bool syncEnded = false;

    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ggj18-22f1e.firebaseio.com/");
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        StartSync();
    }

    public void StartSync()
    {
        trailId = Guid.NewGuid();
        trailData = new Trail(trailName, trackId, trailId.ToString());
        StartCoroutine(SyncAndWait());
    }

    public void EndSync()
    {
        syncEnded = true;
    }

    IEnumerator SyncAndWait()
    {
        while (!syncEnded)
        {
            trailData.path.Add(new TrailPos(transform.position));
            dbRef.Child("trails").Child(trailId.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(trailData));
            yield return new WaitForSeconds(syncInterval);
        }
        syncEnded = false;
    }
}
