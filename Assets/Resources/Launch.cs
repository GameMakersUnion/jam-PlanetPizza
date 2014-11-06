using UnityEngine;
using System.Collections;

public class Launch : MonoBehaviour {

	public GameObject sun, pizza;
	public GameObject target;
	public Transform startPosition;
	GameObject currentPizza;

	// Use this for initialization
	void Start () {
		currentPizza = (GameObject) Instantiate(pizza);
		currentPizza.transform.position = startPosition.position;
		currentPizza.GetComponent<Gravity>().sun = sun;
		currentPizza.GetComponent<Gravity>().target = target;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
