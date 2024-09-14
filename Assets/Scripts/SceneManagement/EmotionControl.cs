using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EmotionControl : MonoBehaviour, IEmotionScene
{
    public List<GameObject> prefabObjects;
    public Transform[] preplannedPositions;
    public List<Particle> particles;

    public Transform[] Get3DPositions()
    {
        return preplannedPositions;
    }

    public List<Particle> GetParticles()
    {
        return particles;
    }

    public List<GameObject> Prefabs
    {
        get { return prefabObjects; }
    }
}
