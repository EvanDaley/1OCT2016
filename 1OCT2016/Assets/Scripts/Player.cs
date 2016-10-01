using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour 
{
	Rigidbody2D rbody;
	private float distToGround;
	public float distanceToGround;
	public LayerMask ground;
	private Renderer render;

	public GameObject explosion;
	private Vector3 startPos;

	public float moveForce = 1.2f;
	public float jumpForce = 200f;
	public int maxJumps = 3;
	private int jumpsLeft = 0;

	public Text jumpText;
	public Text scoreText;
	public Text recordText;

	public int score = 0;
	public int record = 0;

	public float jumpGroundCheckDelay = .2f;
	private float jumpGroundCheckCooldown;

	public float scoreCooldown = 1f;

	private GameObject explosionInstance;

	bool dead = false;

	public float respawnDelay = 1f;

	void Start ()
	{
		rbody = GetComponent<Rigidbody2D> ();
		distToGround = GetComponent<Collider2D> ().bounds.extents.y;
		render = GetComponent<Renderer> ();

		startPos = transform.position;

		record = PlayerPrefs.GetInt ("record", 0);
		recordText.text = record.ToString ();
	}
	
	void Update () 
	{
		if (Time.time > scoreCooldown)
		{
			scoreCooldown = Time.time + 1f;
			score += 1;
			scoreText.text = score.ToString ();
			if (score > record)
			{
				record = score;
				PlayerPrefs.SetInt ("record", record);
				recordText.text = score.ToString ();
			}
		}

		CheckGrounded ();
		rbody.AddForce (Vector3.right * moveForce);

		if (transform.position.y < -6 && dead == false)
			Kill ();
	}

	public void Jump()
	{
		print ("Trying to jump");

		if(jumpsLeft > 0)
		{
			jumpGroundCheckCooldown = Time.time + jumpGroundCheckDelay;
			rbody.AddForce (Vector3.up * jumpForce);
			jumpsLeft -= 1;
			jumpText.text = jumpsLeft.ToString ();
		}
	}

	bool CheckGrounded()
	{
		bool grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - .3f, transform.position.y + .1f), new Vector2(transform.position.x + .3f, transform.position.y - distanceToGround - distToGround), ground); 

		if (grounded && jumpGroundCheckCooldown < Time.time)
		{
			jumpsLeft = maxJumps;
			jumpText.text = jumpsLeft.ToString ();
		}
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

		score = 0;
		scoreText.text = score.ToString ();
	}
}
