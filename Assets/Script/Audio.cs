using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour {
	
	public AudioSource jetSound;
	private float jetPitch;
	private const float LowPitch = .1f;
	private const float HighPitch = 3.0f;
	private const float SpeedToRevs = .01f;
	Vector3 myVelocity;
	Rigidbody rig;
	
	void Awake (){
		rig = GetComponent<Rigidbody>();
		jetSound = GetComponent<AudioSource> ();
	}
	
	private void FixedUpdate(){
		float player=1;
		if (tag == "P1") {
			player = 2.5f;
		} else if (tag == "P2") {
			player = 1.8f;
		} else if (tag == "P3") {
			player = 2f;
		} else if (tag == "P4") {
			player = 1.2f;
		} else 
			Debug.LogError("Player does not exist");

		myVelocity = rig.velocity;
		float forwardSpeed = Mathf.Sqrt (Mathf.Pow (rig.velocity.z, 2) + Mathf.Pow (rig.velocity.x, 2));
		float engineRevs = Mathf.Abs (forwardSpeed) * SpeedToRevs * player;
		jetSound.pitch = Mathf.Clamp (engineRevs, LowPitch, HighPitch);
	}
	
}
