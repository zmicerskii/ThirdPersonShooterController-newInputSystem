using System;
using UnityEngine;
using UnityEngine.UI;

public class PersonSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _personPrefab;
    [SerializeField] private Transform[] _spawnPoint;
    [SerializeField] private Image _diedPanel;

    private void Awake()
    {
        PersonDiedState.PersonDied += SpawnPerson;
    }

    private void SpawnPerson()
    {
        var random = new System.Random();
        var randomPoint = random.Next(_spawnPoint.Length);
        
        Instantiate (_personPrefab, _spawnPoint[randomPoint]);
        _diedPanel.enabled = false;
    }

    private void OnDisable()
    {
        PersonDiedState.PersonDied -= SpawnPerson;
    }
}
