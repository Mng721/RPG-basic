using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health = 3f;

    [Header("Combat")]
    [SerializeField] private float _attackCD = 3f;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _aggroRange = 4f;


    private GameObject _player;
    private NavMeshAgent _agent;
    private Animator _animator;
    private float _timePassed;
    private float _newDestinationCD = 0.5f;

    private int _animIDDamaged;
    private int _animIDSpeed;
    private int _animIDAttack;

    private void Start() {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();

        _animIDDamaged = Animator.StringToHash("Hit");
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDAttack = Animator.StringToHash("Attack");

    }


    private void Update() {
        _animator.SetFloat(_animIDSpeed, _agent.velocity.magnitude / _agent.speed);

        if(_timePassed >= _attackCD) {
            if(Vector3.Distance(_player.transform.position, transform.position) <= _attackRange) {
                _animator.SetTrigger(_animIDAttack);
                _timePassed = 0;
            }
        }

        _timePassed += Time.deltaTime;

        if(_newDestinationCD <= 0 && Vector3.Distance(_player.transform.position, transform.position) <= _aggroRange) {
            _newDestinationCD = 0.5f;
            _agent.SetDestination(_player.transform.position);
        }
        _newDestinationCD -= Time.deltaTime;
        transform.LookAt(_player.transform);
    }

    public void TakeDamage(float damageAmount) {
        _health -= damageAmount;
        _animator.SetTrigger(_animIDDamaged);

        if(_health <= 0) { 
            Die();
        }
    }


    private void Die() {
        Destroy(this.gameObject);
    }


    public void StartDealDamage() {
        GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }

    public void StopDealDamage() {
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamge();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _aggroRange);

    }
}
