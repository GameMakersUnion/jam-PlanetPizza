using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gravity : MonoBehaviour {

	public GameObject sun;
	public GameObject target;
	const float	GRAVITY = 2000;
	const float LAUNCH = 500;
	const float MAX_LAUNCH = 10;
	//const float REQUIRED_DISTANCE = 9;
	float REQUIRED_DISTANCE;
	GameState state;
	enum GameState {
		launching,
		moving,
		win,
		lose
	}
	int level;
	
	bool clicked = false;
	bool cooked = false;

	GameObject[] planets;
	// Use this for initialization
	void Start () {
		planets = GameObject.FindGameObjectsWithTag("Planet");
		state = GameState.launching;
		level =  int.Parse ( Application.loadedLevelName.Substring( Application.loadedLevelName.Length - 1) );
		Debug.Log ("LEVEL:"+level);

		float radius = GameObject.Find("Sun").transform.FindChild("Sphere").collider.bounds.extents.x;
		REQUIRED_DISTANCE = radius;
		Debug.Log ( radius );
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ( state );

		switch (state) {
		case GameState.moving:
			foreach (var planet in planets) {
				float distance = Vector2.Distance(transform.position, planet.transform.position);
				float magnitude = (GRAVITY*gameObject.rigidbody2D.mass*planet.rigidbody2D.mass*Time.deltaTime)/(distance*distance);
				Vector2 direction = (planet.transform.position - transform.position).normalized;
	
	    		gameObject.rigidbody2D.AddForce(magnitude*direction);
			}
			if (Vector2.Distance(transform.position,sun.transform.position) <= REQUIRED_DISTANCE) {
				cooked = true;
				GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pizza");
			}
		break;
		case GameState.launching:

			if (Input.GetMouseButtonDown(0)) {
				if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up)) {
					Debug.Log ("poop");
					clicked = true;
				}
				                                  
			} else if (Input.GetMouseButtonUp(0) && clicked) {
				Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				float distance = Vector3.Distance(position, transform.position);
				if (distance > MAX_LAUNCH) {
					distance = MAX_LAUNCH;
				}
				Vector2 direction = (transform.position - position).normalized;
				gameObject.rigidbody2D.AddForce(direction*distance*LAUNCH);
				state = GameState.moving;
			}


		break;
		}
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (state == GameState.win || state == GameState.lose) return;

		string text;
		if (collider.gameObject == target) {
			if (cooked) {
				text = "You Wins!";
				state = GameState.win;
				Debug.Log ( state );

			}
			else {
				text = "Cold Pizza :(";
				state = GameState.lose;
				Debug.Log ( state );
			}
		}
		else {
			text = "You Lose! GTFO!";
			state = GameState.lose;
			Debug.Log ( state );
		}
		GameObject.Find ("WinText").GetComponent<TextMesh>().text = text;

		//Destroy(gameObject);
		renderer.enabled = false;

	}

	void OnTriggerExit2D(Collider2D collider) {
		if (state == GameState.win || state == GameState.lose) return;
		state = GameState.moving;
	}

	void OnGUI()
	{
		Rect rect = new Rect(0,0,150,50);
		if ( state == GameState.win )
		{
			if ( GUI.Button( rect, "NEXT") ){
				int nextLevel = (level) % 3 + 1;
				Application.LoadLevel("Level_" + nextLevel);
			}
		}else

		
		{
			if ( GUI.Button( rect, "RETRY") ) {
				Application.LoadLevel("Level_" + level);
			}
		}
	}
}
