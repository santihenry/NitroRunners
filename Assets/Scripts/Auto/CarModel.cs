using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class CarModel : MonoBehaviourPun
{

    public bool isMotorcycle;

    public Dictionary<KeyCode, ICommand> keysCommands = new Dictionary<KeyCode, ICommand>();
    public List<ICommand> allCommands = new List<ICommand>();
    [Header("Rotar el vehiculo en eje x")]
    public bool _4wheelsVehicle;
    float _lerp;
    public float LerpPP { get { return _lerp; } set { _lerp = value; } }
    public TMP_Text positionTxt;
    int distToMeta;
    int pos;

    bool _inmortal;



    public int ID
    {
        get
        {
            return photonView.ViewID;
        }
    }

    public bool Inmortality
    {
        get
        {
            return _inmortal;
        }
        set
        {
            _inmortal = value;
        }
    }

    public int DistToMeta
    {
        get
        {
            return distToMeta;
        }
        set
        {
            distToMeta = value;
        }
    }

    public int Pos
    {
        get
        {
            return pos;
        }
        set
        {
            pos = value;
        }
    }

    [PunRPC]
    public void ChangeNick(string name)
    {
        var n = name.ToLower();
        //nick.text = n;
        nickName = n;
    }

    public string nickName;
    //public TMP_Text nick;
    [SerializeField]
    public List<IObserver> Observers = new List<IObserver>();
    [Header("Valores Para Modificar")]
    [Header("Motor")]

    [Header("Respawn")]
    public float currentTimeInvert;
    public float timeRespawn;
    public bool invert;

    [Header("Freno")]




    [Header("Valores Para ver")]


    private WayPoints _waypoints;
    public Transform _currentWaypoint;
    public List<Transform> _waypointsList = new List<Transform>();
    private float sterrForce = .6f;
    [Header("AI stats")]
    public int currentWay;
    [Range(0, 1)] public float acceletation;


    [Header("Weapons")]

    public string weapon;
    public Dictionary<int, string> pathDic = new Dictionary<int, string>();
    public int maxItms = 1;
    private bool _Wall, _misile, _oil, _bomb;
    public string wallPath, rocketPath, bombPath, fieldPath,zapPath,scanPath,dronePath;
    public Transform sapwnPoint;
    public Transform sapwnPointZap;
    public Stack<string> weaponStack = new Stack<string>();
    public int cantItem;
    bool haveItem;




    public List<Transform> nearWaypont = new List<Transform>();
    public CamerMovement cam;
    public UImanager ui;
    int lap;
    public bool wrongWay;
    public Vector3 startPos;
    public Roulete ruleta;


    //  //////////////////



    public vehiclesData statsData;


    public GameObject cameraPos;
    public GameObject cameraPosZoom;
    public GameObject camViewPoint;
    public GameObject camLookAt;
    public GameObject cameraBackPos;

    
    public Rigidbody _rb;
    private bool boosting;
    public GameObject model;
    private float startAccel;


    [Header("Stats")]

    [SerializeField]
    private float forwardAccel;
    [SerializeField]
    private float reverseAccel;
    [SerializeField]
    private float maxSpeed;
    public float gravity;
    private float turnStrength;
    private float horizontal;
    private float vertical;
    private bool onGround;
    private float speedInput, turnInput;
    [Header("Ruedas")]
    public Vector3 offsett;
    [Range(0, .40f)]
    public float offsettSup;
    public GameObject frontRigthWheel;
    public GameObject frontLefthhWheel;
    public GameObject backRigthWheel;
    public GameObject backLefthhWheel;
    public RaycastHit hitFrontRigthWheel;
    public RaycastHit hitFrontLeftWheel;
    public RaycastHit hitBackRigthWheel;
    public RaycastHit hitBackLeftWheel;
    public RaycastHit hitMedio;
    public LayerMask floorLayer;
    public LayerMask TrackLayer;
    public GameObject rayPos;
    [Header("ACTUALIAZAR POSISION RUEDAS")]
    public bool updateWheelPositions;


    private bool stuned;
    public float stunedTime;
    public float currentTimeStuned;
    public float StunedRotationSpeed=1000;

    public float timeBoost;
    private bool _handBrake;


    public float currentTimeBoost;

    private bool _drift;


    public bool Drift
    {
        get
        {
            return _drift;
        }

        set
        {
            _drift = value;
        }
    }

    public bool Handbracke
    {
        get
        {
            return _handBrake;
        }
        set
        {
            photonView.RPC("HandBrakeRpc", RpcTarget.All, value);
        }
    }


    public bool Stuned
    {
        get
        {
            return stuned;
        }
        set
        {
            stuned = value;         
        }
    }


    public float TurnStrength
    {
        get
        {
            return turnStrength;
        }
        set
        {
            turnStrength = value;
        }
    }

    public float SpeedInput
    {
        get
        {
            return speedInput;
        }
        set
        {
            speedInput = value;
        }
    }

    public float TurtnInput
    {
        get
        {
            return turnInput;
        }
        set
        {
            turnInput = value;
        }
    }


    public float StartAccel
    {
        get
        {
            return startAccel;
        }
        set
        {
            startAccel = value;
        }
    }

    public float ForwardAccel
    {
        get
        {
            return forwardAccel;
        }
        set
        {
            forwardAccel = value;
        }
    }

    public float ReverseAccel
    {
        get
        {
            return reverseAccel;
        }
        set
        {
            reverseAccel = value;
        }
    }








    public bool HaveItem
    {
        get
        {
            return haveItem;
        }
        set
        {
            haveItem = value;
        }
    }

    public bool CanTakeItem
    {
        get
        {

            return haveItem ? false : true;
        }
    }

    public bool GetWrongway
    {
        get
        {
            return wrongWay;
        }
    }


    public Rigidbody Rigidbody
    {
        get
        {
            return _rb;
        }
        set
        {
            _rb = value;
        }
    }


    public float Horizontal
    {
        get
        {
            return horizontal;
        }
        set
        {
            horizontal = value;
        }
    }

    public float Vertical
    {
        get
        {
            return vertical;
        }
        set
        {
            vertical = value;
        }
    }

    public bool Bosting
    {
        get
        {
            return boosting;
        }
        set
        {
            currentTimeBoost = 0;
            photonView.RPC("BoostingRpc", RpcTarget.All, value);
        }
    }


    [PunRPC]
    public void BoostingRpc(bool v)
    {
        boosting = v;
    }

    [PunRPC]
    public void HandBrakeRpc(bool v)
    {
        _handBrake = v;
    }




    public float MaxSpeed
    {
        get
        {
            return maxSpeed;
        }
    }

    public int Lap
    {
        set
        {
            lap = value;
        }
        get
        {
            return lap;
        }
    }



    public bool OnGround
    {
        get
        {
            return onGround;
        }
        set
        {
            onGround = value;
        }

    }




    private void Awake()
    {
        positionTxt = GameObject.Find("PositionTxt").gameObject.GetComponent<TMP_Text>();
        if (!photonView.IsMine) return;
        cam = GameObject.Find("CameraFront").GetComponent<CamerMovement>();
        ui = FindObjectOfType<UImanager>();

        cam.SetCar(this);
        cam.SetCameraPos(cameraPos.transform);
        cam.SetViewPoint(camViewPoint.transform);
        cam.SetCameraLookAt(camLookAt.transform);
        cam.SetPosBackCamera(cameraBackPos.transform);
        cam.SetPosBackCamera(cameraBackPos.transform);
        cam.SetCameraPosZoom(cameraPosZoom.transform);


    }



    private void Start()
    {
        _waypoints = FindObjectOfType<WayPoints>();
        _currentWaypoint = transform;
        if (_waypoints != null)
            _waypointsList = _waypoints.nodes;
        
    }

}




