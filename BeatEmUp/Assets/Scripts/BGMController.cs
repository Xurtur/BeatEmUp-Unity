using System.Collections;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    private AudioSource AudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
   
	}

	// Update is called once per frame
	void Update()
    {
        if (AudioSource.time < 2 && AudioSource.clip != null)
        {
            StartCoroutine(Fade());
        }
        else
        {
            StopCoroutine(Fade());
        }


    }

    IEnumerator Fade()
    {
       AudioSource.volume = Mathf.Lerp(AudioSource.volume, .3f, Time.deltaTime * 2f);
       yield return new WaitForSeconds(.1f);
    }
}
