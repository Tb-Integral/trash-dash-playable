using UnityEngine;

/// <summary>
/// Playworks Mecanim will play Start and runLoop (exit-time) but will not switch
/// to Jump/Sliding. Sample those clips in LateUpdate so Animator can stay on
/// runLoop underneath; turning Animator off is what killed run after a swipe.
/// </summary>
[DefaultExecutionOrder(10000)]
public class LunaCatLegacyAnim : MonoBehaviour
{
    public Animation clipPlayer;
    public Animator mecanim;
    public AnimationClip runClip;
    public AnimationClip jumpClip;
    public AnimationClip slideClip;

    bool _driving;
    AnimationClip _clip;
    string _clipName;
    float _time;
    float _length;
    WrapMode _wrap;

    void Awake()
    {
        if (clipPlayer != null)
        {
            clipPlayer.playAutomatically = false;
            clipPlayer.enabled = false;
        }
    }

    public void PlayRun()
    {
        StopLegacy();
        if (mecanim != null)
            mecanim.enabled = true;
    }

    public void PlayJump()
    {
        PlayClip(jumpClip, WrapMode.Once);
    }

    public void PlaySlide()
    {
        PlayClip(slideClip, WrapMode.Once);
    }

    public void HandOffToDeath()
    {
        StopLegacy();
        if (mecanim != null)
            mecanim.enabled = true;
    }

    public void PlayClip(AnimationClip clip, WrapMode wrap)
    {
        if (clip == null)
            return;

        string clipName = "Cat_Jump";
        float length = 0.6f;
        if (clip == slideClip)
        {
            clipName = "Cat_Slide";
            length = 1.0333334f;
        }
        else if (clip == runClip)
        {
            clipName = "Cat_RunShort";
            length = 0.33333334f;
        }

        _clip = clip;
        _clipName = clipName;
        _wrap = wrap;
        _length = length;
        _time = 0f;
        _driving = true;

        if (clipPlayer != null)
        {
            clipPlayer.enabled = true;
            clipPlayer.clip = clip;
            clipPlayer.wrapMode = wrap;
            clipPlayer.Play(clipName);
            clipPlayer.Play();
        }

        ApplyPose(0f);
    }

    void StopLegacy()
    {
        _driving = false;
        _clip = null;
        if (clipPlayer == null)
            return;

        clipPlayer.Stop();
        clipPlayer.enabled = false;
    }

    void LateUpdate()
    {
        if (!_driving || _clip == null)
            return;

        _time += Time.deltaTime;
        float t = _time;
        if (_wrap == WrapMode.Loop)
        {
            if (_length > 0.01f)
                t = Mathf.Repeat(_time, _length);
        }
        else if (t > _length)
        {
            t = _length;
        }

        ApplyPose(t);
    }

    void ApplyPose(float t)
    {
        _clip.SampleAnimation(gameObject, t);

        if (clipPlayer == null || !clipPlayer.enabled)
            return;

        AnimationState state = clipPlayer[_clipName];
        if (state != null)
            state.time = t;
        clipPlayer.Sample();
    }
}
