using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {
    AudioSource aSourse;
    public AudioClip[] clips = new AudioClip[2];

    private void Start()
    {
        aSourse = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            System.Random rand = new System.Random();
            if (aSourse)
            {
                aSourse.clip = clips[rand.Next(0, clips.Length)];
                aSourse.Play();
            }
            Destroy(other.gameObject);
        }
    }
}
