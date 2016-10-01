using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour 
{
	Rigidbody2D rbody;
	private float distToGround;
	public LayerMask ground;
	private Renderer render;

	public GameObject explosion;
	private Vector3 startPos;

	public float moveForce = 1.2f;
	public float jumpForce = 200f;
	public int maxJumps = 3;
	private int jumpsLeft = 0;

	private GameObject explosionInstance;

	bool dead = false;

	public float respawnDelay = 1f;

	void Start ()
	{
		rbody = GetComponent<Rigidbody2D> ();
		distToGround = GetComponent<Collider2D> ().bounds.extents.y;
		render = GetComponent<Renderer> ();

		startPos = transform.position;
	}
	
	void Update () 
	{
		rbody.AddForce (Vector3.right * moveForce);

		if (transform.position.y < -6 && dead == false)
			Kill ();
	}

	public void Jump()
	{
		print ("Trying to jump");
		CheckGrounded ();

		if(jumpsLeft > 0)
		{
			rbody.AddForce (Vector3.up * jumpForce);
			jumpsLeft -= 1;
		}
	}

	bool CheckGrounded()
	{
		bool grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - .3f, transform.position.y + .1f), new Vector2(transform.position.x + .3f, transform.position.y - .3f - distToGround), ground); 

		if (grounded)
			jumpsLeft = maxJumps;
		
		return grounded;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == 9 && dead == false)
		{
			if(dead = false)
				Kill ();
		}

		if (collision.gameObject.layer == 10)
		{
			SceneManager.LoadScene (2);
		}
	}

	void Kill()
	{
		dead = true;
		render.enabled = false;
		explosionInstance = GameObject.Instantiate (explosion, transform.position, transform.rotation) as GameObject;
		Invoke ("Respawn", respawnDelay);
		rbody.velocity = Vector3.zero;
	}

	void Respawn()
	{
		rbody.velocity = Vector3.zero;

		dead = false;
		render.enabled = true;
		transform.position = startPos;
		Destroy (explosionInstance);
	}
}
