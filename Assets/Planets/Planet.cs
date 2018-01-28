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

	void Awake () {
    var scale = Random.Range(MinRadius, MaxRadius);
    transform.localScale = new Vector3(scale, scale, scale);
    Radius = scale * GetComponent<SphereCollider>().radius;
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
}
