using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Particles : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitParticle;

    public Dictionary<string, ParticleSystem> particleMap;
    public Dictionary<string, Transform> cameraTransform;

    public void Init()
    {
        hitParticle = Instantiate(Managers.Resource.Load<ParticleSystem>("Particle/FX_hit1"), transform);
        particleMap = new Dictionary<string, ParticleSystem>();
        cameraTransform = new Dictionary<string, Transform>();
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

    public void LoadParticles(GameObject gameObject)
    {
        GameObject particles = Instantiate(gameObject, transform);

        for(int i = 0; i < particles.transform.childCount; i++)
        {
            Transform child = particles.transform.GetChild(i);
            Debug.Log(child.name);

            ParticleSystem particle;
            child.TryGetComponent<ParticleSystem>(out particle);
            if (particle != null)
            {
                particleMap.Add(particle.name, particle);
            }

            if (child.name.Equals("CameraTransform"))
            {
                Debug.Log("21");
                foreach(Transform transform in child.GetComponentsInChildren<Transform>())
                {
                    Debug.Log(transform.name);
                    cameraTransform.Add(transform.name, transform);
                }
            }
        }
    }
}
