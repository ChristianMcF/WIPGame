using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    private GameObject _playerEyes;
    public float lookSpeed;
    public float moveSpeed;

    // Use this for initialization
    void Start()
    {
        _playerEyes = transform.FindChild("PlayerEyes").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        #region PlayerLook
        _playerEyes.transform.localRotation *= Quaternion.Euler(-Input.GetAxis("Mouse Y") * lookSpeed, 0, 0);
        gameObject.transform.localRotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        #endregion
        #region PlayerMovement
        //gameObject.GetComponent<Rigidbody>().AddRelativeForce((Vector3.forward * Input.GetAxis("Vertical")) * moveSpeed);
        #endregion
    }
}
