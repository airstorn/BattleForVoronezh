using System.Collections;
using System.Collections.Generic;
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
    
    public void Release(Vector3 spawnPosition, ref GridElement hitElement)
    {
        if (hitElement.HoldedUnit)
        {
            var enemyHealth = hitElement.HoldedUnit.GetComponent<UnitHealth>();

            if (enemyHealth)
            {
                enemyHealth.ApplyDamage(_damage);
                hitElement.SetSpriteType(GridSprites.SpriteState.damaged);
            }

            var spawendObject = Instantiate(_hitTemplate, spawnPosition, Quaternion.Euler(-90, 0, 0));
            spawendObject.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            hitElement.SetSpriteType(GridSprites.SpriteState.missed);
            
            var spawendObject = Instantiate(_missTemplate, spawnPosition, Quaternion.Euler(-90, 0, 0));
            spawendObject.GetComponent<ParticleSystem>().Play();
        }
    }
}
