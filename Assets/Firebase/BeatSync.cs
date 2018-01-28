using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class BeatSync : MonoBehaviour
{
    public float syncInterval = 5f;
    public string trailName = "test";
    public int trackId = 0;

    private Trail trailData;
    private Guid trailId;
    private DatabaseReference dbRef;

    void Start()
    {
        trailId = Guid.NewGuid();
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ggj18-22f1e.firebaseio.com/");
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;

        //StartSync();
    }

    public void StartSync()
    {
        trailData = new Trail(trailName, trackId);
        StartCoroutine(SyncAndWait());
    }

    IEnumerator SyncAndWait()
    {
        trailData.path.Add(new TrailPos(transform.position));
        dbRef.Child("trails").Child(trailId.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(trailData));
        yield return new WaitForSeconds(syncInterval);
        StartCoroutine(SyncAndWait());
    }
}
