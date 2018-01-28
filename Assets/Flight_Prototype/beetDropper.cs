using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beetDropper : MonoBehaviour {

	public List<instrument> beet;
	[SerializeField]
	public GameObject instrumentslot;
	//public int instOffset;
	//public int instAngle;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == "instrument") {
			AddInstrument (other.GetComponent<instrument>());
		}
	}
	private void AddInstrument(instrument _instrument){
		beet.Add(_instrument);
		_instrument.transform.SetParent (instrumentslot.transform);
		_instrument.Setup (this);
	}
	private void DropBeets(instrument _instrument){
		beet.Remove(_instrument);
	}

}
