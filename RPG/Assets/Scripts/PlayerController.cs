using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private UI_Inventory ui_inventory;
    [SerializeField] private int inventoryCapacity;
    public Inventory Inventory { get; private set; }
    private float _lastInventoryOpen;

    void Start() {
        Inventory = new Inventory(inventoryCapacity);
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    agent.SetDestination(hit.point);
                    animator.SetBool("Walk", true);
                }
            }
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                animator.SetBool("Walk", false);
            }
        }

        if (Input.GetKey(KeyCode.I) && Time.time - _lastInventoryOpen > 0.3f)
        {
            ui_inventory.ChangeVisibility();
            _lastInventoryOpen = Time.time;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Interactable obj = collision.gameObject.GetComponent<Interactable>();

        if (obj is ItemBaseController)
        {
            (obj as ItemBaseController).PickUpTarget = Inventory;
        }

        obj?.Interact();
    }
}