using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim_104 : CharAnimBase
{
    protected override void LoadParticles()
    {
        base.LoadParticles();

        particles.LoadParticles(Managers.Resource.Load<GameObject>("Particle/PriestFX"));
    }
}
