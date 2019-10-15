using System.Collections;
using UnityEngine;

public class LandscapeOrientation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
    }

    // Update is called once per frame
    void Update()
    {
        if(Screen.width < Screen.height)
            Screen.orientation = ScreenOrientation.Landscape;
    }
}
