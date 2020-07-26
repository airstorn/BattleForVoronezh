using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PageBasement : MonoBehaviour
{
    [SerializeField] protected GameObject _pageObject;
    public virtual void Show()
    {
        _pageObject.SetActive(true);   
    }
    
    public virtual void Hide()
    {
        _pageObject.SetActive(false);
    }
}
