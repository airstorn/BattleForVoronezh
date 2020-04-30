using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuPagable
{
    void Show<T>(T arg);
    void Hide();
}
