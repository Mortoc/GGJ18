using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

  public float _bpm = 126.0f;
  public Text _playText;
  public float _animTime = 0.15f;
  public Color _onColor;
  public Color _offColor;

  public IEnumerator Start() {
    var beatTime = 60.0f / _bpm;
    var nextBeat = beatTime;
    var beatCount = 1.0f;
    var offset = -0.5f * beatTime;
    var anim = GetComponent<Animator>();
    while(gameObject) {
      yield return 0;
      if ((Time.time - offset) > nextBeat) {
        beatCount += 1.0f;
        nextBeat = beatCount * beatTime;
        StartCoroutine(AnimateTextColor());
      }
    }
  }

  private IEnumerator AnimateTextColor() {
    for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / _animTime) {
      _playText.color = Color.Lerp(_onColor, _offColor, t);
      yield return 0;
    }
    _playText.color = _offColor;
  }

  public void Play() {
    SceneManager.LoadScene(1);
  }
}
