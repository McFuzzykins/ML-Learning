using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoal : Agent
{
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private Material winMat;
    [SerializeField]
    private Material loseMat;
    [SerializeField]
    private MeshRenderer floorMeshRenderer;

    public float moveSpeed;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(+4f, -1.25f), 0, Random.Range(-2.4f, +1.4f));
        targetTransform.localPosition = new Vector3(Random.Range(-2.45f, -4.15f), 0, Random.Range(+1.75f, -2.8f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log(actions.ContinuousActions[0]);
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed; 
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            SetReward(+1f);
            floorMeshRenderer.material = winMat;
            EndEpisode();
        }
        if (other.CompareTag("Wall"))
        {
            SetReward(-1f);
            floorMeshRenderer.material = loseMat;
            EndEpisode();
        }
    }
}
