using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

  public Transform Target;

  public Vector3 PositionOffset;
  public Vector3 RotationOffset;

  public float FollowSpeed = 10.0f;
  public float TurnSpeed = 10.0f;

  private Vector3 positionOffset;
  private Quaternion rotationOffset;

  private Vector3 moveVelocity;

  void Awake() {
    positionOffset = transform.position - Target.position;
    rotationOffset = Quaternion.Inverse(Target.rotation) * transform.rotation;
  }

	void LateUpdate () {
    transform.position = Vector3.SmoothDamp(transform.position, Target.TransformPoint(positionOffset + PositionOffset), ref moveVelocity, FollowSpeed * Time.deltaTime);
    transform.rotation = Quaternion.Slerp(transform.rotation, Target.rotation * rotationOffset * Quaternion.Euler(RotationOffset), TurnSpeed * Time.deltaTime);
	}
}
