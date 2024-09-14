using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public interface IEmotionScene
{
    Transform[] Get3DPositions();

    List<Particle> GetParticles();

    List<GameObject> Prefabs { get; }
}