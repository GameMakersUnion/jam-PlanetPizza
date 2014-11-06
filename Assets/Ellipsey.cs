using UnityEngine;
using System.Collections;

public class Ellipsey : MonoBehaviour {

	
	Color colour;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		GetComponent<TextMesh>().color = new Color(Random.Range (0f,1.0f), Random.Range (0f,1.0f), Random.Range (0f,1.0f)); 
	}
}
