using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Mover : MonoBehaviour {

	public Camera cam;
	public GameObject stopLight;
	private Rigidbody rig;
	private Text textLapCounter, textTimer, textSpeed;
	public Text[] lapChrono;
	private Slider slide;
	private GameObject HUD;
	private GameController control;

	public bool canMove, finished = false;
	public int moveForce, turnForce, breakForce, hoverHeight, hoverForce, lapsTotal, lapsComplete;
	private float hor, ver, vel, time;
	private float[] times;
	private bool sector1, sector2, sector3, finish;

	void Awake () {
		rig = GetComponent <Rigidbody> ();


		lapChrono = new Text[3];
		control = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		HUD = GameObject.FindGameObjectWithTag (tag+"HUD");
		textLapCounter = GameObject.FindGameObjectWithTag (tag + "Lap").GetComponent<Text>();
		textTimer = GameObject.FindGameObjectWithTag (tag + "Timer").GetComponent<Text>();
		GameObject[] temp = GameObject.FindGameObjectsWithTag (tag + "TimerLap");

		for (int i = 0; i < lapChrono.Length; i++) {
			lapChrono[i] = temp[i].GetComponent<Text>();
		}

		textSpeed = GameObject.FindGameObjectWithTag (tag + "Speed").GetComponent<Text>();
		slide = GameObject.FindGameObjectWithTag (tag + "Slider").GetComponent<Slider>();
		times = new float[lapsTotal];


		StartCoroutine(AtStart(3));

		textLapCounter.text = "Laps: " + (lapsComplete+1) + "/"+lapsTotal;
	}
	
	void FixedUpdate () {
			Ray ray = new Ray (transform.position, -transform.up);
			RaycastHit hit;			
			if (Physics.Raycast(ray, out hit, hoverHeight)){
				float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
				Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
				rig.AddForce(appliedHoverForce);
			}

			vel = Mathf.Sqrt (Mathf.Pow (rig.velocity.z, 2) + Mathf.Pow (rig.velocity.x, 2));

			cam.fieldOfView = vel + 60f;
			stopLight.GetComponent<MeshRenderer> ().material.color = Color.clear;

		if (canMove) {
			if (ver > 0) {
				rig.AddForce (transform.forward * moveForce * Time.deltaTime);
			}

			if (ver < 0) {
				rig.AddForce (-transform.forward * breakForce * Time.deltaTime);
				stopLight.GetComponent<MeshRenderer> ().material.color = Color.red;
			}

			if (hor != 0) { 
				if (transform.InverseTransformDirection (rig.velocity).z > 0) {
					rig.AddTorque (transform.up * hor * turnForce * Time.deltaTime);
				} else if (transform.InverseTransformDirection (rig.velocity).z < 0) {
					rig.AddTorque (transform.up * -hor * turnForce * Time.deltaTime);
				}
			}
		} else if(lapsComplete==lapsTotal)
			GetComponent<MeshCollider> ().enabled = false;
	}

	void Update(){

		if (canMove) {
			hor = Input.GetAxis ("Horizontal" + tag);
			ver = Input.GetAxis ("Vertical" + tag);

			vel = Mathf.Sqrt (Mathf.Pow (rig.velocity.z, 2) + Mathf.Pow (rig.velocity.x, 2));
			string gtext = " Km/h";
			string temp = string.Format ("{0:0000}" + gtext, vel * 100);

			time = Time.time-3f;

			string temp2 = string.Format ("{0:0}:{1:00}.{2:000}", Mathf.Floor (time / 60), Mathf.Floor (time) % 60, Mathf.Floor ((time * 1000) % 1000));

			textTimer.text = temp2;
			textSpeed.text = temp;
			slide.value = vel;
		}
	}

	public void LapCounter(string sector){
		if (sector == "LapT1") {
			sector1 = true;
		} else if (sector == "LapT2") {
			sector2 = true;
		} else if (sector == "LapT3") {
			sector3 = true;
		} else if (sector == "LapFinish") {
			if (sector1 && sector2 && sector3) {
				if(lapsComplete<lapsTotal-1){
					//times[lapsComplete] = time;
					Chrono();
					sector1 = false;
					sector2 = false;
					sector3 = false;
					lapsComplete++;
				}else{
					Debug.Log(tag+" has finished the race");
					Chrono ();
					control.IveWon(tag);
					finished = true;
					canMove=false;
				}
			}
			if(lapsComplete<lapsTotal){
				textLapCounter.text = "Laps: " + (lapsComplete+1) + "/"+lapsTotal;
			}
		}
	}

	void Chrono(){

		Debug.Log (tag + " has completed a lap");
		if (lapsComplete == 0) {
			times [lapsComplete] = time;
			for( int i = 0; i < times.Length ; i++){
				Debug.Log(times[i]);
			}
			lapChrono [lapsComplete].text = "Lap "+(lapsComplete+1)+": "+ string.Format ("{0:0}:{1:00}.{2:000}", Mathf.Floor (times [lapsComplete] / 60), Mathf.Floor (times [lapsComplete]) % 60, Mathf.Floor ((times [lapsComplete] * 1000) % 1000));

		} else if (lapsComplete == 1) {
			times [lapsComplete] = time - times[0];
			for( int i = 0; i < times.Length ; i++){
				Debug.Log(times[i]);
			}
			lapChrono [lapsComplete].text = "Lap "+(lapsComplete+1)+": "+ string.Format ("{0:0}:{1:00}.{2:000}", Mathf.Floor (times [lapsComplete] / 60), Mathf.Floor (times [lapsComplete]) % 60, Mathf.Floor ((times [lapsComplete] * 1000) % 1000));
		
		} else if (lapsComplete == 2){
			times [lapsComplete] = time - (times [0] + times [1]);
			for( int i = 0; i < times.Length ; i++){
				Debug.Log(times[i]);
			}
			lapChrono [lapsComplete].text = "Lap "+(lapsComplete+1)+": "+string.Format ("{0:0}:{1:00}.{2:000}", Mathf.Floor (times [lapsComplete] / 60), Mathf.Floor (times [lapsComplete]) % 60, Mathf.Floor ((times [lapsComplete] * 1000) % 1000));
		}

	}


	IEnumerator AtStart(int sec) {
		yield return new WaitForSeconds (sec);
		canMove = true;
	}
}
