using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunkWind : MonoBehaviour {
  public float MinAltitudeOffset = 2.0f;
  public float MaxAltitudeOffset = 30.0f;

  public float Speed = 10.0f;

  private float minAltitude;
  private float maxAltitude;

  private Planet planet;

  private Vector3 axis;

  public float RandomSpeed = 20.0f;
  private float xSeed;
  private float ySeed;
  private float zSeed;

  public void Setup(Planet planet) {
    this.planet = planet;
    minAltitude = planet.Radius + MinAltitudeOffset;
    maxAltitude = planet.Radius + MaxAltitudeOffset;

    axis = new Vector3(Random.value, Random.value, 0.0f);
    xSeed = Random.value * 100.0f;
    ySeed = Random.value * 100.0f;
    zSeed = Random.value * 100.0f;
  }

  void Update() {
    if (planet) {
      axis.x = Mathf.PerlinNoise(Time.time * RandomSpeed, xSeed) - 0.5f;
      axis.y = Mathf.PerlinNoise(Time.time * RandomSpeed, ySeed) - 0.5f;

      transform.RotateAround(planet.transform.position, axis, Time.deltaTime * Speed);

      var altitude = Mathf.Lerp(minAltitude, maxAltitude, Mathf.PerlinNoise(Time.time * RandomSpeed, zSeed));
      transform.position = transform.position.normalized * altitude;
    }
  }
}
