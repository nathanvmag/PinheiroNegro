﻿using UnityEngine;
using System.Collections;

public class Enemy1 : MonoBehaviour
{
	private FieldOfVision field;
	private PauseGame isPaused;
	private GameObject tutorial;
	private Transform my;
	private Rigidbody2D body;
	private GameObject player;

	public Transform[] places2Walk;
	private bool goTo2, goTo3, goTo4, goTo5;
	private bool[] goTo = new bool[4];

	private float timer;

	private PolyNavAgent pagent;
	private int rand;
	private bool arrived;
	public Vector3[] Places;
	public GameObject[]temp;
	public static bool canBeath;

	void Awake()
	{
		field = GetComponentInChildren<FieldOfVision> ();
	}

	void Start ()
	{
		canBeath = true;
		temp = GameObject.FindGameObjectsWithTag ("MovE1");
		Places =  new Vector3[temp.Length]; 
		for (int i = 0; i < temp.Length; i++)
		{
			Places[i] = temp[i].transform.position;
		}

		pagent = GetComponent<PolyNavAgent> ();
		rand = Random.Range (0, Places.Length);
		arrived = false; 
		tutorial = GameObject.Find ("Tutorial");
		isPaused = GameObject.Find ("GameManager").GetComponent<PauseGame> ();
		player = GameObject.FindGameObjectWithTag ("Player");

		my = GetComponent <Transform> ();
		body = GetComponent <Rigidbody2D> ();

		places2Walk = GetComponentsInChildren<Transform> ();
		for (int i = 0; i < goTo.Length; i++)
		{
			goTo[i] = false; 
		}

		transform.DetachChildren ();
		places2Walk[1].gameObject.transform.SetParent (transform);
		transform.position = Places [Random.Range (0, Places.Length)];
		timer = 0;
	}

	void Update ()
	{
		if (!isPaused.paused && tutorial == null)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, 0);
			WalkAndRun();
			for (int i=0; i<temp.Length;i++)
			{
				Destroy(temp[i].gameObject);
			}
		}
	}

	void WalkAndRun()
	{
		Vector2 posiplayer = player.transform.position;
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		if (player.GetComponent<player> ().medo >= 100)
			pagent.SetDestination (posiplayer);
		if (field.saw)
		{
			pagent.rotateTransform = false; 
			timer += Time.deltaTime;
			float AngleRad = Mathf.Atan2 (-posiplayer.x + my.position.x, posiplayer.y - my.position.y);
			float angle = (180 / Mathf.PI) * AngleRad;
			body.rotation = angle;

			if (timer > 1)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y, 0);
				pagent.SetDestination(posiplayer);
				pagent.maxSpeed = 25;
			}
		}
		
		if (!field.saw)
		{
			pagent.rotateTransform = true;
			pagent.maxSpeed = 10; 
			if (!arrived)
			{
				pagent.SetDestination (Places [rand]);
				if (pagent.remainingDistance <= 0.45f)
					arrived = true; 
			}
			else
			{ 
				rand = Random.Range (0, Places.Length);
				arrived = false; 
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.transform.parent == null && other.tag.Equals("Player"))
		{
			Application.LoadLevel(3);
		}
		if (field.saw)
		{
			if (other.gameObject.tag == "camLimit")
			{
				field.saw = false;
				rand = Random.Range (0,Places.Length);
				arrived = false; 
				timer = 0;
			}
		}
	}
}
