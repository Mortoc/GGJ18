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

  public Transform Planet;

  private float rotation = 0.0f;

  [Header("Controls")]
  public float TurnSpeed = 0.5f;

  public float PitchSpeed = 0.25f;

  private Rigidbody rb;

  void Start() {
    var planetCollider = Planet.GetComponent<SphereCollider>();
    var planetRadius = planetCollider.radius * Planet.localScale.x;
    minAltitude = planetRadius + MinAltitudeOffset;
    maxAltitude = planetRadius + MaxAltitudeOffset;

    currentAltitude = Mathf.Lerp(minAltitude, maxAltitude, 0.5f);
    transform.position = new Vector3(0.0f, currentAltitude, 0.0f);

    rb = GetComponent<Rigidbody>();
  }

  void FixedUpdate() {
    if (Input.GetKey(KeyCode.W)) {
      currentAltitude = Mathf.Max(currentAltitude - PitchSpeed, minAltitude);
    } else if (Input.GetKey(KeyCode.S)) {
      currentAltitude = Mathf.Min(currentAltitude + PitchSpeed, maxAltitude);
    }

    var targetPosition = (transform.position + transform.forward * 5.0f).normalized * currentAltitude;
    if (Input.GetKey(KeyCode.A)) {
      targetPosition += transform.right * -TurnSpeed;
    } else if (Input.GetKey(KeyCode.D)) {
      targetPosition += transform.right * TurnSpeed;
    }

    Debug.DrawLine(transform.position, targetPosition);
    var dir = targetPosition - transform.position;
    var targetRotation = Quaternion.LookRotation(dir, transform.position.normalized);


    rb.AddForce(dir * BaseMoveSpeed);

    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, BaseRotateSpeed));
  }
}
