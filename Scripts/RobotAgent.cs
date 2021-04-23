using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
public class RobotAgent : Agent
{

    [SerializeField] private MapGenerator m_mapGenerator;

    private AgentMouvement m_agentMouvement;
    private Shoot m_shoot;

    public override void Initialize()
    {
        m_shoot = GetComponent<Shoot>();
        m_agentMouvement = GetComponent<AgentMouvement>();
    }
    public override void OnEpisodeBegin()
    {
   
        m_mapGenerator.Reset();
        m_shoot.ShootRestart();
    }
    public override void OnActionReceived(float[] vectorAction)
    {

        AddReward(1f);
        m_agentMouvement.Mouvement(vectorAction[0], vectorAction[1], vectorAction[2]);
        if (vectorAction[3] == 1) { m_shoot.Fire(); }

    }

    public override void CollectObservations(VectorSensor sensor)
    {

        sensor.AddObservation(m_shoot.currentTime);
        sensor.AddObservation(transform.rotation.y);
    }
    public void Dead()
    {
        AddReward(-5.0f);
        EndEpisode();
    }
}
