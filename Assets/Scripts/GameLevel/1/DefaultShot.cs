using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultShot : MonoBehaviour, IShotable
{
    [SerializeField] private GameObject _hitTemplate;
    [SerializeField] private GameObject _missTemplate;
    
    public IEnumerator Release(Vector3 spawnPosition, bool hit)
    {
        if (hit == true)
        {
            var spawendObject = Instantiate(_hitTemplate, spawnPosition, Quaternion.Euler(-90, 0, 0));
            spawendObject.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            var spawendObject = Instantiate(_missTemplate, spawnPosition, Quaternion.Euler(-90, 0, 0));
            spawendObject.GetComponent<ParticleSystem>().Play();
        }

        yield return new WaitForSeconds(1);
    }
}
