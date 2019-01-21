using UnityEngine;
using System.Collections;

public class Mapper : MonoBehaviour {

	public Transform self, obj;
	public Mover check;

	void Awake () {
		string temp = tag.Replace("Map", "");
		obj = GameObject.FindGameObjectWithTag (temp).GetComponent<Transform> ();
		check = GameObject.FindGameObjectWithTag (temp).GetComponent<Mover> ();
	}
	
	void Update () {
		if (!check.finished)
			self.position = new Vector3 (obj.position.x, -1999, obj.position.z);
		else
			Destroy (gameObject);
	}
}
