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

    public void PlayPoisonParticles(Vector2Int coords)
    {
        poison.transform.position = new Vector3(coords.x, coords.y, 0);
        poison.Play();
    }

    public void PlayArrowParticles(Vector3 coords)
    {
        Instantiate(arrows);
        arrows.transform.position = coords;
        arrows.Play();
    }
    public void PlayHealChurchParticles(Vector2Int coords)
    {
        healChurch.transform.position = new Vector3(coords.x, coords.y, 0);
        healChurch.Play();
    }
    public void PlayHealPieceParticles(Vector2Int coords)
    {
        healPiece.transform.position = new Vector3(coords.x, coords.y, 0);
        healPiece.Play();
    }
    public void PlayLevelParticles(Vector2Int coords)
    {
        levelUp.transform.position = new Vector3(coords.x, coords.y, 0);
        levelUp.Play();
    }
    public void PlayLockParticles(Vector2Int coords)
    {
        lockParticles.transform.position = new Vector3(coords.x, coords.y, 0);
        lockParticles.Play();
    }
    public void PlayBoostParticles(Vector2Int coords)
    {
        boost.transform.position = new Vector3(coords.x, coords.y, 0);
        boost.Play();
    }
    public void PlaySplashParticles(Vector2Int coords)
    {
        splashDmg.transform.position = new Vector3(coords.x, coords.y, 0); 
        splashDmg.Play();
    }

}
