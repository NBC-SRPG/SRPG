using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitParticle;

    public void PlayHitParticle()
    {
        hitParticle.Play();
    }
}
