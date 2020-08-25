using System.Collections;
using System.Collections.Generic;
using GameStates;
using GUI.Core;
using UnityEngine;

public class DefaultShot : MonoBehaviour, IShotable
{
    [SerializeField] private GameObject _hitTemplate;
    [SerializeField] private int _damage = 1;

    [SerializeField]
    private GameObject _missTemplate
    {
        get { return _hitTemplate; }
    }
    
    public void Release(ref GridElement hitElement)
    {
        if (hitElement.HoldedUnit)
        {
            var enemyHealth = hitElement.HoldedUnit.GetComponent<UnitHealth>();

            if (enemyHealth)
            {
                enemyHealth.ApplyDamage(_damage);
                hitElement.SetSpriteType(GridSprites.SpriteState.damaged);
                SoundsPlayer.Instance.PlaySound(SoundType.Hit);
            }

            var spawendObject = Instantiate(_hitTemplate, hitElement.CellPos, Quaternion.Euler(-90, 0, 0));
            spawendObject.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            hitElement.SetSpriteType(GridSprites.SpriteState.missed);
            SoundsPlayer.Instance.PlaySound(SoundType.HitMissed);
            
            var spawendObject = Instantiate(_missTemplate, hitElement.CellPos, Quaternion.Euler(-90, 0, 0));
            spawendObject.GetComponent<ParticleSystem>().Play();
        }
    }
}
