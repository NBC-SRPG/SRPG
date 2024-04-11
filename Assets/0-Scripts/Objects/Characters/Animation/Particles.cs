using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitParticle;

    public Dictionary<string, ParticleSystem> particleMap;

    public void Init()
    {
        particleMap = new Dictionary<string, ParticleSystem>();
    }

    public void PlayHitParticle()
    {
        hitParticle.Play();
    }

    public void PlayParticle(string particleName)
    {
        Debug.Log("Play " +  particleName);
        particleMap[particleName].Play();
    }

    public void AddParticles(ParticleSystem particle)
    {
        particleMap.Add(particle.name, particle);
        particleMap[particle.name] = Instantiate(particle, transform);
    }
}
