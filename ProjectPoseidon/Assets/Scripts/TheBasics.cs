using UnityEngine;
using System.Collections;

public class TheBasics : MonoBehaviour {

    //Initialization of a public variable
    public GameObject cubeObject;           //Creates a variable which holds all information about a certain object in the scene.
    public float moveSpeed;                 //Declares a variable which holds a float, which is intended to control the movement speed of the cube object
    //Initialization of a private variable
    private Ham _hamScript;                 //Declaration of a variable, which is of a type of another script, allowing for access to instanced methods and variables.
    private Transform _cubeTransform;       //Declares a variable which holds the transform of the cube. Transforms hold; Location, rotation and scale.

    // Use this for initialization
    void Start () {
        //Store the transform of the cube object as a variable for easier access
        _cubeTransform = cubeObject.transform; 
	}
	
	// Update is called once per frame
	void Update () {
        //Acesses the Input manager which is handled by unity. And recieves a boolean indicating whether the spacebar is pressed or not pressed
        if (Input.GetKey("space"))
        {
            //Gets the Vector3 position of the cubes transform, and then creates a temporary vector3 which alters only the y coordinate.
            //Adds to the y coordinate by movespeed times the time.deltaTime. Which makes the movement framerate independent, which is a good thing.
            _cubeTransform.position = new Vector3(0, _cubeTransform.position.y + (moveSpeed * Time.deltaTime), 0);
        }
	}
}