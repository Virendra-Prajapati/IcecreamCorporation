using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FlavourType flavour;
    public void OnPointerDown(PointerEventData eventData)
    {
        IcecreamGenerator.instance.RaiseOnGenerateIcecream(flavour);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IcecreamGenerator.instance.RaiseOnStopGeneration();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(0.8f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1, 0.2f);
    }

}
