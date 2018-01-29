using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRandomWander : MonoBehaviour {

  public float speed = 1.0f;
  public float minDirChangeTime = 2.0f;
  public float maxDirChangeTime = 5.0f;

  private float nextUpdate = -1.0f;
  private Vector3 currentTarget;

	void Update () {
    if (Time.time > nextUpdate) {
      currentTarget = Random.onUnitSphere;
      nextUpdate = Time.time + Random.Range(minDirChangeTime, maxDirChangeTime);
    }
    transform.Rotate(currentTarget * speed * Time.deltaTime);
	}
}
