using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public string URL;

    public void Open()
    {
        Application.OpenURL(URL);
    }
}
