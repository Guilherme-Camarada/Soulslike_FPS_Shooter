using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AgentLinkMover : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;


    IEnumerator Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.autoTraverseOffMeshLink = false;

        while (true)
        {
            if (_navMeshAgent.isOnOffMeshLink)
            {
                yield return StartCoroutine(Parabola(_navMeshAgent, 2.0f, 0.5f));

                _navMeshAgent.CompleteOffMeshLink();
            }

            yield return null;
        }
    }

    private IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;

        Vector3 startPosition = agent.transform.position;
        Vector3 endPosition = data.endPos + Vector3.up * agent.baseOffset;

        float normalizedTime = 0f;

        while (normalizedTime < 1f)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            _navMeshAgent.transform.position = Vector3.Lerp(startPosition, endPosition, normalizedTime) + Vector3.up * yOffset;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
}
