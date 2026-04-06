using System.Linq;
using UnityEngine;

public class UIEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;

    public bool IsActive()
    {
        return _particles.Any(o => o.isPlaying);
    }
    
    public void Play()
    {
        foreach (var particle in _particles)
        {
            particle.Stop();
            particle.Clear();
            particle.Play();
        }
    }

    public void Stop()
    {
        foreach (var particle in _particles)
        {
            particle.Stop();
            particle.Clear();
        }
    }
}
