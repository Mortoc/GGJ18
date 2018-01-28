using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Planet : MonoBehaviour {

  public float MinRadius;
  public float MaxRadius;

  public float Radius { get; private set; }

  private float currentFunk;
  public float MaxFunk = 200.0f;

  public FunkWind[] FunkWindPrefabs;
  public int FunkWindCount = 6;

  private readonly List<FunkWind> funkWinds = new List<FunkWind>();

	void Awake () {
    var scale = Random.Range(MinRadius, MaxRadius);
    transform.localScale = new Vector3(scale, scale, scale);
    Radius = scale * GetComponent<SphereCollider>().radius;
    while (funkWinds.Count < FunkWindCount) {
      SpawnFunkWind();
    }
	}

  public void AddFunk(float funk) {
    currentFunk += funk;
    Debug.LogFormat("Current funk is {0}", currentFunk);

    if (currentFunk >= MaxFunk) {
      OnMaximumFunkAchieved();
    }
  }

  private void OnMaximumFunkAchieved() {
    Debug.Log("HUZZAH!!!!!! Mortoc hate's exclamation points!!!!!!!!");
  }

  private void SpawnFunkWind() {
    if (FunkWindPrefabs.Length > 0 && funkWinds.Count < FunkWindCount) {
      var funkWindPrefab = FunkWindPrefabs[Random.Range(0, FunkWindPrefabs.Length)];
      var position = Random.Range(Radius + funkWindPrefab.MinAltitudeOffset, Radius + funkWindPrefab.MaxAltitudeOffset) * Random.insideUnitSphere.normalized;
      var funkWind = Instantiate(funkWindPrefab, position, Quaternion.identity);
      funkWind.Setup(this);
      funkWinds.Add(funkWind);
    }
  }
}
