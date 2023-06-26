using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MachineHandle : MonoBehaviour
{
    public Transform[] sticks;
    private Transform currentSelection;
    public Vector3 onRotate;

    private void Start()
    {
        IcecreamGenerator.instance.onGenerateIceCream += OnGenerating;
        IcecreamGenerator.instance.onStopGeneration += MoveToDefault;
    }

    private void OnGenerating(FlavourType flavourType)
    {
        if(currentSelection != null && currentSelection != sticks[(int)flavourType])
            MoveToDefault();
        currentSelection = sticks[(int)flavourType];
        MoveToGenerate();
    }

    private void MoveToDefault()
    {
        currentSelection.DORotate(Vector3.zero, 0.2f);
    }

    private void MoveToGenerate()
    {
        currentSelection.DORotate(onRotate, 0.2f);
    }
}
