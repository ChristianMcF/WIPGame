using UnityEngine;
using System.Collections;

public class AudioEyes : MonoBehaviour
{

    private Transform _playerEyes;

    // Use this for initialization
    void Start()
    {
        _playerEyes = transform.FindChild("PlayerEyes");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit _raycasthit;
        Physics.Raycast(_playerEyes.position, _playerEyes.transform.forward, out _raycasthit, 100f);
        Debug.DrawRay(_playerEyes.position, _playerEyes.transform.forward * 100, Color.red);
        Debug.Log(_raycasthit.collider.tag);
    }
}
