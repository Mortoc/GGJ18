using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Planet : MonoBehaviour {

  public float MinRadius;
  public float MaxRadius;

  public float Radius { get; private set; }

	void Awake () {
    var scale = Random.Range(MinRadius, MaxRadius);
    transform.localScale = new Vector3(scale, scale, scale);
    Radius = scale * GetComponent<SphereCollider>().radius;
	}
}
