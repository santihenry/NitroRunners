using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class CamerMovement : MonoBehaviour
{
    private CarModel _car;
    private Transform _cameralookAt, _cameraPos, _viewPoint, _backCamPos, _cameraPosZoom;
    private float speed;
    [Range(0, 100)] public float smothTime;


    bool backCamera;
    bool zoom;

    public List<Sprite> sprites = new List<Sprite>();
    public Image img;

    Camera _cam;


    float delay = .02f;
    float currentTimeDelay;
    int randSprite;

    public bool offline;
    private CarControllerV2 _carOffline;


    public List<Transform> waypoints = new List<Transform>();


    public GameObject firtThreePositions;
    public GameObject laps;
    public GameObject drift;

    public CarModel Car
    {
        get
        {
            return _car;
        }
    }
   

    public CamerMovement(Transform cameraLookAt, Transform cameraPos, Transform viewPoint)
    {
        _cameralookAt = cameraLookAt;
        _cameraPos = cameraPos;
        _viewPoint = viewPoint;
    }


    public CamerMovement SetCameraLookAt(Transform cameraLookAt)
    {
        _cameralookAt = cameraLookAt;
        return this;
    }

    
    public CamerMovement SetCameraPos(Transform cameraPos)
    {
        _cameraPos = cameraPos;
        return this;
    }

    public CamerMovement SetViewPoint(Transform viewPoint)
    {
        _viewPoint = viewPoint;
        return this;
    }

    public CamerMovement SetPosBackCamera(Transform backCam)
    {
        _backCamPos = backCam;
        return this;
    }

    public CamerMovement SetCameraPosZoom(Transform cameraPosZoom)
    {
        _cameraPosZoom = cameraPosZoom;
        return this;
    }

    public CamerMovement SetCar(CarModel car)
    {
        _car = car;
        return this;
    }


    public CamerMovement SetOfflineCar(CarControllerV2 car)
    {
        _carOffline = car;
        return this;
    }


    Vector3 startPosfirtThreePositions;
    Vector3 startPosLaps;
    Vector3 startPosDrift;
    public int currWay;
    public float speedPrecentacion;



    private void Start()
    {
        _cam = GetComponent<Camera>();
        img.gameObject.SetActive(false);
        startPosfirtThreePositions = firtThreePositions.transform.position;
        startPosLaps = laps.transform.position;
        startPosDrift = drift.transform.position;
        firtThreePositions.transform.position -= new Vector3(1000, 0, 0);
        laps.transform.position += new Vector3(0, 200, 0);
        drift.transform.position += new Vector3(400, 0, 0);
    }

    private void Update()
    {

        if (!offline && !RaceManager.Instance.startSemaforo)
        {
            var dir = waypoints[currWay].position - transform.position;
            transform.position += dir.normalized * speedPrecentacion * Time.deltaTime;

            if (currWay < waypoints.Count - 2)
            {
                if (Vector3.Distance(waypoints[currWay].position, transform.position) < 1)
                {
                    currWay++;
                }
            }
            else
            {
                currWay = 0;
                RaceManager.Instance.StartSemafoto();
                GameManager.Instance.miniMap.SetActive(true);
            }
        }
        else
        {
            firtThreePositions.transform.position = startPosfirtThreePositions;
            laps.transform.position = startPosLaps;
            drift.transform.position = startPosDrift;

            if (!offline)
            {
                if (_car.Bosting)
                {
                    if (currentTimeDelay == 0)
                        currentTimeDelay += Time.deltaTime;
                    else
                        currentTimeDelay = 0;

                    img.gameObject.SetActive(true);

                    if (_cam.fieldOfView < 70)
                        _cam.fieldOfView += 1f;


                    if (delay - currentTimeDelay <= 0)
                    {
                        randSprite = Random.Range(0, sprites.Count);
                        img.sprite = sprites[randSprite];
                        currentTimeDelay = 0;
                    }

                }
                else
                {
                    if (currentTimeDelay == 0)
                        currentTimeDelay += Time.deltaTime;
                    else
                        currentTimeDelay = 0;

                    if (_cam.fieldOfView > 60)
                        _cam.fieldOfView -= 1f;
                    else
                    {
                        img.gameObject.SetActive(false);
                        _cam.fieldOfView = 60;
                    }


                    if (delay - currentTimeDelay <= 0)
                    {
                        randSprite = Random.Range(0, sprites.Count);
                        img.sprite = sprites[randSprite];
                        currentTimeDelay = 0;
                    }
                }
            }
            else
            {
                if (_carOffline.boosting)
                {
                    if (currentTimeDelay == 0)
                        currentTimeDelay += Time.deltaTime;
                    else
                        currentTimeDelay = 0;

                    img.gameObject.SetActive(true);

                    if (_cam.fieldOfView < 70)
                        _cam.fieldOfView += 1f;


                    if (delay - currentTimeDelay <= 0)
                    {
                        randSprite = Random.Range(0, sprites.Count);
                        img.sprite = sprites[randSprite];
                        currentTimeDelay = 0;
                    }

                }
                else
                {
                    if (currentTimeDelay == 0)
                        currentTimeDelay += Time.deltaTime;
                    else
                        currentTimeDelay = 0;

                    if (_cam.fieldOfView > 60)
                        _cam.fieldOfView -= 1f;
                    else
                    {
                        img.gameObject.SetActive(false);
                        _cam.fieldOfView = 60;
                    }


                    if (delay - currentTimeDelay <= 0)
                    {
                        randSprite = Random.Range(0, sprites.Count);
                        img.sprite = sprites[randSprite];
                        currentTimeDelay = 0;
                    }
                }
            }
        }
    }


    private void FixedUpdate()
    {
        if (_car == null && !offline || !RaceManager.Instance.startSemaforo) return;
        speed = smothTime;

        if (Input.GetKeyDown(KeyCode.V))
        {
            backCamera = !backCamera;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            zoom = !zoom;
        }

        var pos = !backCamera ? _cameraPos : _backCamPos;
        if (!backCamera && zoom)
        {
            pos = _cameraPosZoom;
        }
           
        transform.position = Vector3.Lerp(transform.position, pos.transform.position, Time.deltaTime * speed);
        transform.LookAt(_viewPoint.transform.position);
    }


    private void OnDrawGizmos()
    {
        if (_cameralookAt == null || _viewPoint == null || _cameraPos == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _cameralookAt.transform.position);
        Gizmos.DrawSphere(transform.position, .1f);
        Gizmos.DrawSphere(_cameralookAt.transform.position, .1f);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(_cameraPos.transform.position, _cameralookAt.transform.position);
        Gizmos.DrawSphere(_cameraPos.transform.position, .1f);
        Gizmos.DrawSphere(_cameralookAt.transform.position, .1f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_viewPoint.transform.position, .1f);

    }
}

