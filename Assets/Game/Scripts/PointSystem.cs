using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointSystem : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource SoundSource;
    public AudioClip hitmarker;
    public AudioClip killSound;

    [Header("Animation")]
    public Animation hitmarkerAnimation;
    public Animation killAnimation;
    public GameObject hitmarkerAnimationGameObject;
    public GameObject killAnimationGameObject;

    public void Damage(float damage)
    {

        hitmarkerAnimationGameObject.SetActive(true);
        SoundSource.PlayOneShot(hitmarker, 0.5f);
        if (!hitmarkerAnimation.isPlaying)
        {
            hitmarkerAnimation.Play();
        }
        else
        {
            hitmarkerAnimation[hitmarkerAnimation.clip.name].normalizedTime = 0;
        }
        
        
    }
    public void Killed()
    {
        killAnimationGameObject.SetActive(true);
        SoundSource.PlayOneShot(killSound, 2.5f);
        killAnimation.Play();
    }
}
