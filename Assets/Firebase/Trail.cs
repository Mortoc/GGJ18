using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Trail
{
    public string playerName;
    public int trackId;
    public List<TrailPos> path = new List<TrailPos>();
    public string trailId;

    public Trail(string name, int id, string tId)
    {
        playerName = name;
        trackId = id;
        trailId = tId;
    }
}

[Serializable]
public class TrailPos
{
    public Vector3 position;
    public long ts;

    public TrailPos(Vector3 pos)
    {
        position = pos;
        ts = UtcTimeNow();
    }

    public TrailPos(Vector3 pos, long time)
    {
        position = pos;
        ts = time;
    }

    public long UtcTimeNow()
    {
        var timeSpan = (DateTime.UtcNow - new DateTime(2018, 1, 26, 0, 0, 0));
        return (long)timeSpan.TotalSeconds;
    }
}

