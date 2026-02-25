using UnityEngine;

public class Camera : MonoBehaviour
{
    int currFlag = 0;
    public GameObject camera;
    Vector3 startPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currFlag = AudioReactive.currFlag;
        if (currFlag != 4)
        {
            transform.position = startPos;
        }
        if (currFlag == 2)
        {
            transform.Rotate(0, 0, .1f);

        }
        if (currFlag == 5)
        {
            transform.Rotate(0, 0, .1f);

        }
        if (currFlag == 4)
        {
            transform.Rotate(0, 0, .3f);

            float amp = Mathf.InverseLerp(0.3f, 4.5f, AudioSpectrum.audioAmp);
            amp = Mathf.Clamp01(amp);
            Vector3 basePos = new Vector3(0f, 0f, -10f);
            float moveAmount = amp * 2f;
            transform.localPosition = basePos + new Vector3(0f, 0f, moveAmount);
        }
        
    }
}
