﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace BridgeGame
{
    public class ConnectionPointView : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
    {
        public ConnectionPoint point;
        private InteractionController interactionController;

        private static ConnectionPointView buildEnd;

        private void Awake()
        {
            interactionController = FindObjectOfType<InteractionController>();
        }

        private void LateUpdate()
        {
            transform.position = point.physics.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("Drag");
            if (interactionController.selectedTool == InteractionController.Tool.Move)
            {
                Plane plane = new Plane(Vector3.forward, Vector3.zero);
                var ray = Camera.main.ScreenPointToRay(eventData.position);
                if (plane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    transform.position = hitPoint;
                    point.position = point.preferedPosition = transform.position;

                    Debug.Log("MOVE!");
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactionController.selectedTool == InteractionController.Tool.Delete)
                point.bridge.RemovePoint(point);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (buildEnd == this)
                buildEnd = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            buildEnd = this;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (buildEnd != null)
                point.bridge.AddConnection(point, buildEnd.point, interactionController.selectedType);
            else
            {
                Plane plane = new Plane(Vector3.forward, Vector3.zero);
                var ray = Camera.main.ScreenPointToRay(eventData.position);
                if (plane.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint = ray.GetPoint(enter);
                    var newPoint = point.bridge.AddPoint(hitPoint);
                    point.bridge.AddConnection(point, newPoint, interactionController.selectedType);

                    Debug.Log("BUILD!");
                }
            }
        }
    }
}
