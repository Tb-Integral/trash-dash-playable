using UnityEngine;

public class CountdownSound : MonoBehaviour
{
	protected AudioSource m_Source;
	protected float m_TimeToDisable;
	bool m_Started;

    protected const float k_StartDelay = 0.5f;
	
    void OnEnable()
	{
		m_Source = GetComponent<AudioSource>();
		m_TimeToDisable = m_Source.clip.length;
		m_Started = false;
        TryPlay();
	}

	void Update()
	{
		TryPlay();

		m_TimeToDisable -= Time.deltaTime;

		if (m_TimeToDisable < 0)
			gameObject.SetActive(false);
	}

	void TryPlay()
	{
		if (m_Started || m_Source == null)
			return;

#if UNITY_LUNA
		if (MusicPlayer.instance != null && !MusicPlayer.instance.IsAudioUnlocked)
			return;
		m_Source.Play();
#else
        m_Source.PlayDelayed(k_StartDelay);
#endif
		m_Started = true;
	}
}
