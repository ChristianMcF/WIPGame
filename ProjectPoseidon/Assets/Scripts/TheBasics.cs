using UnityEngine;
using System.Collections;

public class TheBasics : MonoBehaviour {
    //initialization of a private variable
    private Ham _hamScript;
    //initialization of a public variable
    public GameObject cubeObject;
    public float moveSpeed;
    private Transform _cubeTransform;

    // Use this for initialization
    void Start () {
        _cubeTransform = cubeObject.transform; 
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("space"))
        {
            _cubeTransform.position = new Vector3(0, _cubeTransform.position.y + (moveSpeed * Time.deltaTime), 0);
        }
	}
}