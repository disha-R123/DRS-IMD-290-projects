// UMD IMDM290 
// Instructor: Myungin Lee
//Noah Petroski and Disha Ramesh
// All the same Lerp but using audio

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class AudioReactive : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 200; 
    float time = 0f;
    Vector3[] initPos;
    Vector3[,] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;
    // Color[] bright = [(255, 188, 66), ()];
    public static int currFlag = 0;
    float[] timeFlags;
    int segmentCount;
    public Texture2D ppImg;
    public Texture2D starImg;

    public Texture2D dolphinImg;
    float actualTime;

    // Start is called before the first frame update
    void Start()
    {
        actualTime = 0;
        // Assign proper types and sizes to the variables.
        //timeFlags = new float[] {0f, 10f, 20f, 30f};
        segmentCount = 8;
        List<Vector3> ppShape = outlineShape(ppImg);
        List<Vector3> starShape = outlineShape(starImg);
     
        spheres = new GameObject[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        startPosition = new Vector3[segmentCount, numSphere]; 
        endPosition = new Vector3[segmentCount, numSphere];

        // Define target positions. Start = random, End = heart 
        for (int j = 0; j < segmentCount; j++) {
            // Random start positions
            float r = 10f;
            Vector3 randomPoint = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));

            for (int i =0; i < numSphere; i++)
            {

                startPosition[j, i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));
                
                // Circular end position
                r = 10f;

                if (j == 0)
                {
                    endPosition[j, i] = ppShape[i%ppShape.Count];
                }
                 else if (j == 1 || j == 7) {
                    endPosition[j, i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numSphere), r * Mathf.Cos(i * 2 * Mathf.PI / numSphere));      
                } else if (j == 2)
                {
                    int v = 3;
                    int c = 3;
                    int w = 3;
                    endPosition[j, i] = new Vector3((v*i + c) * Mathf.Cos(w*i), (v*i + c) * Mathf.Sin(w*i), 50f);
                }
                else if (j == 3 || j == 5)
                {
                    endPosition[j, i] = starShape[i%starShape.Count];      
                }
                else if (j == 4)
                {
                    List<Vector3> dolphinShape = outlineShape(dolphinImg);

                    endPosition[j, i] = dolphinShape[i%dolphinShape.Count];
                }
                else
                {   
                    endPosition[j, i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numSphere), r * Mathf.Cos(i * 2 * Mathf.PI / numSphere));
                }
            }
        }
        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Position
            initPos[i] = startPosition[currFlag, i];
            spheres[i].transform.position = initPos[i];
            spheres[i].transform.localRotation = Quaternion.EulerAngles(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
            spheres[i].transform.localScale = new Vector3(Random.Range(0.3f, 0.5f), Random.Range(0.3f, 0.5f), Random.Range(0.3f, 0.5f));
            // Color
            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }
    List<Vector3> outlineShape(Texture2D shape)
    {
        List<Vector3> shapeCoordinates = new List<Vector3>(); 
        for (int i = 0; i < shape.height; i++)
        {
            for (int j = 0; j < shape.width; j++)
            {
                Color pixel = shape.GetPixel(i, j);
                Color pixelTop = shape.GetPixel(i-1, j);
                Color pixelLeft = shape.GetPixel(i, j-1);
                Color pixelRight = shape.GetPixel(i, j+1);
                Color pixelBottom = shape.GetPixel(i+1, j);

                if (pixel.a > 0.5f && (pixelTop.a < 0.5f || pixelBottom.a < 0.5f || pixelLeft.a < 0.5f || pixelRight.a < 0.5f))
                {
                    shapeCoordinates.Add(new Vector3(i - 20, j - 25, 23f));
                } 
            }
        }
        return shapeCoordinates;
    }


private static readonly Color[] palette = new Color[]
{
    new Color32(0xFF, 0xBC, 0x42, 255), // #FFBC42
    new Color32(0xF5, 0x8F, 0x5B, 255),
    new Color32(0xEC, 0x62, 0x78, 255),

    new Color32(0xD8, 0x11, 0x59, 255), // #D81159
    new Color32(0xB6, 0x24, 0x57, 255),

    new Color32(0x8F, 0x2D, 0x56, 255), // #8F2D56
    new Color32(0x6B, 0x44, 0x63, 255),
    new Color32(0x45, 0x5F, 0x73, 255),
    
    new Color32(0x21, 0x83, 0x80, 255), // #218380
    new Color32(0x3C, 0xA6, 0xB0, 255),
    new Color32(0x5C, 0xC0, 0xC9, 255),

    new Color32(0x73, 0xD2, 0xDE, 255)  // #73D2DE
};

