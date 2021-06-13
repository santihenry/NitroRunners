using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CarControllerV2 : MonoBehaviour
{

    public Dictionary<KeyCode, ICommand> keysCommands = new Dictionary<KeyCode, ICommand>();
    public List<ICommand> allCommands = new List<ICommand>();


    public Rigidbody rb;
    public float forwardAccel = 8;
    public float reverseAccel = 4;
    public float maxSpeed = 50;
    public float turnStrength = 180;
    public float gravity;

    public bool OnGround;
    public float SpeedInput, turnInput;
    public GameObject model;
    float startAccel;
    public GameObject frontRigthWheel;
    public GameObject frontLefthhWheel;
    public GameObject backRigthWheel;
    public GameObject backLefthhWheel;
    public Vector3 offsett;
    [Range(0,.40f)]
    public float offsettSup;
    RaycastHit hitFrontRigthWheel;
    RaycastHit hitFrontLeftWheel;
    RaycastHit hitBackRigthWheel;
    RaycastHit hitBackLeftWheel;
    RaycastHit hitMedio;

    public LayerMask floorLayer;

    public GameObject rayPos;

    [Header("ACTUALIAZAR POSISION RUEDAS")]
    public bool UpdateWheelPositions;

    public bool Stuned;
    public float stunedTime;
    public float currentTimeStuned;

    public float timeBoost;
    public bool _handBrake;

    public bool boosting;

    private float horizontal;
    private float vertical;


    public bool isMotorcycle;
    public bool _4wheelsVehicle;
    private float _currentTimeBoost;

    public CamerMovement cam;

    public GameObject cameraPos;
    public GameObject cameraPosZoom;
    public GameObject camViewPoint;
    public GameObject camLookAt;
    public GameObject cameraBackPos;


    public GameObject rayo;
    public Transform spawnPoint;



    public bool canShoot;

    public void Awake()
    {
        cam = GameObject.Find("CameraFront").GetComponent<CamerMovement>();

        cam.SetOfflineCar(this);
        cam.SetCameraPos(cameraPos.transform);
        cam.SetViewPoint(camViewPoint.transform);
        cam.SetCameraLookAt(camLookAt.transform);
        cam.SetPosBackCamera(cameraBackPos.transform);
        cam.SetPosBackCamera(cameraBackPos.transform);
        cam.SetCameraPosZoom(cameraPosZoom.transform);
    }


    private void Start()
    {
        rb.transform.parent = null;
        startAccel = forwardAccel;
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Menu");


        if (Input.GetKeyDown(KeyCode.Q) && canShoot)
        {
            var r = Instantiate(rayo, spawnPoint.position, Quaternion.identity);
            r.GetComponent<LightingWave>().SetDir(transform.forward);
            r.GetComponent<LightingWave>().SetMaster(rb.transform);
        }



        vertical = Input.GetAxis("Vertical");

        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyUp(KeyCode.Space))
        {
            model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }


        frontRigthWheel.transform.localRotation = Quaternion.Euler(frontRigthWheel.transform.localRotation.eulerAngles.x, frontRigthWheel.transform.localRotation.eulerAngles.y, (turnInput * 35) - 180);
        frontLefthhWheel.transform.localRotation = Quaternion.Euler(frontLefthhWheel.transform.localRotation.eulerAngles.x, frontLefthhWheel.transform.localRotation.eulerAngles.y, (turnInput * 35) - 180);


        if (Stuned)
        {
            currentTimeStuned += Time.deltaTime;
            if (stunedTime - currentTimeStuned >= 0)
            {
                model.transform.rotation = Quaternion.Euler(model.transform.transform.rotation.eulerAngles + new Vector3(0, 1500 * Time.deltaTime, 0));
            }
            else
            {
                currentTimeStuned = 0;
                Stuned = false;
                model.transform.rotation = transform.rotation;
            }
        }
        else
        {
            Engine();
            Streting();
            Drift();
        }

        Boosting();

    }



    private void FixedUpdate()
    {

        if (Stuned) return;

        OnGround = Physics.Raycast(rayPos.transform.position, Vector3.down, out hitMedio, 1.3f, floorLayer) ? true : false;

        if (Mathf.Abs(SpeedInput) > 0 && OnGround)
        {
            rb.AddForce(transform.forward * SpeedInput);
            rb.drag = 4;
        }

        if (!OnGround)
        {
            rb.AddForce(transform.up * -gravity * 10000);
            rb.drag = -4;
        }
        else
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, hitMedio.normal) * transform.rotation;
            rb.drag = 4;
        }
        


        transform.position = rb.transform.position;
        rb.transform.rotation = transform.rotation;


        var frontWr = Physics.Raycast(frontRigthWheel.transform.position, Vector3.down, out hitFrontRigthWheel, 1, floorLayer);
        var frontWl = Physics.Raycast(frontLefthhWheel.transform.position, Vector3.down, out hitFrontLeftWheel, 1, floorLayer);
        var backWr = Physics.Raycast(backRigthWheel.transform.position, Vector3.down, out hitBackRigthWheel, 1, floorLayer);
        var backWl = Physics.Raycast(backLefthhWheel.transform.position, Vector3.down, out hitBackLeftWheel, 1, floorLayer);

        if (UpdateWheelPositions)
        {
            if (frontWr)
                frontRigthWheel.transform.position = hitFrontRigthWheel.point + new Vector3(0, offsettSup, 0);
            if (frontWl)
                frontLefthhWheel.transform.position = hitFrontLeftWheel.point + new Vector3(0, offsettSup, 0);
            if (backWr)
                backRigthWheel.transform.position = hitBackRigthWheel.point + new Vector3(0, offsettSup, 0);
            if (backWl)
                backLefthhWheel.transform.position = hitBackLeftWheel.point + new Vector3(0, offsettSup, 0);
        }



    }



    public void Boosting()
    {
        if (boosting)
        {
            _currentTimeBoost += Time.deltaTime;
            if (timeBoost - _currentTimeBoost <= 0)
            {
                boosting = false;
                _currentTimeBoost = 0;
            }
        }
    }

    public void Engine()
    {
        if (OnGround)
        {
            forwardAccel = boosting ? forwardAccel = startAccel + startAccel / 2 : startAccel;
            SpeedInput = boosting ? 1 * forwardAccel * 1000 : vertical * forwardAccel * 1000;
        }
        else
        {
            SpeedInput = 0;
        }
    }

    public void Streting()
    {
        turnInput = horizontal;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, turnInput * turnStrength * Time.deltaTime * rb.velocity.normalized.magnitude, 0));
    }

    public void Drift()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isMotorcycle)
            {
                if (horizontal == -1 && vertical != 0)
                {
                    model.transform.localRotation = Quaternion.Euler(new Vector3(0, -25, 0));
                    if (!_4wheelsVehicle) model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -25));
                }
                if (horizontal == 1 && vertical != 0)
                {
                    model.transform.localRotation = Quaternion.Euler(new Vector3(0, 25, 0));

                    if (!_4wheelsVehicle) model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 25));
                }
            }
            else
            {
                if (horizontal == -1 && vertical != 0)
                {
                    model.transform.localRotation = Quaternion.Euler(new Vector3(0, 25, 0));
                    if (!_4wheelsVehicle) model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 25));
                }
                if (horizontal == 1 && vertical != 0)
                {
                    model.transform.localRotation = Quaternion.Euler(new Vector3(0, -25, 0));

                    if (!_4wheelsVehicle) model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -25));
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }







    

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ArrowNitro>())
        {
            boosting = true;
            _currentTimeBoost = 0;
        }

    }


    private void OnDrawGizmos()
    {
        /*
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3);

        Gizmos.color = OnGround ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position - new Vector3(0,1.3f,0));

        /// WHEELS

        //  Front Rigth
        Gizmos.color = OnGround ? Color.cyan : Color.red;
        Gizmos.DrawSphere(frontRigthWheel.transform.position + offsett, .2f);
        Gizmos.DrawLine(hitFrontRigthWheel.point, frontRigthWheel.transform.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(hitFrontRigthWheel.point, .05f);

        //  Front Lefth
        Gizmos.color = OnGround ? Color.cyan : Color.red;
        Gizmos.DrawSphere(frontLefthhWheel.transform.position + offsett, .2f);
        Gizmos.DrawLine(hitFrontLeftWheel.point, frontLefthhWheel.transform.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(hitFrontLeftWheel.point, .05f);

        //  Back Rigth
        Gizmos.color = OnGround ? Color.cyan : Color.red;
        Gizmos.DrawSphere(backRigthWheel.transform.position + offsett, .2f);
        Gizmos.DrawLine(hitBackRigthWheel.point, backRigthWheel.transform.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(hitBackRigthWheel.point, .05f);
        //  Back Lefth
        Gizmos.color = OnGround ? Color.cyan : Color.red;
        Gizmos.DrawSphere(backLefthhWheel.transform.position + offsett, .2f);
        Gizmos.DrawLine(hitBackLeftWheel.point, backLefthhWheel.transform.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(hitBackLeftWheel.point, .05f);
        */
    }


}
