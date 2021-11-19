using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public static Spinning Instance;

    public static event System.Action<bool> SpinningEvent;
    public Transform spinningTrans;
    public bool isSpinning;
    public float spinSpeed;

    public AudioSource startSource, loopSource, endSource;
    public float delayStartFadeOut, startFadeOutSpeed;
    public float loopFadeInSpeed, loopFadeOutSpeed;
    public float endFadeInSpeed;

    public float maxVolume;


    Vector3 startRot;

    public GameObject dial;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SpinningEventCall(false);
        startRot = dial.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            spinningTrans.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
        }
    }

    public void SpinningEventCall(bool isSpinningBool)
    {
        isSpinning = isSpinningBool;
        SpinningEvent?.Invoke(isSpinning);
    }

    public void CycleSpin()
    {
        //Debug.Log("Cycling spin.");
        SpinningEventCall(!isSpinning);
        if (isSpinning)
        {
            dial.transform.rotation = Quaternion.Euler(startRot + new Vector3(0f, 0f, 50f));
            StartCoroutine(StartSoundFadeAfterFloat());
            StartCoroutine(FadeInLoop());
        }
        else
        {
            dial.transform.rotation = Quaternion.Euler(startRot);
            StartCoroutine(FadeOutLoop());
            StartCoroutine(FadeInEndSound());
        }
    }


    IEnumerator StartSoundFadeAfterFloat()
    {
        startSource.volume = maxVolume;
        startSource.Play();
        yield return new WaitForSeconds(delayStartFadeOut);
        for (float i = maxVolume; i > 0; i -= Time.deltaTime * startFadeOutSpeed)
        {
            startSource.volume = Mathf.Max(0f, i);
            yield return null;
        }
        startSource.Stop();
    }

    IEnumerator FadeInLoop()
    {
        loopSource.volume = 0f;
        loopSource.Play();
        for (float i = 0; i < maxVolume; i += Time.deltaTime * loopFadeInSpeed)
        {
            loopSource.volume = Mathf.Min(maxVolume, i);
            yield return null;
        }

    }
    IEnumerator FadeOutLoop()
    {
        loopSource.volume = maxVolume;
        for (float i = maxVolume; i > 0; i -= Time.deltaTime * loopFadeOutSpeed)
        {
            loopSource.volume = Mathf.Max(0, i);
            yield return null;
        }
        loopSource.Stop();
    }

    IEnumerator FadeInEndSound()
    {
        endSource.volume = 0;
        endSource.Play();
        for (float i = 0f; i < maxVolume; i += Time.deltaTime * endFadeInSpeed)
        {
            endSource.volume = Mathf.Min(maxVolume, i);
            yield return null;
        }
    }
}
