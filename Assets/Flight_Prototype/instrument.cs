using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instrument : MonoBehaviour {
  public float StartingPower = 25.0f;

  private float currentPower;
  private PlayerController player;

  void Awake() {
    currentPower = StartingPower;
  }

  void Update() {
    if (player) {
      currentPower -= Time.deltaTime;

      if (currentPower <= 0.0f) {
        player.RemoveInstrument(this);
        Destroy(gameObject);
      }
    }
  }

  public void Setup(PlayerController player) {
    this.player = player;
  }

  public void AddPower(float power) {
    currentPower += power;
  }
}	
