using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Linq;

public class TrailLoader : MonoBehaviour {
    // TODO cull "expired" trails from DB 
    private int trailExpirationInSeconds = 300; // 5 min (test)

    [SerializeField]
    private List<Trail> trailData = new List<Trail>();

    private DateTime toUtcNow(int ts)
    {
        return (new DateTime(2018, 1, 26, 0, 0, 0) + new TimeSpan(0, 0, ts));
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
                var trails = (IDictionary)snapshot.Child("trails").Value;
                foreach(Dictionary<string, object> trail in trails.Values)
                {
                    var trailDatum = new Trail(trail["playerName"].ToString(), Convert.ToInt32(trail["trackId"]));
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
            }
        });
    }
}
