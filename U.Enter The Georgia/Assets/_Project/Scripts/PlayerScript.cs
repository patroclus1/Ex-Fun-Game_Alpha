using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    // Controls
    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction move;
    private InputAction dash;
    private InputAction escape;

    private Vector2 input;
    private Vector3 moveDirection;

    // Animation
    private Animator anim;

    // Gameplay
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashTime = 0.25f;
    [SerializeField] private int health;

    private bool isDashing;
    private bool isPlayerAlive = true;

    private Vector3 persistantHeight;

    // FX and UI
    [SerializeField] private LayerMask defaultLayer;
    [SerializeField] private LayerMask transparentLayer;
    [SerializeField] private Light playerLight;
    [SerializeField] private GameObject playerGFX;
    [SerializeField] private CameraShake cameraFX;
    [SerializeField] private ParticleSystem deathFX;

    [SerializeField] private Texture2D cursor;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject menuButtonScreen;
    private Color defaultColor;

    public int getHealth { get => health; }

    public bool isAlive
    {
        get { return isPlayerAlive; }
    }

    public bool setLife
    {
        set { isPlayerAlive = value; }
    }


    void Awake()
    {
        deathScreen.SetActive(false);
        menuButtonScreen.SetActive(false);

        Vector2 hotspot = new Vector2(cursor.width/2, cursor.height/2);
        Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;

        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        move = playerInput.actions["Move"];
        dash = playerInput.actions["Dash"];
        escape = playerInput.actions["Escape"];

        defaultColor = playerLight.color;
        gameObject.layer = defaultLayer;
    }


    private void Update()
    {
        Move();
        Dash();
        OpenMenuOnEscape();
    }

    private void Move()
    {
        if (isDashing) { return; }

        input = move.ReadValue<Vector2>();
        moveDirection = new Vector3(input.x, 0, input.y);

        if (moveDirection == Vector3.zero) { return; }

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        
        anim.SetFloat("Speed", input.y);

        if (transform.position.y == 1f) { return; }

        PersistHeight();
    }

    private void PersistHeight()
    {
        persistantHeight = new Vector3(transform.position.x, 1f, transform.position.z);
        transform.position = persistantHeight;
    }

    private void Dash()
    {
        if (isDashing) { return; }

        if (dash.triggered)
        {
            StartCoroutine(StartDash());
            StartCoroutine(cameraFX.cameraShake(0.1f, 0.04f));
        }
    }

    private IEnumerator StartDash()
    {
        float startTime = Time.time;
        isDashing = true;
        gameObject.layer = transparentLayer;

        while (Time.time < startTime + dashTime)
        {
            controller.Move(moveDirection.normalized * dashForce * Time.deltaTime);
            yield return null;
        }
        isDashing = false;
        gameObject.layer = defaultLayer;
    }

    public void TakeDamage(int dmg)
    {
        if (health > 0)
        {
            StartCoroutine(StartAlphaFlicker());
            health -= dmg;
        }
        if (health <= 0)
        {
            StopAllCoroutines();
            health = 0;
            PlayerDied();
        }
        StartCoroutine(cameraFX.cameraShake(0.1f, 0.1f));
    }

    public void PlayerDied()
    {
        var particles = Instantiate(deathFX, transform.position, Quaternion.identity);
        Destroy(particles.gameObject, 2);
        Destroy(playerGFX, 0.5f);
        
        enabled = false;
        isPlayerAlive = false;
        enabled = false;

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None;
        menuButtonScreen.SetActive(false);
        deathScreen.SetActive(true);
    }

    private IEnumerator StartAlphaFlicker()
    {
        gameObject.layer = transparentLayer;
        gameObject.tag = "Untagged";
        for (int i = 0; i < 3; i++)
        {
            playerLight.color = Color.black;
            yield return new WaitForSeconds(0.2f);
            playerLight.color = Color.red;
            yield return new WaitForSeconds(0.2f);
        }
        playerLight.color = defaultColor;
        gameObject.layer = defaultLayer;
        gameObject.tag = "Player";
    }

    private void OpenMenuOnEscape()
    {
        if (escape.triggered && !menuButtonScreen.activeInHierarchy)
        {
            menuButtonScreen.SetActive(true);
        }
        else if (escape.triggered && menuButtonScreen.activeInHierarchy)
        {
            menuButtonScreen.SetActive(false);
        }
    }
}
