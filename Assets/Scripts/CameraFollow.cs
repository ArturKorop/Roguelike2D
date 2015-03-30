using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float Smoothing = 5f;

    private Vector3 offset;

    public void Start()
    {
        this.offset = this.transform.position - this.Target.position;
    }

    public void FixedUpdate()
    {
        var targetCamPos = this.Target.position + this.offset;
        this.transform.position = Vector3.Lerp(this.transform.position, targetCamPos, this.Smoothing * Time.deltaTime);
    }
}
