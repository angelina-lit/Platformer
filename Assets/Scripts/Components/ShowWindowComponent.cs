using UnityEngine;

public class ShowWindowComponent : MonoBehaviour
{
    [SerializeField] private string _path;

    public void Show()
    {
        WindowUtils.CreateWindow(_path);
    }
}
