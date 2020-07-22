using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _count;
    
    public void SetItem(string name, string count)
    {
        _count.text = count;
        _name.text = name;
    }
}
