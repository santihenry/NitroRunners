using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarView : MonoBehaviour, IObserver
{
    public Animator carAnim,riderAnim;
    CarModel _carModel;
    
    private void Start()
    {
        _carModel = GetComponent<CarModel>();
        var obs = GetComponent<IObservable>();
        if (obs != null) obs.SubEvent(this);
    }

    public void JumpAnim()     //<-----  Se llama en funcion Jump() del car model line 274
    {
        riderAnim.SetTrigger("Jump");
    }

    public void AcelerateAnim(float vel)    //<-----  Se llama en funcion EnginePower() del car model line 427
    {   if(carAnim!=null)
        carAnim.SetFloat("MoveVertical", vel);
        if (riderAnim != null)
            riderAnim.SetFloat("MoveVertical", vel);
    }


    public void SteerAnim(float vel)      //<-----  Se llama en funcion SteerVehicle() del car model line 405
    {
        if (carAnim != null)
            carAnim.SetFloat("MoveHorizontal", vel);
        if (riderAnim != null)
            riderAnim.SetFloat("MoveHorizontal", vel);
    }


    public void BrakeAnim(bool v)      //<-----  Se llama en funcion Brake() del car model line 547
    {
        if (carAnim != null)
            carAnim.SetBool("Brake", v);
        if (riderAnim != null)
            riderAnim.SetBool("Brake", v);

    }

    public void HandBrake(bool v)       //<-----  Se llama en funcion Traction() del car model line 305
    {
        if (carAnim != null)
            carAnim.SetBool("Handbrake", v);
        if (riderAnim != null)
            riderAnim.SetBool("Handbrake", v);
        print("frenando");
    }
    public void Hit()
    {
        if (carAnim != null)
            //carAnim.SetTrigger("Hit");
            carAnim.Play("Hit");
        if (riderAnim != null)
            //riderAnim.SetTrigger("Hit");
            riderAnim.Play("Hit");
    }
    public void Stunned(bool b)
    {
        if (carAnim != null)
            carAnim.SetBool("Stunned",b);
        if (riderAnim != null)
            riderAnim.SetBool("Stunned", b);
    }
    public void Electrocuted(bool b)
    {
        if (carAnim != null)
            carAnim.SetBool("Electrocuted", b);
        if (riderAnim != null)
            riderAnim.SetBool("Electrocuted", b);
    }


    public void BoostAnim(bool v)     //<-----  Se llama en funcion ActivateBoost() del car model line 376
    {
       /* if (carAnim != null)
            carAnim.SetBool("Boost", v);
        if (rayerAnim != null)
            rayerAnim.SetBool("Boost", v);*/
    }

    public void Special()
    {
        if (carAnim != null)
            carAnim.SetTrigger("Special");
        if (riderAnim != null)
            riderAnim.SetTrigger("Special");
    }
    public void OnNotify(string eventName)
    {
        //if (carAnim == null || rayerAnim == null) return;

        if(eventName == "SteerAnim")
        {
            SteerAnim(_carModel.Horizontal);
            
        }   
                 
        if (eventName == "AcelerateAnim")
        {
            AcelerateAnim(_carModel.Vertical);
        }

        if (eventName == "HandBrake")
        {
          BoostAnim(true);
        }
        else
        {
            BoostAnim(false);
        }


        if (eventName == "BoostAnim")
        {
            BoostAnim(_carModel.Bosting);
        }
        if (eventName == "ElectrocutedOn")
        {
            Electrocuted(true);
        }
        if (eventName == "ElectrocutedOff")
        {
            Electrocuted(false);
        }
        if (eventName == "Hit")
        {
            Hit();
        }
        if (eventName == "StunnedOn")
        {
            Stunned(true);
        }
        if (eventName == "StunnedOff")
        {
            Stunned(false);
        }
        if (eventName == "Special")
        {
            Special();
        }

    }

}
