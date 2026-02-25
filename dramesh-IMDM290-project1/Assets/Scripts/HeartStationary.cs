using UnityEngine;

public class HeartStationary : MonoBehaviour
{
   GameObject[] spheres;
    Vector3[] spheresInitialPositions;
    static int numSphere = 200; 
    float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        spheres = new GameObject[numSphere]; // how many spheres
        spheresInitialPositions = new Vector3[numSphere]; // initial positions of the spheres
        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            float r = 10f; // radius of the circle
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
            // Initial positions of the spheres. make it in circle with r radius.
            // https://www.cuemath.com/geometry/unit-circle/
            r = 4.5f; // radius of the circle
            
            float sin  = Mathf.Sin(i * 2 * Mathf.PI / numSphere);
            float cos = Mathf.Cos(i * 2 * Mathf.PI / numSphere);
            spheresInitialPositions[i] = new Vector3(
                r * (Mathf.Sqrt(2) * sin * sin * sin),
                r * ((-1 * cos * cos * cos) - (cos * cos) + (2 * cos)) + 3f,
                5f);
            //spheresInitialPositions[i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numSphere), r * Mathf.Cos(i * 2 * Mathf.PI / numSphere), 10f);
            spheres[i].transform.position = spheresInitialPositions[i];

            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // hsv color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

         for (int i =0; i < numSphere; i++){

            // Color Update over time
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Sin(time)), Mathf.Cos(time), 1.5f + Mathf.Cos(time)); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }
}
