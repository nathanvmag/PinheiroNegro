﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	FieldOfVision field;
	private Camera cam;
	private Transform my;
	private Rigidbody2D body;
	public Sprite[] lados;
	private float Speed;

	SpriteRenderer sp;

	private float activeZoom;
	private bool zoomOut = true;

	[HideInInspector] public float stamina, staminaconta ;
	public GameObject staminabar;

	void Awake ()
	{
		cam = Camera.main;
		my = GetComponent <Transform> ();
		body = GetComponent <Rigidbody2D> ();
	}

	void Start()
	{
		sp = GetComponent<SpriteRenderer> ();
		stamina = 100; 
		field = GameObject.Find ("Enemy").GetComponentInChildren<FieldOfVision> ();
		Speed = 15;
	}

	void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.Space) && zoomOut && stamina > 0)
		{
			Speed = 30;
			activeZoom = Time.time;
			zoomOut = false;
		}
		else if(!Input.GetKey(KeyCode.Space) && !zoomOut)
		{
			Speed = 15;
			activeZoom = Time.time;
			zoomOut = true;
		}
	}
	
	void Update ()
	{
		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);

		if (stamina <= 0)
		{
			zoomOut = true;
			Speed = 15; 		
		}

		staminaconta = (stamina/100f) * 2.45f;
		staminabar.transform.localScale = new Vector3(staminaconta,staminabar.transform.localScale.y,staminabar.transform.localScale.z);

		if (field.saw)
			GetComponent<SpriteRenderer> ().color = Color.cyan;
		else
			GetComponent<SpriteRenderer> ().color = Color.white;

		body.velocity = new Vector3 (0, 0, 0);

		if (Input.GetKey ("up"))
		{
			body.velocity = Vector3.up * Speed;
		}
		if (Input.GetKey ("left"))
		{
			sp.sprite = lados[2];
			body.velocity = Vector3.left * Speed;
		}
		if (Input.GetKey ("down"))
		{
			sp.sprite = lados[0];
			body.velocity = Vector3.down * Speed;
		}
		if (Input.GetKey ("right"))
		{
			sp.sprite = lados[1];
			body.velocity = Vector3.right * Speed;
		}
		if (Input.GetKey ("up") && Input.GetKey ("left"))
		{
			body.velocity = new Vector3(-1,1,0) * Speed;
		}
		if (Input.GetKey ("up") && Input.GetKey ("right"))
		{
			body.velocity = new Vector3(1,1,0) * Speed;
		}
		if (Input.GetKey ("down") && Input.GetKey ("left"))
		{
			body.velocity = new Vector3(-1,-1,0) * Speed;
		}
		if (Input.GetKey ("down") && Input.GetKey ("right"))
		{
			body.velocity = new Vector3(1,-1,0) * Speed; 
		}

		if (zoomOut)
		{
			Camera.main.orthographicSize = Mathf.Lerp (7, 12, 5f * (Time.time - activeZoom));
			if (stamina < 100 && !Input.GetKey(KeyCode.Space))
				stamina +=10 * Time.deltaTime;
		} 
		else
		{
			Camera.main.orthographicSize = Mathf.Lerp (12, 7, 5f * (Time.time - activeZoom));
			if (stamina > 0)
				stamina -= 10 * Time.deltaTime;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.name.Equals ("Enemy"))
		{
			Application.LoadLevel(5);
		}
		if (other.gameObject.tag == "goldenHouse")
		{
			Application.LoadLevel(4);
		}
	}
}
