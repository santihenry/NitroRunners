using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;


public class CarController : MonoBehaviourPun, IObservable
{
    private CarModel _carModel;


    public CarModel Model
    {
        get
        {
            return _carModel;
        }
    }

    private void Awake()
    {
        //if (!_carModel.photonView.IsMine) return;
        _carModel = GetComponent<CarModel>();
        _carModel.ruleta = FindObjectOfType<Roulete>(true);
        _carModel.keysCommands.Add(KeyCode.W, new FowardCommand());
        _carModel.keysCommands.Add(KeyCode.S, new BackCommand());
        _carModel.keysCommands.Add(KeyCode.A, new LeftCommand());
        _carModel.keysCommands.Add(KeyCode.D, new RigthCommand());
        _carModel.keysCommands.Add(KeyCode.UpArrow, new FowardCommand());
        _carModel.keysCommands.Add(KeyCode.DownArrow, new BackCommand());
        _carModel.keysCommands.Add(KeyCode.LeftArrow, new LeftCommand());
        _carModel.keysCommands.Add(KeyCode.RightArrow, new RigthCommand());
        _carModel.keysCommands.Add(KeyCode.Space, new HandbrakeCommand());
        _carModel.keysCommands.Add(KeyCode.LeftShift, new AtackCommand());
        _carModel.keysCommands[KeyCode.LeftShift].Init(_carModel.ruleta.gameObject);
        _carModel.keysCommands[KeyCode.W].Init(_carModel.gameObject);
        _carModel.keysCommands[KeyCode.S].Init(_carModel.gameObject);
        _carModel.keysCommands[KeyCode.A].Init(_carModel.gameObject);
        _carModel.keysCommands[KeyCode.D].Init(_carModel.gameObject);
        _carModel.keysCommands[KeyCode.UpArrow].Init(_carModel.gameObject);
        _carModel.keysCommands[KeyCode.DownArrow].Init(_carModel.gameObject);
        _carModel.keysCommands[KeyCode.LeftArrow].Init(_carModel.gameObject);
        _carModel.keysCommands[KeyCode.RightArrow].Init(_carModel.gameObject);
        

    }



    public Image driftImg;
    public Image marcoDriftImg;

    private void Start()
    {
        //_carModel.nick.gameObject.SetActive(false);
        _carModel.Rigidbody.transform.parent = null;
        if (!_carModel.photonView.IsMine) return;
        _carModel.photonView.RPC("ChangeNick", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);


        //var x = PhotonNetwork.Instantiate("TESTUI", Vector3.zero, Quaternion.identity);
        //x.GetPhotonView().RPC("ChangeId", RpcTarget.AllBuffered, _carModel.photonView.ViewID);


        _carModel.startPos = transform.position;
        _carModel.pathDic.Add(0, _carModel.rocketPath);
        _carModel.pathDic.Add(1, _carModel.wallPath);
        _carModel.pathDic.Add(2, _carModel.bombPath);
        _carModel.pathDic.Add(3, _carModel.fieldPath);
        _carModel.pathDic.Add(4, _carModel.zapPath);
        _carModel.pathDic.Add(5, _carModel.scanPath);
        _carModel.pathDic.Add(6, _carModel.dronePath);

        _carModel.StartAccel = _carModel.ForwardAccel;


        driftImg = GameObject.Find("FuegoNitro").GetComponent<Image>();
        marcoDriftImg = GameObject.Find("Nitro").GetComponent<Image>();
        calificacionDriftTxt = GameObject.Find("CalificacionDrift").GetComponent<TMP_Text>();
        multipliDriftTxt = GameObject.Find("MultipliDriftTxt").GetComponent<TMP_Text>();

    }


    [PunRPC]
    public void Win()
    {
        //GameManager.Instance.nameWin = _carModel.nick.text;
        GameManager.Instance.nameWin = _carModel.nickName;
        GameManager.Instance.finishRace = true;
    }

    [PunRPC]
    public void Finish()
    {
        GameManager.Instance.ganadores.Add(_carModel.nickName);
        GameManager.Instance.ganadoresTime.Add($"{GetComponent<Laps>().h} : {GetComponent<Laps>().m} : {GetComponent<Laps>().s}");
    }


