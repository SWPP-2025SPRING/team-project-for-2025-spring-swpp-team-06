using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMeshManager : MonoBehaviour
{
    public ParticleSystem ps;
    public float speed = 2f;
    public TutorialUIController.TutorialStep nextStep;

    private ParticleSystem.Particle[] particles;

    void LateUpdate()
    {
        if (particles == null || particles.Length < ps.main.maxParticles) 
            particles = new ParticleSystem.Particle[ps.main.maxParticles];

        int numParticlesAlive = ps.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++) {
            Vector3 toCenter = (Vector3.zero - particles[i].position).normalized;
            particles[i].velocity = toCenter * speed;
        }

        ps.SetParticles(particles, numParticlesAlive);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerControl>().StopMove();
            FindObjectOfType<TutorialUIController>().StartStep(nextStep);
            gameObject.SetActive(false);
        }
    }
}
