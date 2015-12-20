using UnityEngine;
using System.Collections;

public class TestButtonReciever : MonoBehaviour
{
	private Rigidbody _cubeRigidbody;
	public string controlName;

	void Start ()
	{
		_cubeRigidbody = gameObject.GetComponent<Rigidbody> ();
	}

	public void PlayerAButton ()
	{
		_cubeRigidbody.AddForce (transform.up * 100);
	}
}
