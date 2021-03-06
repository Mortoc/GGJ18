﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
  public float BaseMoveSpeed = 10.0f;
  public float BaseRotateSpeed = 10.0f;

  public float MinAltitudeOffset = 2.0f;
  public float MaxAltitudeOffset = 10.0f;

  private float minAltitude;
  private float maxAltitude;

  private float currentAltitude;

  public Planet Planet;

  private float rotation = 0.0f;

  [Header("Controls")]
  public float MouseTurnSpeed = 2.0f;
  public float MousePitchSpeed = 1.0f;

  private Vector2 mouseOffset;

  private Rigidbody rb;

  private readonly Dictionary<string, instrument> instrumentList = new Dictionary<string, instrument>();

  public float InstrumentPowerIncrement = 1.0f;

  void Start() {
    minAltitude = Planet.Radius + MinAltitudeOffset;
    maxAltitude = Planet.Radius + MaxAltitudeOffset;

    currentAltitude = Mathf.Lerp(minAltitude, maxAltitude, 0.5f);
    transform.position = new Vector3(0.0f, currentAltitude, 0.0f);

    rb = GetComponent<Rigidbody>();
  }

  void Update() {
#if !UNITY_EDITOR
    if(Input.GetMouseButton(0))
#endif
    {
      mouseOffset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
      mouseOffset.x -= 0.5f;
      mouseOffset.x *= MouseTurnSpeed;
      mouseOffset.y -= 0.5f;
      mouseOffset.y *= MousePitchSpeed;
    }
  }

  void FixedUpdate() {
    currentAltitude = Mathf.Clamp(currentAltitude + mouseOffset.y, minAltitude, maxAltitude);

    var targetPosition = (transform.position + transform.forward * 5.0f).normalized * currentAltitude;
    targetPosition += transform.right * mouseOffset.x;

    var dir = (targetPosition - transform.position).normalized;
    var targetRotation = Quaternion.LookRotation(dir, transform.position.normalized);


    rb.AddForce(dir * BaseMoveSpeed);

    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, BaseRotateSpeed));
  }

  void OnParticleCollision(GameObject other) {
    if (other.tag == "FunkWind") {
      if (!instrumentList.ContainsKey(other.name)) { 
        var funkWind = other.GetComponentInParent<FunkWind>();
        var instrument = Instantiate(funkWind.instrumentPrefab, transform);
        instrument.Setup(this);
        instrumentList.Add(other.name, instrument);
      } else {
        var instrument = instrumentList[other.name];
        instrument.AddPower(InstrumentPowerIncrement);
      }
    }
  }

  public void RemoveInstrument(instrument instrument) {
    if (instrumentList.ContainsKey(instrument.name)) {
      instrumentList.Remove(instrument.name);
    }
  }
}
