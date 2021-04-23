using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private GameObject m_destroyEffect;
    [SerializeField] private LayerMask m_playerMask;
    private RobotAgent m_robotAgent;



    private void DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50f, m_playerMask);
        if (hitColliders.Length > 0)
        {
            m_robotAgent = hitColliders[0].gameObject.GetComponent<RobotAgent>();
        }
    }

    public void Explose()
    {
        DetectPlayer();
        if (m_robotAgent != null)
        {
            m_robotAgent.AddReward(5f);
            Destroy(Instantiate(m_destroyEffect, transform.position, Quaternion.identity), 2);
            Destroy(gameObject);
        }       
    }
}