    [PunRPC]
    public void StunedRPC(bool value)
    {
        _carModel.Stuned = value;
    }


    public bool hideNick;


    [PunRPC]
    public void DisableCar()
    {
        gameObject.SetActive(false);
    }
   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //_carModel.nick.gameObject.SetActive(!_carModel.nick.gameObject.activeSelf);
            hideNick = !hideNick;
        }

        if (!_carModel.photonView.IsMine) return;

        if (GameManager.Instance.finishRace)
        {
            //gameObject.SetActive(false);
            if (gameObject.activeSelf)
            {
                _carModel.photonView.RPC("DisableCar", RpcTarget.Others);
            }
            AIDrive();
            Engine();
            Streting();
        }
        else
        {
            foreach (var comand in _carModel.keysCommands)
            {
                if (Input.GetKey(comand.Key))
                {
                    comand.Value.Execute(_carModel.gameObject);
                    _carModel.allCommands.Add(comand.Value);
                }
                if (Input.GetKeyUp(comand.Key))
                {
                    comand.Value.Stop(_carModel.gameObject);
                    _carModel.allCommands.Add(comand.Value);
                }
            }


            if (GameManager.Instance.Online && _carModel.Lap > int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["Laps"].ToString()) && !GameManager.Instance.finishRace)
            {
                GameManager.Instance.finishRace = true;
                _carModel.photonView.RPC("Finish", RpcTarget.AllBuffered);
            }

            if (!RaceManager.Instance.StartRace)
            {
                transform.position = new Vector3(_carModel.startPos.x, transform.position.y, _carModel.startPos.z);
                return;
            }

            if (RaceManager.Instance.StartRace && !GameManager.Instance.finishRace)
            {
                _carModel.positionTxt.text = $"{_carModel.Pos}<size=55>°.</size>"; 
                _carModel.Lap = GetComponent<Laps>().lap;
                _carModel.ui.SetCarTgt(_carModel);
                _carModel.photonView.RPC("UpdateLape", RpcTarget.All,_carModel.Lap);
            }
            else
            {
                _carModel.positionTxt.text = "";
            }


            if (_carModel.ruleta.finish && _carModel.HaveItem)
            {
                _carModel.weapon = _carModel.pathDic[_carModel.ruleta.itemWin];
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                _carModel.Stuned = !_carModel.Stuned;
                _carModel.Rigidbody.velocity = _carModel.Rigidbody.transform.forward * 100;
            }

            if (_carModel.Stuned && !_carModel.Inmortality)
            {
                _carModel.currentTimeStuned += Time.deltaTime;
                _carModel.StunedRotationSpeed -= Time.deltaTime*2;
                if(_carModel.stunedTime - _carModel.currentTimeStuned >= 0)
                {
                    _carModel.model.transform.rotation = Quaternion.Euler(_carModel.model.transform.transform.rotation.eulerAngles + new Vector3(0, _carModel.StunedRotationSpeed * Time.deltaTime, 0));
                    
                    Notify("StunnedOn");
                }
                else
                {
                    _carModel.currentTimeStuned = 0;
                    _carModel.Stuned = false;
                    Notify("StunnedOff");
                    _carModel.model.transform.rotation = _carModel.transform.rotation;
                }
            }
            else
            {
                Engine();
                Streting();
                Drift();
                CheckInvert();
                Wrongway();
            }

            DistanceOfWaypoints();
            Boosting();


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var item = PhotonNetwork.Instantiate(_carModel.zapPath, _carModel.sapwnPointZap.position, _carModel.sapwnPointZap.rotation);
                item.GetPhotonView().RPC("ChangeId", RpcTarget.AllBuffered,_carModel.photonView.ViewID);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                var item = PhotonNetwork.Instantiate(_carModel.dronePath, _carModel.sapwnPointZap.position, _carModel.sapwnPointZap.rotation);
                item.GetPhotonView().RPC("ChangeId", RpcTarget.AllBuffered, _carModel.photonView.ViewID);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                var item = PhotonNetwork.Instantiate(_carModel.rocketPath, _carModel.sapwnPoint.position, _carModel.sapwnPoint.rotation);
                item.GetPhotonView().RPC("ChangeId", RpcTarget.AllBuffered, _carModel.photonView.ViewID);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                var item = PhotonNetwork.Instantiate(_carModel.fieldPath, _carModel.sapwnPoint.position, _carModel.sapwnPoint.rotation);
                item.GetPhotonView().RPC("ChangeId", RpcTarget.AllBuffered, _carModel.photonView.ViewID);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                var item = PhotonNetwork.Instantiate(_carModel.wallPath, _carModel.sapwnPoint.position, _carModel.sapwnPoint.rotation);
                item.GetPhotonView().RPC("ChangeId", RpcTarget.AllBuffered, _carModel.photonView.ViewID);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                var item = PhotonNetwork.Instantiate(_carModel.scanPath, _carModel.sapwnPoint.position, _carModel.sapwnPoint.rotation);
                item.GetPhotonView().RPC("ChangeId", RpcTarget.AllBuffered, _carModel.photonView.ViewID);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                var item = PhotonNetwork.Instantiate(_carModel.bombPath, _carModel.sapwnPoint.position, _carModel.sapwnPoint.rotation);
                item.GetPhotonView().RPC("ChangeId", RpcTarget.AllBuffered, _carModel.photonView.ViewID);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (GetComponent<MultiMisiles>() != null)
                    GetComponent<MultiMisiles>().Shoot();
                Notify("Special");
            }
        }
    }

    

    public void Boosting()
    {
        if (_carModel.Bosting)
        {
            _carModel.currentTimeBoost += Time.deltaTime;
            if(_carModel.timeBoost - _carModel.currentTimeBoost <= 0 || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || !_carModel.OnGround)
            {
                _carModel.Bosting = false;
                _carModel.currentTimeBoost = 0;
            }
        }
    }


    private void FixedUpdate()
    {

        if (!_carModel.photonView.IsMine) return;
        if (_carModel.Stuned && !_carModel.Inmortality) return;

        _carModel.OnGround = Physics.Raycast(_carModel.rayPos.transform.position, Vector3.down, out _carModel.hitMedio, 1.3f, _carModel.TrackLayer) ? true : false;

        if (Mathf.Abs(_carModel.SpeedInput) > 0 /*&& _carModel.OnGround*/)
        {
            _carModel.Rigidbody.AddForce(transform.forward * _carModel.SpeedInput);
        }

        if (!_carModel.OnGround)
        {
            /*if(!_carModel.Bosting)
                _carModel.Rigidbody.AddForce(transform.up * -_carModel.gravity * 1000);
            else
                _carModel.Rigidbody.AddForce(transform.up * -_carModel.gravity * 400);*/

            _carModel.Rigidbody.AddForce(transform.up * -_carModel.gravity * 1800,ForceMode.Force);
        }
        else
        {
            //if(!_carModel.Bosting)
                transform.rotation = Quaternion.FromToRotation(transform.up, _carModel.hitMedio.normal) * transform.rotation;
        }

        transform.position = _carModel.Rigidbody.transform.position;
        _carModel.Rigidbody.transform.rotation = transform.rotation;




        if (_carModel.updateWheelPositions)
        {
            var frontWr = Physics.Raycast(_carModel.frontRigthWheel.transform.position, Vector3.down, out _carModel.hitFrontRigthWheel, 1, _carModel.floorLayer);
            var frontWl = Physics.Raycast(_carModel.frontLefthhWheel.transform.position, Vector3.down, out _carModel.hitFrontLeftWheel, 1, _carModel.floorLayer);
            var backWr = Physics.Raycast(_carModel.backRigthWheel.transform.position, Vector3.down, out _carModel.hitBackRigthWheel, 1, _carModel.floorLayer);
            var backWl = Physics.Raycast(_carModel.backLefthhWheel.transform.position, Vector3.down, out _carModel.hitBackLeftWheel, 1, _carModel.floorLayer);

            if (frontWr)
                _carModel.frontRigthWheel.transform.position = _carModel.hitFrontRigthWheel.point + new Vector3(0, _carModel.offsettSup, 0);
            if (frontWl)
                _carModel.frontLefthhWheel.transform.position = _carModel.hitFrontLeftWheel.point + new Vector3(0, _carModel.offsettSup, 0);
            if (backWr)
                _carModel.backRigthWheel.transform.position = _carModel.hitBackRigthWheel.point + new Vector3(0, _carModel.offsettSup, 0);
            if (backWl)
                _carModel.backLefthhWheel.transform.position = _carModel.hitBackLeftWheel.point + new Vector3(0, _carModel.offsettSup, 0);
        }
      
    }

    float vel;

    public void Engine()
    {
        _carModel.ForwardAccel = _carModel.Bosting ? _carModel.ForwardAccel = _carModel.StartAccel + _carModel.StartAccel / 2 : _carModel.StartAccel;
        _carModel.SpeedInput = _carModel.Bosting ? _carModel.ForwardAccel * 1000 : _carModel.Vertical * _carModel.ForwardAccel * 1000;
        Notify("AcelerateAnim");
    }


    public void Streting()
    {
        _carModel.TurtnInput = _carModel.Horizontal;
        Debug.LogWarning("turn input  " + _carModel.Horizontal);
        _carModel.TurnStrength = _carModel.Drift ? _carModel.statsData.driftStrength : _carModel.statsData.turnStrength;
        if(_carModel.Rigidbody.velocity.normalized.magnitude > .2f && _carModel.Vertical != 0 || _carModel.Bosting)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, _carModel.TurtnInput * _carModel.TurnStrength * Time.deltaTime, 0));
        Notify("SteerAnim");
    }


    public float driftValue;
    public float multipler = 1;
    public TMP_Text calificacionDriftTxt;
    public TMP_Text multipliDriftTxt;


    public void Drift()
    {
        if (Input.GetKey(KeyCode.Space) && !_carModel.Stuned)
        {
            _carModel.Drift = true;
            multipler += .04f;
            if(driftValue < 1)
            {
                driftValue += Time.deltaTime * multipler;
                driftImg.material.SetFloat("_driftBar", driftValue);
            }
            else
            {
                Debug.Log("FAIL DRIFT !!");
                calificacionDriftTxt.text = $"fail";
                _carModel.photonView.RPC("StunedRPC", RpcTarget.All, true);
                _carModel.Drift = false;
                driftValue = 0;
                multipler = 0;
                driftImg.material.SetFloat("_driftBar", driftValue);
            }

            if (_carModel.Drift)
            {
                if (!_carModel.isMotorcycle)
                {
                    if (_carModel.Horizontal == -1 && _carModel.Vertical != 0)
                    {
                        _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, -25, 0));
                        if(!_carModel._4wheelsVehicle ) _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -25));
                    }
                    if (_carModel.Horizontal == 1 && _carModel.Vertical != 0)
                    {
                        _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 25, 0));

                        if (!_carModel._4wheelsVehicle ) _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 25));
                    }
                }
                else
                {
                    if (_carModel.Horizontal == -1 && _carModel.Vertical != 0)
                    {
                        _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 25, 0));
                        if (!_carModel._4wheelsVehicle) _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 25));
                    }
                    if (_carModel.Horizontal == 1 && _carModel.Vertical != 0)
                    {
                        _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, -25, 0));

                        if (!_carModel._4wheelsVehicle) _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -25));
                    }
                }
                Notify("HandBrake");
            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {

            _carModel.Drift = false;
            _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            if (driftValue < .70f)
            {
                _carModel.photonView.RPC("StunedRPC", RpcTarget.All, true);
                Debug.Log("FAIL DRIFT !!  " + driftValue);
                calificacionDriftTxt.text = $"fail";
            }
            else if(driftValue > .70f && driftValue < .9f)
            {
                Debug.Log("GOOD DRIF !!  " + driftValue);
                calificacionDriftTxt.text = $"good";
            }
            else if(driftValue > .9f && driftValue < 1)
            {
                Debug.Log("PERFECT DRIF !!   " + driftValue);
                calificacionDriftTxt.text = $"perfect";
            }
            driftValue = 0;
            multipler = 0;
            driftImg.material.SetFloat("_driftBar", driftValue);

        }
    }


    public void Horn()
    {
        Notify("Horn");
    }


    public void CheckInvert()
    {
        float t;

        if (_carModel.OnGround)
        { 
            respawnPos = _carModel.currentWay == 0 ? _carModel._waypointsList[_carModel.currentWay].transform.position :
                _carModel._waypointsList[_carModel.currentWay - 1].transform.position;
        }

        if (_carModel.invert)
        {
            _carModel.currentTimeInvert += Time.deltaTime;
            if (_carModel.timeRespawn - _carModel.currentTimeInvert < 0)
            {
                Respawn();
            }
        }
    }

    Vector3 respawnPos;

    public void Respawn()
    {
        _carModel.currentTimeInvert = 0;
        _carModel.invert = false;
        //_carModel._rb.transform.position = TakeNearWaypoint().position;
        _carModel._rb.transform.position = respawnPos;
        transform.rotation = Quaternion.identity;
    }

    public Transform TakeNearWaypoint()
    {
        foreach (var item in _carModel._waypointsList)
        {
            _carModel.nearWaypont.Add(item);
        }
        var way = SortWaypints();
        _carModel.nearWaypont = new List<Transform>();
        _carModel.nearWaypont.AddRange(way);
        return _carModel.nearWaypont[0];
    }

    private IEnumerable<Transform> SortWaypints()
    {
        return _carModel.nearWaypont.OrderBy(n => Vector3.Distance(transform.position, n.position));
    }



    [PunRPC]
    public void UpdateCurrentWay(int way)
    {
        _carModel.currentWay = way;
    }

    [PunRPC]
    public void UpdateLape(int lap)
    {
        _carModel.Lap = lap;
    }




    // //////////////////////////////////////////
    // IA DRIVE 
    // //////////////////////////////////////////
    ////////////////////////////////////////////

    public void DistanceOfWaypoints()
    {
        Vector3 position = transform.position;
        float distance = Mathf.Infinity;
        
        for (int i = 0; i < _carModel._waypointsList.Count; i++)
        {
            Vector3 difference = _carModel._waypointsList[i].transform.position - position;
            float currentDistance = difference.magnitude;
            if (currentDistance < distance)
            {
                if ((i + 1) >= _carModel._waypointsList.Count)
                {
                    _carModel._currentWaypoint = _carModel._waypointsList[1];
                    distance = currentDistance;
                }
                else
                {
                    _carModel._currentWaypoint = _carModel._waypointsList[i + 1];
                    distance = currentDistance;
                }
                _carModel.currentWay = i;
            }
        }
        photonView.RPC("UpdateCurrentWay", RpcTarget.All, _carModel.currentWay);
    }


    public void AIDrive()
    {
        DistanceOfWaypoints();
        AISteer();
        _carModel.Vertical  = 1;
    }


    private void AISteer()
    {
        Vector3 relative = transform.InverseTransformPoint(_carModel._currentWaypoint.transform.position);
        relative /= relative.magnitude;
        _carModel.Horizontal = (relative.x / relative.magnitude) * 1;//_carModel.TurnStrength;
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<ArrowNitro>() && !GameManager.Instance.finishRace)
        {
            _carModel.Bosting = true;
            _carModel.currentTimeBoost = 0;
        }

        if (!photonView.IsMine || GameManager.Instance.finishRace) return;

        if (other.GetComponent<PowerUp>() && _carModel.CanTakeItem)
        {
            _carModel.HaveItem = true;
            other.GetComponent<PowerUp>().TakeItem();
            _carModel.cantItem++;
        }

        if(other.gameObject.layer == 14)
        {
            _carModel.invert = true;
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.finishRace) return;

        if (collision.gameObject.layer == 9)
        {
            Notify("Hit");
        }
    }


  


    public void Wrongway()
    {
        _carModel.wrongWay = Vector3.Angle(transform.forward, transform.position - _carModel._currentWaypoint.position) > 75 ? false : true;
    }

    public void Notify(string eventName)
    {
        foreach (var item in _carModel.Observers)
        {
            item.OnNotify(eventName);
        }
    }

    public void SubEvent(IObserver obs)
    {
        _carModel.Observers.Add(obs);
    }

    public void UnSubEvent(IObserver obs)
    {
        _carModel.Observers.Remove(obs);
    }
}
