using System;
using System.Collections;
using System.Collections.Generic;
using GameStates;
using UnityEngine;
using UnityEngine.UI;

public class PlacementPage : PageBasement, IMenuPagable
{
    [SerializeField] private Button _confirm;
    [SerializeField] private Button _rotate;
    [SerializeField] private Button _randomize;
    [SerializeField] private Button _menuButton;
    [SerializeField] private PausePage _pause;

    private void Start()
    {
        _menuButton.onClick.AddListener(delegate { _pause.OpenPause(); });
    }

    public override void Show<T>(T args)
    {
        base.Show(args);
        if (args is PlaceUnits units)
        {
            _confirm.onClick.AddListener(delegate { units.Confirm(); });
            _rotate.onClick.AddListener(delegate { units.RotateElement(); });
            _randomize.onClick.AddListener(delegate { units.PlaceRandomly(); });
        }
    }
}
