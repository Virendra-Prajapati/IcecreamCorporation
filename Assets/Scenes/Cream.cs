using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cream : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public void SetProperties(float time, Material mat, Vector3 endPosition, Quaternion endRotation)
    {
        float speed = 2 - time;
        meshRenderer.material = mat;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(endPosition, speed));
        sequence.Insert(0.7f, transform.DORotateQuaternion(endRotation, speed - 1));
    }
}
