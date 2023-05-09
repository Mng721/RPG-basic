using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    private bool _canDealDamage;
    private bool _hasDealtDamage;

    [SerializeField] private float _weaponLength;
    [SerializeField] private float _weaponDamage;


    private void Start() {
        _canDealDamage = false;
        _hasDealtDamage = false;
    }

    private void Update() {
        if (_canDealDamage && !_hasDealtDamage) {
            RaycastHit hit;

            int layerMask = 1 << 8;
            if (Physics.Raycast(transform.position, -transform.up, out hit, _weaponLength, layerMask)) {
                if (hit.transform.TryGetComponent(out ThirdPersonController player)) {
                    player.TakeDamage(_weaponDamage);
                    _hasDealtDamage = true;
                }
            }
        }
    }

    public void StartDealDamage() {
        _canDealDamage = true;
        _hasDealtDamage = false;
    }

    public void EndDealDamge() {
        _canDealDamage = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * _weaponLength);
    }
}
