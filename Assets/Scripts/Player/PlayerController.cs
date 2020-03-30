using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpSpeed = 50.0f;
    [SerializeField] private float gravity = 20.0f;
    [SerializeField] private float coverDistance = 0.5f;
    private Vector3 moveDirection = Vector3.zero;

    private float coverLerpStartTime = 0.0f;
    private float coverLerpTotalTime = 1.0f;
    private float vaultLerpStartTime = 0.0f;
    private float vaultLerpTotalTime = 1.0f;

    private Vector3 lastClosestVaultPoint = Vector3.zero;
    private Vector3[] lastClosestVaultFaces = new Vector3[4];
    private Vector3 vaultEndPoint = Vector3.zero;

    public bool CanVault { get; set; } = false;
    public bool vaulting = false;
    public GameObject coverObject = null;
    public UIManager uiManager;

    private void Awake()
    {
        characterController = transform.GetComponent<CharacterController>();
        mouseLook.Init(transform, Camera.main.transform);
        mouseLook.lockCursor = true;
        uiManager = GetComponent<UIManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mouseLook.LookRotation(transform, Camera.main.transform);
        if (coverObject != null && Input.GetButtonDown("Cover")) { Cover(); }
        if (CanVault && Input.GetButtonDown("Vault")) { Vault(); }
    }

    private void FixedUpdate()
    {
        mouseLook.UpdateCursorLock();
        Debug.DrawLine(lastClosestVaultPoint, vaultEndPoint, Color.red);

        if (vaulting)
        {
            if (characterController.enabled == true) { characterController.enabled = false; }
            float timeSinceStarted = Time.time - vaultLerpStartTime;
            float percentComplete = timeSinceStarted / vaultLerpTotalTime;

            transform.position = Vector3.Lerp(lastClosestVaultPoint, vaultEndPoint, percentComplete);

            if (percentComplete >= 1.0f)
            {
                vaulting = false;
                characterController.enabled = true;
            }
        }
    }

    private void Cover()
    {
        Debug.Log("OMG TAKING COVER!");
        BarycentricDistance barycentricDistance = new BarycentricDistance(coverObject.GetComponent<MeshFilter>());
        lastClosestVaultPoint = barycentricDistance.GetClosestTriangleAndPoint(transform.position).closestPoint;

        lastClosestVaultFaces = coverObject.transform.CalculateLocalXZFacePoints();
        Vector3 localizedPlayerPosition = coverObject.transform.InverseTransformPoint(transform.position);

        if(localizedPlayerPosition.x < lastClosestVaultFaces[2].x && localizedPlayerPosition.x > lastClosestVaultFaces[3].x)
        {
            //Vector3 endpoint = coverObject.transform
        }
    }

    private void Vault()
    {
        BarycentricDistance barycentricDistance = new BarycentricDistance(coverObject.GetComponent<MeshFilter>());
        lastClosestVaultPoint = barycentricDistance.GetClosestTriangleAndPoint(transform.position).closestPoint;

        lastClosestVaultFaces = coverObject.transform.CalculateLocalXZFacePoints();
        Vector3 localizedPlayerPosition = coverObject.transform.InverseTransformPoint(transform.position);

        if (localizedPlayerPosition.x < lastClosestVaultFaces[2].x && localizedPlayerPosition.x > lastClosestVaultFaces[3].x)
        {
            Vector3 endpoint = coverObject.transform.InverseTransformPoint(lastClosestVaultPoint);
            endpoint.z *= -1;
            vaultEndPoint = coverObject.transform.TransformPoint(endpoint);
        }
        else
        {
            Vector3 endpoint = coverObject.transform.InverseTransformPoint(lastClosestVaultPoint);
            endpoint.x *= -1;
            vaultEndPoint = coverObject.transform.TransformPoint(endpoint);
        }

        vaultLerpTotalTime = Vector3.Distance(lastClosestVaultPoint, vaultEndPoint) / 4;
        vaultLerpStartTime = Time.time;
        vaulting = true;
    }

    private void HandleMovement(float horizontal, float vertical)
    {
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(horizontal, 0.0f, vertical);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;
            if (Input.GetKeyDown(KeyCode.Space)) { moveDirection.y = jumpSpeed; }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (coverObject != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireMesh(coverObject.GetComponent<MeshFilter>().mesh,
                coverObject.transform.position,
                coverObject.transform.rotation,
                coverObject.transform.localScale);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(coverObject.transform.TransformPoint(lastClosestVaultFaces[0]), 0.1f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(coverObject.transform.TransformPoint(lastClosestVaultFaces[1]), 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(coverObject.transform.TransformPoint(lastClosestVaultFaces[2]), 0.1f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(coverObject.transform.TransformPoint(lastClosestVaultFaces[3]), 0.1f);

            Gizmos.color = Color.white;
            Gizmos.DrawSphere(lastClosestVaultPoint, 0.1f);

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vaultEndPoint, 0.1f);
        }
    }
}