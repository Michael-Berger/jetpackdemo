using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float accelerationPower;
    public float defaultMaxSpeed;
    public float jumpPower;
    public float jetpackPower;
    public float tiltPower;
    public float landingTimer;

    public Transform body;
    public ParticleSystem jetpackParticles;
    public LayerMask layerMask;

    private Quaternion defaultRotation;
    private float maxSpeed;
    private bool landing = false;
    private bool grounded = false;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        maxSpeed = defaultMaxSpeed;
    }
    
    void Start () {
        defaultRotation = transform.rotation;
	}
	
	void Update () {

        GroundCheck();

		//check for movement presses
        if (Input.GetAxis("Horizontal") != 0 && grounded && !landing)
        {
            MoveDirection(Input.GetAxis("Horizontal"));
        } else if (Input.GetAxis("Horizontal") != 0 && !grounded && !landing)
        {
            TiltDirection(Input.GetAxis("Horizontal"));
        }

        // Cap speed
        if (rigidBody.velocity.magnitude > maxSpeed)
        {
            rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
        }


        if (Input.GetKeyDown(KeyCode.Space) && grounded && !landing)
        {
            Jump();
        }

        if (Input.GetMouseButton(0) && !landing)
        {
            Jetpack();

            if (jetpackParticles != null)
            {
                ParticleSystem.EmissionModule jetpackEmission = jetpackParticles.emission;
                jetpackEmission.rateOverTime = 40;

            }
        } else
        {
            if (jetpackParticles != null)
            {
                ParticleSystem.EmissionModule jetpackEmission = jetpackParticles.emission;
                jetpackEmission.rateOverTime = 0;
            }
        }
	}

    private void MoveDirection(float direction)
    {
        rigidBody.AddForce(Vector3.right * direction * accelerationPower);

        if (direction > 0)
        {
            body.localEulerAngles = new Vector3(0, 180, 0);
        } else if (direction < 0)
        {
            body.localEulerAngles = Vector3.zero;
        }
    }

    private void Jump()
    {
        rigidBody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    private void Jetpack()
    {
        rigidBody.AddForce(transform.up * jetpackPower);
    }

    private void TiltDirection(float direction)
    {
        rigidBody.AddTorque(transform.forward * -direction * tiltPower);
    }

    private void GroundCheck()
    {
        //raycast down to see if grounded or not.
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, 1f, layerMask))
        {
            if (!grounded)
            {
                StartCoroutine(StraightenPlayer());
            }

            grounded = true;
            rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            maxSpeed = defaultMaxSpeed;
        } else
        {
            grounded = false;
            maxSpeed = defaultMaxSpeed * 20;
            rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;
        }
    }

    private IEnumerator StraightenPlayer()
    {
        landing = true;
        float timer = 0;

        Quaternion currentRotation = transform.rotation;

        while (timer < landingTimer) {
            timer += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(currentRotation, defaultRotation, timer / landingTimer);
            yield return null;
        }

        landing = false;
    }
}
