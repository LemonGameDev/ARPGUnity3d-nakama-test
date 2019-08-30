using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToClick
{
    private GameObject rotable;
    private Rigidbody _rigidbody;
    private float rotSpeed;
    private Quaternion lookAtTarget;

    public RotateToClick(GameObject gameObject, Rigidbody _rigidbody)
    {
        rotable = gameObject;
        this._rigidbody = _rigidbody;
        rotSpeed = 20;
    }

    public void Rotate(Vector3 pos, float rotationSpeed)
    {
        var lookAt = new Vector3(pos.x - rotable.transform.position.x,
            rotable.transform.position.y, pos.z - rotable.transform.position.z);
        lookAtTarget = Quaternion.LookRotation(lookAt);
    }

    public IEnumerator SmoothRotation(Vector3 pos, float rotationSpeed)
    {
        var rotableTransform = rotable.transform;
        float dotProd = 0;
        while (dotProd <= 0.99f)
        {
            var dirFromAtoB = (pos - rotableTransform.position).normalized;
            dotProd = Vector3.Dot(rotableTransform.forward, dirFromAtoB);
            if (dotProd == 0) yield break;
            Vector3 dir = pos - rotableTransform.position;
            dir.y = 0; // keep the direction strictly horizontal
            Quaternion rot = Quaternion.LookRotation(dir);
            rotableTransform.rotation =
                Quaternion.Slerp(rotableTransform.rotation, rot, rotationSpeed * Time.deltaTime);


            yield return new WaitForSeconds(.001f);
        }
    }

    public void Tick()
    {
        if (lookAtTarget != null) return;
        rotable.transform.rotation = Quaternion.Slerp(
            rotable.transform.rotation,
            lookAtTarget, rotSpeed * Time.deltaTime);
    }
}