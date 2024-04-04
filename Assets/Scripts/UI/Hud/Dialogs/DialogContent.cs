using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogContent : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Image _icon;

    public Text Text => _text;

    public void TrySetIcon(Sprite icon)
    {
        if(_icon != null) 
            _icon.sprite = icon;
    }
}
