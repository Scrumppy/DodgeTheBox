using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    public float forceMultiplier = 3f;
    public float maxVelocity = 3f;
    public ParticleSystem deathParticles;
    //public CinemachineVirtualCamera mainVCam;
    //public CinemachineVirtualCamera zoomVCam;

    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = 45;
#endif
    }

    void Start()
    {
        //rigidBody = GetComponent<Rigidbody>();
        //cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        float horizontalInput = 0;

        //Is using mouse or mobile screen
        if (Input.GetMouseButton(0))
        {
            var screenCenter = Screen.width / 2;
            var mousePosition = Input.mousePosition;

            //Clicking on the right side of the screen
            if (mousePosition.x > screenCenter)
            {
                horizontalInput = 1;
            }
            //Clicking on the left side of the screen
            else if (mousePosition.x < screenCenter)
            {
                horizontalInput = -1;
            }
        }
        //Is using keyboard
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        if (rigidBody.velocity.magnitude <= maxVelocity)
        {
            rigidBody.AddForce(new Vector3(horizontalInput * forceMultiplier * Time.deltaTime, 0, 0)); 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            GameOver();
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            cinemachineImpulseSource.GenerateImpulse();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FallTrigger"))
        {
            GameOver();
            Instantiate(deathParticles, transform.position, Quaternion.identity);
        }
    }

    private void GameOver()
    {
        GameManager.Instance.GameOver();
        gameObject.SetActive(false);
    }
}
