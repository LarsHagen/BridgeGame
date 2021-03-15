using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Rigidbody2D pointA;
    public Rigidbody2D pointB;

    public HingeJoint2D hingeJoint2D;
    public SliderJoint2D sliderJoint2D;
    public EdgeCollider2D edgeCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector2.Lerp(pointA.position, pointB.position, 0.5f);
        //transform.right = pointB.transform.position - transform.position;
        Vector3 dir = pointB.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        edgeCollider2D.SetPoints(new List<Vector2>() { transform.InverseTransformPoint(pointA.position), transform.InverseTransformPoint(pointB.position) });
        hingeJoint2D.connectedBody = pointA;
        hingeJoint2D.anchor = edgeCollider2D.points[0];
        sliderJoint2D.connectedBody = pointB;
        sliderJoint2D.anchor = edgeCollider2D.points[1];
    }

    // Update is called once per frame
    void Update()
    {
        //float dist = Vector2.Distance(pointA.position, pointB.position);
        transform.position = Vector2.Lerp(pointA.position, pointB.position, 0.5f);
    }
}
