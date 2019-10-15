using UnityEngine;

public class PortraitOrientation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.width > Screen.height)
            Screen.orientation = ScreenOrientation.Portrait;
    }
}
