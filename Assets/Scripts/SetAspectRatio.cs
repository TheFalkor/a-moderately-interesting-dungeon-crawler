using UnityEngine;

public class SetAspectRatio : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isEditor)
            return;

        Screen.SetResolution(Screen.width, Screen.width*9/16, Screen.fullScreen);
    }
}