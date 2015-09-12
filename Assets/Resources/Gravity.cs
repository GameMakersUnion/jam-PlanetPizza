using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gravity : MonoBehaviour
{

    LineRenderer lineRenderer;

    public GameObject sun;
    public GameObject target;
    const float GRAVITY = 2000;
    const float LAUNCH = 500;
    const float MAX_LAUNCH = 10;
    //const float REQUIRED_DISTANCE = 9;
    float REQUIRED_DISTANCE;
    GameState state;
    enum GameState
    {
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
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        planets = GameObject.FindGameObjectsWithTag("Planet");
        state = GameState.launching;
        level = int.Parse(Application.loadedLevelName.Substring(Application.loadedLevelName.Length - 1));

        float radius = GameObject.Find("Sun").transform.FindChild("Sphere").GetComponent<Collider>().bounds.extents.x;
        REQUIRED_DISTANCE = radius;
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case GameState.moving:
                foreach (var planet in planets)
                {
                    float distance = Vector2.Distance(transform.position, planet.transform.position);
                    float magnitude = (GRAVITY * gameObject.GetComponent<Rigidbody2D>().mass * planet.GetComponent<Rigidbody2D>().mass * Time.deltaTime) / (distance * distance);
                    Vector2 direction = (planet.transform.position - transform.position).normalized;

                    gameObject.GetComponent<Rigidbody2D>().AddForce(magnitude * direction);
                }
                if (Vector2.Distance(transform.position, sun.transform.position) <= REQUIRED_DISTANCE)
                {
                    cooked = true;
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pizza");
                }
                break;
            case GameState.launching:

                if (!clicked && Input.GetMouseButtonDown(0))
                {
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, transform.position);
                    if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up))
                    {
                        clicked = true;
                    }

                }
                else if (clicked)
                {
                    float distance;
                    Vector3 position;
                    Vector2 direction;
                    
                    position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    distance = Vector3.Distance(position, transform.position);
                    if (distance > MAX_LAUNCH)
                    {
                        distance = MAX_LAUNCH;
                    }

                    direction = (transform.position - position).normalized;

                    Debug.Log(direction + " : " + distance + " : " + MAX_LAUNCH);
                    
                    lineRenderer.SetColors(Color.Lerp(Color.green, Color.red, distance/MAX_LAUNCH), Color.Lerp(Color.green, Color.red, distance/MAX_LAUNCH));
                    lineRenderer.SetPosition(1, (Vector3)(direction * distance) + transform.position);
                    
                    if (Input.GetMouseButtonUp(0) && clicked)
                    {

                        Component.Destroy(lineRenderer);

                        gameObject.GetComponent<Rigidbody2D>().AddForce(direction * distance * LAUNCH);
                        state = GameState.moving;
                    }
                }


                break;
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (state == GameState.win || state == GameState.lose) return;

        string text;
        if (collider.gameObject == target)
        {
            if (cooked)
            {
                text = "Great Job!";
                state = GameState.win;
            }
            else
            {
                text = "Cold Pizza";
                state = GameState.lose;
            }
        }
        else
        {
            text = "Wrong Target";
            state = GameState.lose;
        }
        GameObject.Find("WinText").GetComponent<TextMesh>().text = text;

        //Destroy(gameObject);
        GetComponent<Renderer>().enabled = false;

    }

    void OnTriggerExit2D(Collider2D collider)
    {
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