private static Color[] palette2 = new Color[]
{
    new Color32(0x69, 0x30, 0xC3, 255), // #6930C3
    new Color32(0x74, 0x00, 0xB8, 255), // #7400B8
    new Color32(0x48, 0xBF, 0xE3, 255), // #48BFE3
    new Color32(0x80, 0xFF, 0xDB, 255)  // #80FFDB
};

private static Color[] palette3 = new Color[]
{
    new Color32(0xFF, 0x49, 0x9E, 255), // #FF499E
    new Color32(0xD2, 0x64, 0xB6, 255), // #D264B6
    new Color32(0xA4, 0x80, 0xCF, 255), // #A480CF
    new Color32(0x77, 0x9B, 0xE7, 255), // #779BE7
    new Color32(0x49, 0xB6, 0xFF, 255)  // #49B6FF
};

private static Color[] palette4 = new Color[]
{
    new Color32(255, 105, 235, 255), // #FF69EB
    new Color32(255, 134, 200, 255), // #FF86C8
    new Color32(255, 163, 165, 255), // #FFA3A5
    new Color32(255, 191, 129, 255), // #FFBF81
    new Color32(255, 220,  94, 255), // #FFDC5E
};

private static Color[] palette5 = new Color[]
{
    new Color32(135, 135, 135, 255), // #878787
    new Color32(160, 135, 148, 255), // #A08794
    new Color32(187, 126, 140, 255), // #BB7E8C
    new Color32(201, 182, 190, 255), // #C9B6BE
    new Color32(209, 190, 207, 255), // #D1BECF
};
// Update is called once per frame
    void Update()
    {
        /*
        flag 0 = PP
        flag 1 = circle
        flag 2 = weird 
        flag 3 = star
        flag 4 = zara verses
        flag 5 = rotating star
        flag 6 = zara main verse
        flag 7 = ending
        */
        
        if (actualTime < 8.65){
            currFlag = 0;
            Debug.Log("segment 1");
        } else if ( actualTime < 39.5){
            Debug.Log("Segment 2");
            currFlag = 1;
        } else if (actualTime < 54.5){
            Debug.Log("Segment 3");
            currFlag = 2;
        } else if(actualTime < 70.5){
            Debug.Log("Segment 4");
            currFlag = 3;
        } else if(actualTime < 80){
             Debug.Log("Segment 5");
             currFlag = 5;
        } else if(actualTime < 100){
             Debug.Log("Segment 6");
             currFlag = 5;
        } else if(actualTime < 117){ //zara 1st verse
             Debug.Log("Segment 7");
             currFlag = 4;
        } else if(actualTime < 135){ //zara main verse
             Debug.Log("Segment 8");
             currFlag = 4;
        } else if(actualTime < 157){
             Debug.Log("Segment 9");
             currFlag = 2;
        } else if(actualTime < 174){
             Debug.Log("Segment 10");
             currFlag = 5;
        } else {
            //ending
             Debug.Log("Segment 12");
             currFlag = 7;
        }

        float amp = AudioSpectrum.audioAmp;
        time += Time.deltaTime * (1f + amp) * 0.3f;
        actualTime += Time.deltaTime;

        float minAmp = 0.3f;
        float maxAmp = 4.5f;

        float ampClamped = Mathf.Clamp(amp, minAmp, maxAmp);

        float tAmpLog =
            (Mathf.Log(ampClamped) - Mathf.Log(minAmp)) /
            (Mathf.Log(maxAmp) - Mathf.Log(minAmp));

        tAmpLog = Mathf.Clamp01(tAmpLog);

        for (int i = 0; i < numSphere; i++)
        {
            if (currFlag == 1) {
                        lerpFraction = Mathf.Sin(time) * 0.75f;
                    }
                    else if (currFlag == 2) {
                        lerpFraction = Mathf.Sin(time) * 0.9f + 0.5f;
                    }
                    else {
                        lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f; 
                    }
            lerpFraction = Mathf.Clamp01(lerpFraction);

            t = i * 2f * Mathf.PI / numSphere;
            spheres[i].transform.position =
                Vector3.Lerp(startPosition[currFlag, i], endPosition[currFlag, i], lerpFraction);

            if (currFlag != 1)
            {
                float scale = Mathf.Clamp(1f + amp, 0.1f, 10f); // pick a sane upper bound
                spheres[i].transform.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                float scale = Mathf.Clamp(0.1f + AudioSpectrum.audioAmp, 0.05f, 5f);
                spheres[i].transform.localScale = new Vector3(scale, scale, 0.2f);
            }

            spheres[i].transform.Rotate(amp, 1f, 0f);

            // Color 
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            Color color;

            /*if (currFlag == 2)
            {
                float h = Mathf.Repeat(0.15f * Mathf.Sin(time) + (float)i / numSphere, 1f);
                color = Color.HSVToRGB(h, 1f, 1f);
            }*/
            if (currFlag == 0) {
                float t = Mathf.InverseLerp(0.3f, 4.5f, AudioSpectrum.audioAmp);
                t = Mathf.Clamp01(t);
                float scaled = t * (palette.Length - 1);

                int idx = Mathf.FloorToInt(scaled);
                idx = Mathf.Clamp(idx, 0, palette.Length - 1);

                int nextIdx = Mathf.Min(idx + 1, palette.Length - 1);

                float blend = scaled - idx;

                color = Color.Lerp(palette[idx], palette[nextIdx], blend);

            }
            else if (currFlag == 1) {
                float scaled = tAmpLog * (palette3.Length - 1);

                int idx = Mathf.FloorToInt(scaled);
                idx = Mathf.Clamp(idx, 0, palette2.Length - 1);

                int nextIdx = Mathf.Min(idx + 1, palette2.Length - 1);
                float blend = scaled - idx;

                color = Color.Lerp(palette2[idx], palette2[nextIdx], blend);

            } else if (currFlag == 3 || currFlag == 5) {
                float t = Mathf.InverseLerp(0.3f, 4.5f, AudioSpectrum.audioAmp);
                t = Mathf.Clamp01(t);
                float scaled = t * (palette3.Length - 1);

                int idx = Mathf.FloorToInt(scaled);
                idx = Mathf.Clamp(idx, 0, palette3.Length - 1);

                int nextIdx = Mathf.Min(idx + 1, palette3.Length - 1);

                float blend = scaled - idx;

                color = Color.Lerp(palette3[idx], palette3[nextIdx], blend);
            } else if (currFlag == 2){
                float t = Mathf.InverseLerp(0.3f, 4.5f, AudioSpectrum.audioAmp);
                t = Mathf.Clamp01(t);
                float scaled = t * (palette.Length - 1);

                int idx = Mathf.FloorToInt(scaled);
                idx = Mathf.Clamp(idx, 0, palette.Length - 1);

                int nextIdx = Mathf.Min(idx + 1, palette.Length - 1);

                float blend = scaled - idx;

                color = Color.Lerp(palette[idx], palette[nextIdx], blend);
            }else if (currFlag == 7){
                float t = Mathf.InverseLerp(0.3f, 4.5f, AudioSpectrum.audioAmp);
                t = Mathf.Clamp01(t);
                float scaled = t * (palette5.Length - 1);

                int idx = Mathf.FloorToInt(scaled);
                idx = Mathf.Clamp(idx, 0, palette5.Length - 1);

                int nextIdx = Mathf.Min(idx + 1, palette5.Length - 1);

                float blend = scaled - idx;

                color = Color.Lerp(palette5[idx], palette5[nextIdx], blend);
                
            } else {

                float t = Mathf.InverseLerp(0.3f, 4.5f, AudioSpectrum.audioAmp);
                t = Mathf.Clamp01(t);
                float scaled = t * (palette4.Length - 1);

                int idx = Mathf.FloorToInt(scaled);
                idx = Mathf.Clamp(idx, 0, palette4.Length - 1);

                int nextIdx = Mathf.Min(idx + 1, palette4.Length - 1);

                float blend = scaled - idx;

                color = Color.Lerp(palette4[idx], palette4[nextIdx], blend);
            }
            sphereRenderer.material.color = color;
        }
         Debug.Log(actualTime);
    }
}
