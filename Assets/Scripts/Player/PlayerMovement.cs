using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;     // �ȱ� �ӵ�
    public float jumpForce = 7f;     // ���� ����
    public float airControl = 0.5f;  // ���� �̵� ����(0~1)

    private Rigidbody rb;
    private bool isGrounded = false;
    private Vector3 inputDir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // ȸ�� ���� (�Ѿ����� �ʰ�)
    }

    void Update()
    {
        // --- �̵� �Է� ---
        float h = Input.GetAxisRaw("Horizontal"); // A,D or ��, ��
        float v = Input.GetAxisRaw("Vertical");   // W,S or ��, ��
        inputDir = new Vector3(h, 0f, v).normalized;

        // --- ���� �Է� ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // �̵� ���� ��� (ī�޶� ������ �ƴ϶� ���� ����)
        Vector3 move = transform.TransformDirection(inputDir) * moveSpeed;

        // ���� ���� (isGrounded �ƴ� ���� ����� ���̱�)
        if (!isGrounded)
            move *= airControl;

        Vector3 velocity = rb.velocity;
        velocity.x = move.x;
        velocity.z = move.z;
        rb.velocity = velocity;
    }

    void Jump()
    {
        // �������� ���� �� �߰�
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // ���� y�ӵ� �ʱ�ȭ
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    // --- �ٴ� ���� ���� ---
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
