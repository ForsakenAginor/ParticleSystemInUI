using System.Linq;
using UnityEngine;

public class UIEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;

    /// <summary>
    /// Checks whether any of the particle systems in this effect are currently playing.
    /// </summary>
    /// <returns>
    /// <c>true</c> if at least one particle system is playing; otherwise, <c>false</c>.
    /// </returns>
    public bool IsActive()
    {
        return _particles.Any(o => o.isPlaying);
    }
    
    /// <summary>
    /// Plays the effect by restarting all particle systems.
    /// Stops any ongoing emission, clears all existing particles, and starts playback from the beginning.
    /// </summary>
    public void Play()
    {
        foreach (var particle in _particles)
        {
            particle.Stop();
            particle.Clear();
            particle.Play();
        }
    }

    /// <summary>
    /// Stops the effect immediately and clears all active particles.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="Play"/>, this method does not restart playback.
    /// Use this to abruptly end the effect without fading out.
    /// </remarks>
    public void Stop()
    {
        foreach (var particle in _particles)
        {
            particle.Stop();
            particle.Clear();
        }
    }
}
