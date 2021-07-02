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

        startTimeBost = _carModel.timeBoost;

        _carModel.CurentPowerValue = _carModel.statsData.starPower;

    }


    [PunRPC]
    public void Win()
    {
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
                _carModel.Drift = false;
                _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
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
                _carModel.photonView.RPC("UpdateLape", RpcTarget.All, _carModel.Lap);
                _carModel.DeleyToUlti += Time.deltaTime;
                _carModel.timeToUlti += Time.deltaTime;

                if (_carModel.timeToUlti < 45)
                {
                    //_carModel.timeToUlti = _carModel.DeleyToUlti;
                }
                else
                {
                    _carModel.timeToUlti = 45;
                    _carModel.Ulti = true;
                }
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
                _carModel.Drift = false;
                _carModel.DriftValue = 0;
                _carModel.MultiplerVelue = 1;
                _carModel.TimeDriftBoost = 0;
                _carModel.driftImg.material.SetFloat("_driftBar", _carModel.DriftValue);

                if (_carModel.stunedTime - _carModel.currentTimeStuned >= 0)
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


            _carModel.ultimo = _carModel.Pos == PhotonNetwork.CurrentRoom.PlayerCount ? true : false;
                
            if (Input.GetKey(KeyCode.LeftShift))
            {
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
            }

            if (Input.GetKeyDown(KeyCode.Q) && _carModel.Ulti && _carModel.statsData.name != "rayer")
            {
                if (GetComponent<MultiMisiles>() != null)
                    GetComponent<MultiMisiles>().Shoot();

                Notify("Special");
                _carModel.Ulti = false;
            }
        }
    }


    float startTimeBost;

    public void Boosting()
    {
        if (_carModel.Bosting)
        {
            var timeBost = startTimeBost;
            _carModel.timeBoost = driftBoost ? _carModel.TimeDriftBoost : timeBost;

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

        if (Mathf.Abs(_carModel.SpeedInput) > 0 && _carModel.OnGround || _carModel.flyArea)
        {
            _carModel.Rigidbody.AddForce(transform.forward * _carModel.SpeedInput);
        }

        if (!_carModel.OnGround)
        {
            _carModel.Rigidbody.AddForce(transform.up * -_carModel.gravity * 1800,ForceMode.Force);
        }
        else
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, _carModel.hitMedio.normal) * transform.rotation;
        }

        transform.position = _carModel.Rigidbody.transform.position;
        _carModel.Rigidbody.transform.rotation = transform.rotation;



        if(_carModel.Vertical != 0)
        {
            currentTimeEngineOff = 0;
            currentTimeEngine += Time.fixedDeltaTime;
            if (_carModel.CurentPowerValue < _carModel.statsData.maxPower)
            {
                _carModel.CurentPowerValue += _carModel.statsData.increacePower  * Time.fixedDeltaTime;
            }
        }
        else
        {
            currentTimeEngine = 0;
            currentTimeEngineOff += Time.fixedDeltaTime;
            if (_carModel.CurentPowerValue > _carModel.statsData.starPower)
            {
                _carModel.CurentPowerValue -= _carModel.statsData.increacePower * 2  * Time.fixedDeltaTime;
            }
            else
            {
                _carModel.CurentPowerValue = _carModel.statsData.starPower;
            }
        }

    }

    float currentTimeEngine;
    float currentTimeEngineOff;

    public void Engine()
    {       
        _carModel.ForwardAccel = _carModel.Bosting ? _carModel.ForwardAccel = _carModel.StartAccel + _carModel.StartAccel / 2 : _carModel.StartAccel;
        _carModel.SpeedInput = _carModel.Bosting ? _carModel.ForwardAccel * _carModel.PowerValue : _carModel.Vertical * _carModel.ForwardAccel * _carModel.PowerValue;
        Notify("AcelerateAnim");
    }


    public void Streting()
    {
        _carModel.TurtnInput = _carModel.Horizontal;
        _carModel.TurnStrength = _carModel.Drift ? _carModel.statsData.driftStrength : _carModel.statsData.turnStrength;
        if(_carModel.Rigidbody.velocity.normalized.magnitude > .2f && _carModel.Vertical != 0 || _carModel.Bosting)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, _carModel.TurtnInput * _carModel.TurnStrength * Time.deltaTime, 0));
        Notify("SteerAnim");
    }



    float clearCalificationTxt;
    bool driftBoost;

    public void Drift()
    {
        _carModel.PowerValue = _carModel.Drift ? _carModel.statsData.driftPower : _carModel.CurentPowerValue;

        if (_carModel.CalificacionDriftTxt.text != "")
        {
            clearCalificationTxt += Time.deltaTime;
            if (clearCalificationTxt >= .5f)
            {
                _carModel.CalificacionDriftTxt.text = "";
                clearCalificationTxt = 0;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {          
            _carModel.Drift = true;
            _carModel.MultiplerVelue += _carModel.multipler * Time.deltaTime; 
            if(_carModel.DriftValue < 1)
            {
                _carModel.DriftValue += Time.deltaTime * _carModel.MultiplerVelue;
                _carModel.driftImg.material.SetFloat("_driftBar", _carModel.DriftValue);
            }
            else
            {
                Debug.Log("FAIL DRIFT !!");
                _carModel.CalificacionDriftTxt.text = $"fail";
                if(!_carModel.Inmortality)
                    _carModel.photonView.RPC("StunedRPC", RpcTarget.All, true);
                _carModel.Drift = false;
                _carModel.DriftValue = 0;
                _carModel.MultiplerVelue = 1;
                _carModel.TimeDriftBoost = 0;
                _carModel.driftImg.material.SetFloat("_driftBar", _carModel.DriftValue);

            }

            if (_carModel.Drift)
            {
                if (_carModel.Horizontal == -1 && _carModel.Vertical != 0)
                {
                    _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, -25, 0));
                    if (!_carModel._4wheelsVehicle) _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 25));
                }
                if (_carModel.Horizontal == 1 && _carModel.Vertical != 0)
                {
                    _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 25, 0));
                    if (!_carModel._4wheelsVehicle) _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -25));
                }

                Notify("HandBrake");
            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            _carModel.Drift = false;
            _carModel.model.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            if (_carModel.DriftValue < .70f)
            {
                _carModel.CalificacionDriftTxt.text = $"good";
                driftBoost = false;
                _carModel.TimeDriftBoost = 0;
      
            }
            else if(_carModel.DriftValue > .70f && _carModel.DriftValue < .9f)
            {
                _carModel.CalificacionDriftTxt.text = $"perfect";
                driftBoost = true;
                _carModel.Bosting = true;
                _carModel.TimeDriftBoost = .5f;
                _carModel.timeToUlti += 3;
                _carModel.currentTimeBoost = 0;
            }
            else if(_carModel.DriftValue > .9f && _carModel.DriftValue < 1)
            {
                _carModel.CalificacionDriftTxt.text = $"exelent";
                driftBoost = true;
                _carModel.Bosting = true;
                _carModel.TimeDriftBoost = 1f;
                _carModel.timeToUlti += 10;
                _carModel.currentTimeBoost = 0;
            }
            _carModel.DriftValue = 0;
            _carModel.MultiplerVelue = 1;
            _carModel.driftImg.material.SetFloat("_driftBar", _carModel.DriftValue);

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
