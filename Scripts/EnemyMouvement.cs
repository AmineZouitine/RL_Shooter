using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMouvement : MonoBehaviour
{
    [SerializeField] private LayerMask m_layerMask;
    [SerializeField] private float m_timeBetweenMouvement;
    private NavMeshAgent m_navmesh;
    private Transform m_playerTransform;
    private void Awake()
    {
        m_navmesh = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50f, m_layerMask);
        if (hitColliders.Length > 0)
        {
            m_playerTransform = hitColliders[0].gameObject.transform;
        }
    }

    void Update()
    {
        if (m_playerTransform != null)
        {
            m_navmesh.SetDestination(m_playerTransform.position);

        }
    }
}
