using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
      [SerializeField] private string _username;
      [SerializeField] private int _coins;
      [SerializeField] private List<GridUnit> _units;
}
