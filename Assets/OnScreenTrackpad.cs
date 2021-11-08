using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem.Layouts;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenTrackpad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 basePosition;
    private Vector2 currentPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out basePosition);
        currentPosition = basePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out currentPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out currentPosition);
    }


    public Vector2 getDelta()
    {
        if (!this.isActiveAndEnabled) return new Vector2(0, 0);
        return currentPosition - basePosition;
    }

   void LateUpdate()
    {
        basePosition = currentPosition;
    }
}
