using UnityEngine;
using System;
using System.Collections;

public class AudioEyes : MonoBehaviour
{

    public Transform _playerEyes;
    private AudioSource aud;

    #region AudioGenStuff
    public int position = 0;
    public int samplerate = 44100;
    public float frequency = 440;
    #endregion

    // Use this for initialization
    void Start()
    {

        AudioClip myClip = AudioClip.Create("MySinusoid", samplerate, 1, samplerate, false, OnAudioRead, OnAudioSetPosition);
        aud = GetComponent<AudioSource>();
        aud.clip = myClip;
        aud.Play();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit _raycasthit;
        Physics.Raycast(_playerEyes.position, _playerEyes.transform.forward, out _raycasthit, 100f);
        Debug.DrawRay(_playerEyes.position, _playerEyes.transform.forward * 100, Color.red);
        if (_raycasthit.collider != null)
        {
            aud.volume = 0.1f;
            aud.pitch = 3 / _raycasthit.distance;
            Debug.Log(_raycasthit.collider.tag + "  " + _raycasthit.distance);
        }
        else
        {
            aud.volume = 0;
        }
    }

    void OnAudioRead(float[] data)
    {
        int count = 0;
        while (count < data.Length)
        {
            data[count] = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * frequency * position / samplerate));
            position++;
            count++;
        }
    }
    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }
}
