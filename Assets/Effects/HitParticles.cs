using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticles : MonoBehaviour {

    public Material cyanMat;
    public Material magentaMat;
	
	public void InitSystem(PlayerTeam team, int amount = 200)
    {
        GetComponent<Renderer>().material = team == PlayerTeam.CYAN ? cyanMat : magentaMat;

        ParticleSystem system = GetComponent<ParticleSystem>();

        ParticleSystem.Burst[] burst = { new ParticleSystem.Burst(0, (short)amount) };
        system.emission.SetBursts(burst);
        ParticleSystem.MainModule mainMod = system.main;
        mainMod.startSpeed = new ParticleSystem.MinMaxCurve(amount / 20, amount / 10);
        Invoke("Kill", 5);
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
