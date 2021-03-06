using System;
using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class PersonDiedState : MonoBehaviour
{
    [SerializeField] 
    private Image _diedPanel;
    [SerializeField] 
    private float _timeToRespawn = 10f;

    private Animator _animator;
    private void Awake()
    {
        PersonHealthController.Died += OnDied;

        _animator = GetComponent<Animator>();
    }

    private void OnDied()
    {
        _animator.Play("Died");
        _diedPanel.enabled = true;
        
        StartCoroutine(TimeToRespawn());
    }

    private IEnumerator TimeToRespawn ()
    {
        yield return new WaitForSeconds(_timeToRespawn);
        EventStreams.Game.Publish(new PersonDiedEvent());
    }
    private void OnDestroy()
    {
        PersonHealthController.Died -= OnDied;
    }
}
