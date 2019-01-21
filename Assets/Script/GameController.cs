using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject[] players, maps;
	public Mover[] control;
	public GameObject[] respPoint;
	public GameObject[] HUD;
	public Text centralText;

	private bool hasWon=false;

	void Start () {
		respPoint = GameObject.FindGameObjectsWithTag ("Resp");

		int j = 3;
		for (int i = 0 ; i < players.Length; i++, j--) {
			Instantiate (players[i], respPoint[j].gameObject.transform.position, Quaternion.identity);
			control[i] = players[i].GetComponent<Mover> ();
		}

		for (int i = 0 ; i < maps.Length; i++, j--) {
			Instantiate (maps[i], new Vector3(0f,-1999,0f), Quaternion.identity);
		}

		StartCoroutine (StartGame());
	}
	
	void Update () {
		if (Input.GetButton ("Reset")) {
			Debug.Log("Reset");
			Application.LoadLevel (Application.loadedLevelName);
		}
	}

	public void IveWon(string temp){
		int player;
		if (temp == "P1") {
			player = 0;
		} else if (temp == "P2") {
			player = 1;
		} else if (temp == "P3") {
			player = 2;
		} else if (temp == "P4") {
			player = 3;
		} else 
			Debug.LogError("Player does not exist");

		if (!hasWon) {
			string winner="No Player";
			if (temp == "P1") {
				winner = "Player 1";
			} else if (temp == "P2") {
				winner = "Player 2";
			} else if (temp == "P3") {
				winner = "Player 3";
			} else if (temp == "P4") {
				winner = "Player 4";
			}
			Debug.Log (winner + " is The Winner");
			centralText.text = winner+" Is The Winner!!!";
			hasWon=true;
		}
	}

	IEnumerator StartGame(){
		centralText.text = "3";
		yield return new WaitForSeconds (1);
		centralText.text = "2";
		yield return new WaitForSeconds (1);
		centralText.text = "1";
		yield return new WaitForSeconds (1);
		centralText.text = "GO!";
		yield return new WaitForSeconds (3);
		centralText.text = "";
	}
}