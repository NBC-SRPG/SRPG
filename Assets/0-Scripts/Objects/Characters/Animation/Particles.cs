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

    public void HitParticle()
    {
        hitParticle.Play();
    }

    public void PlayParticle(string particleName)
    {
        particleMap[particleName].Play();
    }

    public void StopParticle(string particleName)
    {
        particleMap[particleName].Stop();
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

            ParticleSystem particle;
            child.TryGetComponent<ParticleSystem>(out particle);
            if (particle != null)
            {
                particleMap.Add(particle.name, particle);
            }

            if (child.name.Equals("CameraTransform"))
            {
                foreach(Transform transform in child.GetComponentsInChildren<Transform>())
                {
                    cameraTransform.Add(transform.name, transform);
                }
            }
        }
    }

    public void ChangeParent(string name, Transform parent)
    {
        particleMap[name].transform.SetParent(parent);
        particleMap[name].transform.localPosition = Vector3.zero;
        particleMap[name].transform.localScale = Vector3.one;
    }
}
