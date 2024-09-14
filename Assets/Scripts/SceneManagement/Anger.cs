using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Anger : IEmotionScene
{
    public List<GameObject> prefabObjects;
    public Transform t1;
    public Transform t2;

    public Transform[] Get3DPositions()
    {
        return new Transform[]
        {
            t1,t2
        };
    }

    public List<Particle> GetParticles()
    {
        return new List<Particle>
        {
            new Particle(), 
            new Particle(),
            new Particle()
        };
    }

    public List<GameObject> Prefabs
    {
        get { return prefabObjects; }
    }
}
