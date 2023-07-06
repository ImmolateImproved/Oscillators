using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public bool dragged;

    public UnityEvent<bool> DragEvent;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        dragged = true;
        DragEvent.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragged = false;
        DragEvent.Invoke(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta * Time.deltaTime;
    }
}