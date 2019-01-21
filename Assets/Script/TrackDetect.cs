using UnityEngine;
using System.Collections;

public class TrackDetect : MonoBehaviour {

	GameObject obj;
	Mover move;

	void OnTriggerEnter(Collider other) {
		if (other.tag != "Untagged") {
			obj = other.gameObject;
			move = obj.GetComponent<Mover> ();
			move.LapCounter (tag);
		}
	}
}
