using UnityEngine;
using System.Collections.Generic;

public class ChainPathDrawer : MonoBehaviour
{
    public GameObject trackLinkPrefab;
    public float linkSpacing = 0.3f;
    public List<Vector2> pathPoints = new List<Vector2>();
    public bool buildChain = false;

    private GameObject[] links;

    void Update()
    {
        if (buildChain)
        {
            BuildChain();
            buildChain = false;
        }
    }

    void BuildChain()
    {
        if (pathPoints.Count < 2) return;

        float totalLength = 0f;
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            totalLength += Vector2.Distance(pathPoints[i], pathPoints[i + 1]);
        }

        int numberOfLinks = Mathf.FloorToInt(totalLength / linkSpacing);
        links = new GameObject[numberOfLinks];

        float distanceSoFar = 0f;
        int linkIndex = 0;

        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            Vector2 start = pathPoints[i];
            Vector2 end = pathPoints[i + 1];
            float segmentLength = Vector2.Distance(start, end);
            Vector2 dir = (end - start).normalized;

            while (distanceSoFar + linkSpacing <= totalLength && linkIndex < numberOfLinks)
            {
                float t = (distanceSoFar - GetLengthUpTo(i)) / segmentLength;
                Vector2 pos = Vector2.Lerp(start, end, t);
                Vector3 worldPos = transform.TransformPoint(pos);

                GameObject link = Instantiate(trackLinkPrefab, worldPos, Quaternion.identity, transform);
                link.transform.right = dir;
                links[linkIndex] = link;

                if (linkIndex > 0)
                {
                    ConnectLinks(links[linkIndex - 1], link);
                }

                linkIndex++;
                distanceSoFar += linkSpacing;
            }
        }

        if (numberOfLinks > 1)
        {
            ConnectLinks(links[numberOfLinks - 1], links[0]);
        }
    }

    float GetLengthUpTo(int index)
    {
        float length = 0f;
        for (int i = 0; i < index; i++)
        {
            length += Vector2.Distance(pathPoints[i], pathPoints[i + 1]);
        }
        return length;
    }

    void ConnectLinks(GameObject a, GameObject b)
    {
        HingeJoint2D joint = a.AddComponent<HingeJoint2D>();
        joint.connectedBody = b.GetComponent<Rigidbody2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector2.zero;
        joint.connectedAnchor = Vector2.zero;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector3 worldPos = transform.TransformPoint(pathPoints[i]);
            Gizmos.DrawSphere(worldPos, 0.05f);

            if (i > 0)
            {
                Vector3 prev = transform.TransformPoint(pathPoints[i - 1]);
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(prev, worldPos);
            }
        }
    }
}
