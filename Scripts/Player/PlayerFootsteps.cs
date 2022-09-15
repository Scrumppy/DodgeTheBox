using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField]
    private AudioSource footstepSound;
    [SerializeField]
    private ParticleSystem destroyedParticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grass"))
        {
            footstepSound.Play();
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);
        }
    }
}
