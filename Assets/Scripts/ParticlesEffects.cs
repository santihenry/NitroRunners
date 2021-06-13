using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ParticlesEffects : MonoBehaviour
{   
    CarModel _model;
    public List<ParticleSystem> sparks = new List<ParticleSystem>();
    public List<ParticleSystem> smoke = new List<ParticleSystem>();
    public List<ParticleSystem> turbo = new List<ParticleSystem>();
    public List<ParticleSystem> oil = new List<ParticleSystem>();

    void Start()
    {
        _model = GetComponent<CarModel>();

        StopParticles(sparks);
        StopParticles(smoke);
        StopParticles(turbo);
        StopParticles(oil);

    }

    
    void Update()
    {
        if (_model.Handbracke)
        {
            if (_model.Rigidbody.velocity.magnitude > 10)
            {
                PlayParticles(sparks);
                PlayParticles(smoke);
                ChangeSmoke(true);
            }
            else
            {
                StopParticles(sparks);
                StopParticles(smoke);
            }
        }
        else
        {
            StopParticles(sparks);
            StopParticles(smoke);
         
        }
        

        if (_model.Bosting == true)
        {
            PlayParticles(turbo);
            

        }
        else
        {
            StopParticles(turbo);
        }
    }




    public void PlayParticles(List<ParticleSystem> particles)
    {
        foreach (var particle in particles)
        {
            if (!particle.isPlaying)
                particle.Play();
        }
    }

  
    public void StopParticles(List<ParticleSystem> particles)
    {
        foreach (var particle in particles)
        {
            if (particle.isPlaying)
                particle.Stop();
        }
    }

   
    public void ChangeSmoke(bool fix)
    {
        foreach (var dust in smoke)
        {
            var newColor = dust.GetComponent<ParticleSystemRenderer>().material.color;
            dust.GetComponent<ParticleSystemRenderer>().material.color = 
                new Color(newColor.r,newColor.g,newColor.b,fix? (_model.Rigidbody.velocity.magnitude / _model.MaxSpeed) *3.2f: (_model.Rigidbody.velocity.magnitude / _model.MaxSpeed));
        }
    } 
}
