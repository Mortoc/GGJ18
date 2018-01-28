using System.Collections;
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
  public float TurnSpeed = 0.5f;

  public float PitchSpeed = 0.25f;

  public float MouseTurnSpeed = 2.0f;
  public float MousePitchSpeed = 1.0f;

  private Vector2 mouseOffset;

  private Rigidbody rb;

  void Start() {
    minAltitude = Planet.Radius + MinAltitudeOffset;
    maxAltitude = Planet.Radius + MaxAltitudeOffset;

    currentAltitude = Mathf.Lerp(minAltitude, maxAltitude, 0.5f);
    transform.position = new Vector3(0.0f, currentAltitude, 0.0f);

    rb = GetComponent<Rigidbody>();
  }

  void Update() {
    mouseOffset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    mouseOffset.x -= 0.5f;
    mouseOffset.x *= MouseTurnSpeed;
    mouseOffset.y -= 0.5f;
    mouseOffset.y *= MousePitchSpeed;
  }

  void FixedUpdate() {
    if (Input.GetKey(KeyCode.W)) {
      currentAltitude -= PitchSpeed;
    } else if (Input.GetKey(KeyCode.S)) {
      currentAltitude += PitchSpeed;
    }

    currentAltitude = Mathf.Clamp(currentAltitude + mouseOffset.y, minAltitude, maxAltitude);

    var targetPosition = (transform.position + transform.forward * 5.0f).normalized * currentAltitude;
    if (Input.GetKey(KeyCode.A)) {
      targetPosition += transform.right * -TurnSpeed;
    } else if (Input.GetKey(KeyCode.D)) {
      targetPosition += transform.right * TurnSpeed;
    } else {
      targetPosition += transform.right * mouseOffset.x;
    }

    var dir = (targetPosition - transform.position).normalized;
    var targetRotation = Quaternion.LookRotation(dir, transform.position.normalized);


    rb.AddForce(dir * BaseMoveSpeed);

    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, BaseRotateSpeed));
  }

  void OnParticleCollision(GameObject other) {
    Debug.Log("Ran into " + other.name);
  }
}
