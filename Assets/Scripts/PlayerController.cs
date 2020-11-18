using System.Collections;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	public LayerMask whatIsGround;

	public GameObject bulletGreen;
	public GameObject bulletPink;
	public GameObject bulletOrange;
	public GameObject bulletYellow;
	

	static public int health=3;
	private int bulletCount = 1;
	private float lastShotgunTime;
	private float lastHurtTime;

	public float runSpeed = 40f;
	private AnimatorController animatorController;

	float horizontalMove = 0f;
	bool jump = false;
	static public bool dead;

	static public bool jon = false;
	static public bool sarah = false;
	static public bool aaron = false;
	static public bool mars = false;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Start()
	{
		health = 3;
		lastHurtTime = 0;
		lastShotgunTime = 0;
		dead = false;
		animatorController = GetComponent<AnimatorController>();
	}

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		if (dead) return;
		bool wasGrounded = m_Grounded;
		animatorController.Jump(!m_Grounded);
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		//move
		Move(horizontalMove * Time.fixedDeltaTime, false, jump);
		jump = false;

		if (transform.position.y < -6.5f && !dead)
		{
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
			health = 0;
			Hurt();
		}

	}

	private void Update()
	{
		if (dead) return;
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}

		if (Input.GetMouseButtonDown(0))
		{
			Shoot();
		}

		if (Input.GetMouseButtonDown(1))
		{
			Shotgun();
		}
		/*
		if (Input.GetKeyDown(KeyCode.J))
		{
			Shoot(1);
		}

		if (Input.GetKeyDown(KeyCode.K))
		{
			Shoot(2);
		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			Shoot(3);
		}

		if (Input.GetKeyDown(KeyCode.Semicolon))
		{
			Shoot(4);
		}
		*/

		if (Mathf.Abs(horizontalMove) > 0 && m_Grounded) animatorController.Walk(true);
		else animatorController.Walk(false);

	}


	public void Move(float move, bool crouch, bool jump)
	{
		if (dead) return;

		// Only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			Vector3 targetVelocity;
			// Move the character by finding the target velocity
			if(m_Grounded)
				targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			else
				targetVelocity = new Vector2(move * 10f-.1f, m_Rigidbody2D.velocity.y);
			

			// And then smoothing it out and applying it to the character
			if(m_Grounded)
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			else
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing+.15f);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		transform.Rotate(new Vector3(0,180f,0));
	}

	static public void Die()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void Hurt()
	{
		animatorController.TriggerHurt(--health);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (dead) return;
		Enemy enemy = collision.collider.GetComponent<Enemy>();
		Cupcake cupcake = collision.collider.GetComponent<Cupcake>();
		if(cupcake!=null || enemy!=null && Time.time-lastHurtTime>.2f)
		{
			Hurt();
			lastHurtTime = Time.time;
			if (collision.collider.name == "Cake")
			{
				m_Rigidbody2D.AddForce(new Vector2(-2000f, 250f));
			}
		}

	}

	private void Shoot()
	{
		bool shot = false;
		if(bulletCount==1 && jon)
		{
			GameObject go = Instantiate(bulletGreen, new Vector3(m_FacingRight ? transform.position.x + 1f : transform.position.x - 1f, transform.position.y + .8f, transform.position.z), Quaternion.identity) as GameObject;
			Bullet obj = go.GetComponent<Bullet>();
			obj.isFacingLeft = !m_FacingRight;
			obj.initialVelocity = horizontalMove;
			shot = true;
			{
				if (sarah)
					bulletCount = 2;
				else if (aaron)
					bulletCount = 3;
				else if (mars)
					bulletCount = 4;
			}
		}

		else if(bulletCount==2 && sarah || bulletCount<2 && sarah && !shot)
		{
			GameObject go = Instantiate(bulletPink, new Vector3(m_FacingRight ? transform.position.x + 1f : transform.position.x - 1f, transform.position.y + .4f, transform.position.z), Quaternion.identity) as GameObject;
			Bullet obj = go.GetComponent<Bullet>();
			obj.isFacingLeft = !m_FacingRight;
			obj.initialVelocity = horizontalMove;
			shot = true;
			{
				if (aaron)
					bulletCount = 3;
				else if (mars)
					bulletCount = 4;
				else if (jon)
					bulletCount = 1;
			}
		}

		else if (bulletCount == 3 && aaron || bulletCount < 3 && aaron && !shot)
		{
			GameObject go = Instantiate(bulletOrange, new Vector3(m_FacingRight ? transform.position.x + 1f : transform.position.x - 1f, transform.position.y + 0f, transform.position.z), Quaternion.identity) as GameObject;
			Bullet obj = go.GetComponent<Bullet>();
			obj.isFacingLeft = !m_FacingRight;
			obj.initialVelocity = horizontalMove;
			shot = true;
			{
				if (mars)
					bulletCount = 4;
				else if (jon)
					bulletCount = 1;
				else if (sarah)
					bulletCount = 2;
			}
		}

		else if (bulletCount == 4 && mars || bulletCount < 4 && mars && !shot)
		{
			GameObject go = Instantiate(bulletYellow, new Vector3(m_FacingRight ? transform.position.x + 1f : transform.position.x - 1f, transform.position.y + -.4f, transform.position.z), Quaternion.identity) as GameObject;
			Bullet obj = go.GetComponent<Bullet>();
			obj.isFacingLeft = !m_FacingRight;
			obj.initialVelocity = horizontalMove;
			shot = true;
			{
				if (jon)
					bulletCount = 1;
				else if (sarah)
					bulletCount = 2;
				else if (aaron)
					bulletCount = 3;
			}
		}


	}

	private void Shotgun()
	{
		if (lastShotgunTime != 0 && Time.time - lastShotgunTime < 1f) return;
		
		if(jon)
		{
			GameObject go = Instantiate(bulletGreen, new Vector3(m_FacingRight ? transform.position.x + 1f : transform.position.x - 1f, transform.position.y + .8f, transform.position.z), Quaternion.identity) as GameObject;
			Bullet obj = go.GetComponent<Bullet>();
			obj.isFacingLeft = !m_FacingRight;
			obj.initialVelocity = horizontalMove;
		}
		if(sarah)
		{
			GameObject go = Instantiate(bulletPink, new Vector3(m_FacingRight ? transform.position.x + 1f : transform.position.x - 1f, transform.position.y + .4f, transform.position.z), Quaternion.identity) as GameObject;
			Bullet obj = go.GetComponent<Bullet>();
			obj.isFacingLeft = !m_FacingRight;
			obj.initialVelocity = horizontalMove;
		}
		if(aaron)
		{
			GameObject go = Instantiate(bulletOrange, new Vector3(m_FacingRight ? transform.position.x + 1f : transform.position.x - 1f, transform.position.y + 0f, transform.position.z), Quaternion.identity) as GameObject;
			Bullet obj = go.GetComponent<Bullet>();
			obj.isFacingLeft = !m_FacingRight;
			obj.initialVelocity = horizontalMove;
		}
		if(mars)
		{
			GameObject go = Instantiate(bulletYellow, new Vector3(m_FacingRight ? transform.position.x + 1f : transform.position.x - 1f, transform.position.y + -.4f, transform.position.z), Quaternion.identity) as GameObject;
			Bullet obj = go.GetComponent<Bullet>();
			obj.isFacingLeft = !m_FacingRight;
			obj.initialVelocity = horizontalMove;
		}
		lastShotgunTime = Time.time;
	}
}