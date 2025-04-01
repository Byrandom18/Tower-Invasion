using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll : MonoBehaviour
{
    private Rigidbody2D rb;
    public float rollSpeed = 5f; // �������� �������
    public float rollDuration = 0.5f; // ������������ �������
    private float rollTimer; // ������
    private bool isRolling = false; // ���� �������
    private Animator animator; // ��� ���������� ���������
    private int originalLayer; // ��������� �������� ���� ���������
    private int ignoreLayer; // ���� ��� ������������

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalLayer = gameObject.layer; // ��������� �������� ����
        ignoreLayer = LayerMask.NameToLayer("IgnoreCollisions"); // ��������� ���� ������������
    }

    void Update()
    {
        // �������� ����� ��� ������� (��������, ������� "Space")
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isRolling)
        {
            StartRoll();
        }

        // ��������� �������
        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0)
            {
                StopRoll();
            }
        }
    }

    void StartRoll()
    {
        isRolling = true;
        rollTimer = rollDuration;
        animator.SetBool("isRolling", true); // ������ �������� �������

        // ������ ��������� ����������, ����� ����
        gameObject.layer = ignoreLayer;

        // ���������� ����������� ������� �� �����
        float direction = Input.GetAxisRaw("Horizontal"); // -1 (�����), 1 (������), 0 (��� �����)

        // ���� ��� �����, ���������� ������� ����������� ���������
        if (direction == 0)
        {
            direction = transform.localScale.x > 0 ? 1 : -1; // ������, ���� ������� ������, ����� �����
        }

        // ��������� �������� � ������ �����������
        rb.linearVelocity = new Vector2(direction * rollSpeed, rb.linearVelocity.y);

        // ������������ ��������� � ������� ������� (���� �����)
        if (direction != 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
        }
    }

    void StopRoll()
    {
        isRolling = false;
        animator.SetBool("isRolling", false); // ��������� ��������
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // ������������� �������������� ��������

        // ���������� �������� ����
        gameObject.layer = originalLayer;
    }
}

