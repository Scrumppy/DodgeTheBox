using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Hazard : MonoBehaviour
{
    public ParticleSystem destroyedParticles;
    public AudioSource destroyedSound;
    public AudioClip[] soundClipArray;
    public AudioClip soundClip;

    [SerializeField]
    private CinemachineImpulseSource cinemachineImpulseSource;
    private Player player;

    Vector3 rotation;
    private void Start()
    {
        var randRotation = Random.Range(90f, 180f);
        rotation = new Vector3(-randRotation, 0);
        //cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        player = FindObjectOfType<Player>();

        destroyedSound = gameObject.GetComponent<AudioSource>();
        SortRandomSound();
    }
    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Hazard"))
        {
            destroyedSound.Play();
            Destroy(gameObject, 0.08f);
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            if (player != null)
            {
                var distance = Vector3.Distance(transform.position, player.transform.position);
                var force = 1 / distance;

                cinemachineImpulseSource.GenerateImpulse(force);
            }
        }
    }

    private void SortRandomSound()
    {
        int index = Random.Range(0, soundClipArray.Length);
        soundClip = soundClipArray[index];
        destroyedSound.clip = soundClip;
        destroyedSound.volume = 0.05f;
    }
}
