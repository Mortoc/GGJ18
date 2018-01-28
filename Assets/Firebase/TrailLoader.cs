using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Linq;

public class TrailLoader : MonoBehaviour {
    private int trailExpirationInSeconds = 30; // 5 min (test)

    [SerializeField]
    private List<Trail> trailData = new List<Trail>();

    [SerializeField]
    private GameObject trailPrefab;

    private DateTime toUtcNow(long ts)
    {
        return (new DateTime(2018, 1, 26, 0, 0, 0) + new TimeSpan(0, 0, Convert.ToInt32(ts)));
    }

    void Start() {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ggj18-22f1e.firebaseio.com/");

        FirebaseDatabase.DefaultInstance.RootReference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error loading player trails: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                var trailDict = (IDictionary)snapshot.Child("trails").Value;
                List<string> trailKeys = new List<string>((IEnumerable<string>)trailDict.Keys);
                List<object> trails = new List<object>((IEnumerable<object>)trailDict.Values);
                for (var i = 0; i < trails.Count; ++i)
                {
                    Dictionary<string, object> trail = (Dictionary<string, object>)trails[i];
                    var trailDatum = new Trail(trail["playerName"].ToString(), Convert.ToInt32(trail["trackId"]), trailKeys[i]);
                    var path = new List<TrailPos>();
                    foreach(Dictionary<string, object> p in (IList)trail["path"])
                    {
                        var time = (long)Convert.ToDouble(p["ts"]);
                        var posDict = (IDictionary)p["position"];
                        var pos = new Vector3(
                            float.Parse(posDict["x"].ToString()),
                            float.Parse(posDict["y"].ToString()),
                            float.Parse(posDict["z"].ToString()));
                        path.Add(new TrailPos(pos, time));
                    }
                    trailDatum.path = path;
                    trailData.Add(trailDatum);
                }
                InitializeTrails();
            }
        });
    }

    void InitializeTrails()
    {
        Debug.Log("init trails "+trailData.Count);
        Color prev = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        for(var j = 0; j < trailData.Count; ++j)
        {
            var trailDatum = trailData[j];
            var visibleTrailData = trailDatum.path.Where(tp => DateTime.UtcNow <= toUtcNow(tp.ts) + new TimeSpan(0, 0, trailExpirationInSeconds)).ToList();
            if(visibleTrailData.Count > 0)
            {
                var go = Instantiate(trailPrefab);
                go.name = "Trail_" + trailDatum.playerName;
                go.transform.parent = transform;
                var lineRenderer = go.GetComponent<LineRenderer>();
                lineRenderer.startColor = prev;
                lineRenderer.positionCount = visibleTrailData.Count;
                for (var i = 0; i < visibleTrailData.Count; ++i)
                {
                    lineRenderer.SetPosition(i, visibleTrailData[i].position);
                }
                prev = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
                lineRenderer.endColor = prev;
            } else
            {
                // CULL HERE
                Debug.Log("We should delete " + trailDatum.playerName + "'s trail it's tooooo old: "+ trailDatum.trailId);
                FirebaseDatabase.DefaultInstance.RootReference.Child("trails").Child(trailDatum.trailId).RemoveValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("could not delete (maybe someone beat us to it? it's not like corey concurrency proofed this or anything)");
                    }
                    else if (task.IsCompleted)
                    {
                        Debug.Log(trailDatum.trailId + " deleted!");
                    }
                });
            }
        }
    }
}
