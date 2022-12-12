using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem poison;
    [SerializeField] ParticleSystem arrows;
    [SerializeField] ParticleSystem healChurch;
    [SerializeField] ParticleSystem healPiece;
    [SerializeField] ParticleSystem levelUp;
    [SerializeField] ParticleSystem lockParticles;
    [SerializeField] ParticleSystem boost;
    [SerializeField] ParticleSystem splashDmg;
    [SerializeField] ParticleSystem deadParticles;
    [SerializeField] ParticleSystem winGoldParticles;
    [SerializeField] ParticleSystem loseGoldParticles;

    [Header("Turn manager particle")]
    [SerializeField] private ParticleSystem FireWhite;
    [SerializeField] private Light LightWhite;
    [SerializeField] private ParticleSystem FireBlack;
    [SerializeField] private Light LightBlack;


    private void Awake()
    {
        LightWhite.enabled = true;
        FireWhite.Play();
        LightBlack.enabled = false;
        FireBlack.Stop();
    }
    public void PlayPoisonParticles(Vector3 coords)
    {
        ParticleSystem p= Instantiate(poison);
        p.transform.position = coords;
        p.Play();
    }

    public void PlayArrowParticles(Vector3 coords)
    {
        ParticleSystem a= Instantiate(arrows);
        a.transform.position = coords;
        a.Play();
    }
    public void PlayHealChurchParticles(Vector3 coords)
    {
        ParticleSystem h= Instantiate(healChurch);
        h.transform.position = coords;
        h.Play();
    }
    public void PlayHealPieceParticles(Vector3 coords)
    {
        ParticleSystem h= Instantiate(healPiece);
        h.transform.position = coords;
        h.Play();
    }
    public void PlayLevelParticles(Vector3 coords)
    {
        ParticleSystem l = Instantiate(levelUp);
        l.transform.position = coords;
        l.Play();
    }
    public void PlayLockParticles(Vector3 coords)
    {
        ParticleSystem l= Instantiate(lockParticles);
        l.transform.position = coords;
        l.Play();
    }
    public void PlayBoostParticles(Vector3 coords)
    {
        ParticleSystem b= Instantiate(boost);
        b.transform.position = coords;
        b.Play();
    }
    public void PlaySplashParticles(Vector3 coords)
    {
        ParticleSystem s= Instantiate(splashDmg);
        s.transform.position = coords; 
        s.Play();
    }

    public void PlayDeadParticles(Vector3 coords)
    {
        ParticleSystem s = Instantiate(deadParticles);
        s.transform.position = coords;
        s.Play();
        GameObject.Find("AudioManager").GetComponent<AudioManager>().pieceDeath.Play();
    }

    public void PlayWinGoldParticles(Vector3 coords)
    {
        ParticleSystem s = Instantiate(winGoldParticles);
        s.transform.position = coords;
        s.Play();
    }

    public void PlayLoseGoldParticles(Vector3 coords)
    {
        ParticleSystem s = Instantiate(loseGoldParticles);
        s.transform.position = coords;
        s.Play();
    }

    public void ChangeTurn()
    {
        bool isWhiteTurn = LightWhite.isActiveAndEnabled;

        if (isWhiteTurn)
        {
            LightBlack.enabled = true;
            FireBlack.Play();
            LightWhite.enabled = false;
            FireWhite.Stop();
        }
        else
        {
            LightWhite.enabled = true;
            FireWhite.Play();
            LightBlack.enabled = false;
            FireBlack.Stop();
        }
    }

}
