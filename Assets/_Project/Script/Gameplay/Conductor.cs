using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defend
{
    public class Conductor : MonoBehaviour
    {
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    //The offset to the first beat of the song in seconds
public float firstBeatOffset;

//the number of beats in each loop
public float beatsPerLoop;

//the total number of loops completed since the looping clip first started
public int completedLoops = 0;

//The current position of the song within the loop in beats.
public float loopPositionInBeats;

    private float currentTime;

        void Start()
        {
            //Load the AudioSource attached to the Conductor GameObject
            musicSource = GetComponent<AudioSource>();

            //Calculate the number of seconds in each beat
            secPerBeat = 60f / songBpm;

            //Record the time when the music starts
            dspSongTime = (float)AudioSettings.dspTime;

            //Start the music
            musicSource.Play();
        } 

        private void Update()
        {
            songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
            songPositionInBeats = songPosition / secPerBeat;
            
            //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop; 

            currentTime += Time.deltaTime;
            if (currentTime >= secPerBeat)
            {
                Debug.Log("letsgo!");
                currentTime = 0f;
            }
        }
    }
}
