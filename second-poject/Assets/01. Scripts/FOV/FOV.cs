using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float eyeRadius = 5f;
    [Range(0,360)]
    public float eyeAngle = 90f;

    public LayerMask targetLayerMask;
    public LayerMask blockLayerMask;

    private List<Transform> targetLists = new List<Transform>();
    private Transform firstTarget;
    private float distanceTarget = 0.0f;

    public List<Transform> TargetLists => targetLists;
    public Transform FirstTarget => firstTarget;
    public float DistanceTarget => distanceTarget;

    void FindTargets()
    {
        distanceTarget = 0.0f;
        firstTarget = null;

        targetLists.Clear();

        Collider[] overlapSphereTargets = Physics.OverlapSphere(transform.position, eyeRadius, targetLayerMask);
        for(int i = 0; i < overlapSphereTargets.Length; i++)
        {
            Transform target = overlapSphereTargets[i].transform;

            Vector3 LookAtTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, LookAtTarget) < eyeAngle / 2)
            {
                float nowFirstDistanceTarget = Vector3.Distance(transform.position, target.position);
               
                if (!Physics.Raycast(transform.position, LookAtTarget, nowFirstDistanceTarget, blockLayerMask)) 
                TargetLists.Add(target);
                if (firstTarget==null||(distanceTarget>nowFirstDistanceTarget))
                {
                    firstTarget = target;
                    distanceTarget = nowFirstDistanceTarget;
                }
            }
        }
    }
    public float delayFindTime = 0.2f;
   
    void Start()
    {
        StartCoroutine("updateFindTargets", delayFindTime);
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    IEnumerator updateFindTargets(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
      
    }
    public Vector3 findTargetAngle(float degrees,bool flagFlobalAngle)
    {
        if (!flagFlobalAngle)
        {
            degrees += transform.eulerAngles.y;

        }
        return new Vector3(Mathf.Sin(degrees * Mathf.Deg2Rad), 0, Mathf.Cos(degrees * Mathf.Deg2Rad));
    }
}
