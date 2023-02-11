using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnLocation;
    [SerializeField] private Rigidbody2D _inheritVelocityFrom;

    public void SpawnObject(GameObject prefab)
    {
        
        var go = Instantiate(prefab, _spawnLocation.position, _spawnLocation.rotation);
        var rb = go.GetComponent<Rigidbody2D>();
        if (rb != null && _inheritVelocityFrom != null)
        {
            rb.AddForce(_inheritVelocityFrom.velocity * Vector2.right, ForceMode2D.Impulse);
        }
    }
}
