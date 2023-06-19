using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MachineHandle : MonoBehaviour
{
    public Transform strawberryStick, choclateStick;
    private Transform currentSelection;
    public Vector3 onRotate;

    private void Start()
    {
        IcecreamGenerator.instance.onGenerateIceCream += OnGenerating;
        IcecreamGenerator.instance.onStopGeneration += MoveToDefault;
    }

    private void OnGenerating(FlavourType flavourType)
    {
        switch (flavourType)
        {
            case FlavourType.Strawberry:
                if(currentSelection != null && currentSelection != strawberryStick)
                    MoveToDefault();
                currentSelection = strawberryStick;
                break;
            case FlavourType.Choclate:
                if(currentSelection != null && currentSelection != choclateStick)
                    MoveToDefault();
                currentSelection = choclateStick;
                break;
        }
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
