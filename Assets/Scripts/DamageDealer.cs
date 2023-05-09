using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    private bool _canDealDamage;
    private List<GameObject> _hasDealtDamage;

    [SerializeField] private float _weaponLength;
    [SerializeField] private float _weaponDamage;


    private void Start() {
        _canDealDamage = false;
        _hasDealtDamage = new List<GameObject>();
    }

    private void Update() {
        if(_canDealDamage) {
            RaycastHit hit;

            int layerMask = 1 << 9;
            if(Physics.Raycast(transform.position, - transform.up, out hit, _weaponLength, layerMask)) {
                if(hit.transform.TryGetComponent(out Enemy enemy) && !_hasDealtDamage.Contains(hit.transform.gameObject)) {
                    enemy.TakeDamage(_weaponDamage);
                    _hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
        }        
    }

    public void StartDealDamage() {
        _canDealDamage = true;
        _hasDealtDamage.Clear();
    }

    public void EndDealDamge() {
        _canDealDamage = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * _weaponLength);
    }
}
