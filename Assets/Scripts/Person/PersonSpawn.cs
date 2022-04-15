using System;
using Events;
using SimpleEventBus.Disposables;
using UnityEngine;
using UnityEngine.UI;

public class PersonSpawn : MonoBehaviour
{
    [SerializeField] 
    private GameObject _personPrefab;
    [SerializeField] 
    private Transform[] _spawnPoint;
    [SerializeField] 
    private Image _diedPanel;

    private CompositeDisposable _subscriptions;
    
    private void Awake()
    {
        _subscriptions = new CompositeDisposable
        {
            EventStreams.Game.Subscribe<PersonDiedEvent>(SpawnPerson)
        };
    }

    private void SpawnPerson(PersonDiedEvent eventData)
    {
        var random = new System.Random();
        var randomPoint = random.Next(_spawnPoint.Length);
        
        Instantiate (_personPrefab, _spawnPoint[randomPoint]);
        _diedPanel.enabled = false;
    }

    private void OnDestroy()
    {
        _subscriptions?.Dispose();
    }
}
