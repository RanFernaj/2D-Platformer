using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundFXCat
    {
        FootStepConcrete, FootStepWood, Jump, Death, HitGround,
        HitCeiling, Flag, Squish, OpenDoor, PickUpKey, PickUpCoin
    }
    public GameObject audioObject;
    public AudioClip[] footSteps;
    public AudioClip[] ladderSteps;
    public AudioClip[] jumpAudio;
    public AudioClip[] deathAudio;
    public AudioClip[] groundedAudio;
    public AudioClip[] ceilingedAudio;
    public AudioClip[] flagRaisedAudio;
    public AudioClip[] sqiushAudio;
    public AudioClip[] openDoorAudio;
    public AudioClip[] keyClips;
    public AudioClip[] coinClips;

    public void AudioTrigger(SoundFXCat audioType, Vector3 audioPosition, float volume)
    {
        GameObject newAudio = GameObject.Instantiate(audioObject, audioPosition, Quaternion.identity);
        ControlAudio ca = newAudio.GetComponent<ControlAudio>();
        switch (audioType)
        {
            case (SoundFXCat.Death):
                ca.myClip = deathAudio[Random.Range(0, deathAudio.Length)];
                break;
            case (SoundFXCat.Flag):
                ca.myClip = flagRaisedAudio[Random.Range(0, flagRaisedAudio.Length)];
                break;
            case (SoundFXCat.FootStepConcrete):
                ca.myClip = footSteps[Random.Range(0, footSteps.Length)];
                break;
            case (SoundFXCat.FootStepWood):
                ca.myClip = ladderSteps[Random.Range(0, ladderSteps.Length)];
                break;
            case (SoundFXCat.HitCeiling):
                ca.myClip = ceilingedAudio[Random.Range(0, ceilingedAudio.Length)];
                break;
            case (SoundFXCat.HitGround):
                ca.myClip = groundedAudio[Random.Range(0, groundedAudio.Length)];
                break;
            case (SoundFXCat.Jump):
                ca.myClip = jumpAudio[Random.Range(0, jumpAudio.Length)];
                break;
            case (SoundFXCat.OpenDoor):
                ca.myClip = openDoorAudio[Random.Range(0, openDoorAudio.Length)];
                break;
            case (SoundFXCat.PickUpCoin):
                ca.myClip = coinClips[Random.Range(0, coinClips.Length)];
                break;
            case (SoundFXCat.PickUpKey):
                ca.myClip = keyClips[Random.Range(0, keyClips.Length)];
                break;
            case (SoundFXCat.Squish):
                ca.myClip = sqiushAudio[Random.Range(0, sqiushAudio.Length)];
                break;
        }

        ca.volume = volume;
        ca.StartAudio();
    }
}
