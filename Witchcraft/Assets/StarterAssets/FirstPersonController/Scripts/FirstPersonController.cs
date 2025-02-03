﻿using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine.UI;

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		public static FirstPersonController instance;
		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

        public RectTransform hand;
        public float speed = 1f;

        private float stamina;

		public float StaminaRegenRate;
		public float maxStamina;

		public int pointA;
		public int pointB;
		public int pointC;
		public int pointD;

		public Image staminabar;

		public bool isMoving = false;
		bool canRun = true;
		public bool isSprinting = false;

		Rigidbody rb;
	
#if ENABLE_INPUT_SYSTEM
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		public Transform monsterFace;
		public Transform minGamePos;
		private const float _threshold = 0.01f;

		[SerializeField] AudioSource exhausted;
		[SerializeField] AudioSource walking;
		[SerializeField] AudioSource running;

		public GameObject minigame;
		public bool minigameActive;
		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
			minigame.SetActive(false);
			rb = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			instance = this;
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			stamina = maxStamina;



			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		public void ToggleMinigame()
		{
            minigameActive = !minigameActive;
            minigame.SetActive(minigameActive);
		
            GetComponent<StarterAssetsInputs>().SetCursorState(!minigameActive);
            GetComponent<StarterAssetsInputs>().cursorInputForLook = !minigameActive;
            GetComponent<StarterAssetsInputs>().LookInput(new Vector2());
            GetComponent<StarterAssetsInputs>().MoveInput(new Vector2());
            GetComponent<StarterAssetsInputs>().ToggleMovement(!minigameActive);
            minigame.GetComponent<RuneDrawing>().ResetLines();
            Cursor.visible = minigameActive;
        }
		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();

			if (Input.GetKeyDown(KeyCode.Q))
			{

                ToggleMinigame();
			}

            isMoving = rb.velocity.magnitude > 0.1f;


            MovingSTuff();

			if (enemy.instance.jumpscareOMG)
			{
				//freeze camera somewhere
			}
        }

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}
        #region Movement sounds and hand 
        private void MovingSTuff()
        {
			if(isMoving && !isSprinting)
			{
				if (running.isPlaying) running.Stop();
				if (!walking.isPlaying) walking.Play();

				float y = Mathf.PingPong(Time.time * speed, 1) * pointA - pointB;
                hand.anchoredPosition = new Vector2(-376, y);
            }
			else if(isMoving && isSprinting)
			{
				if(walking.isPlaying) walking.Stop();
				if(!running.isPlaying) running.Play();

                float y = Mathf.PingPong(Time.time * speed, 1) * pointC - pointD;
                hand.anchoredPosition = new Vector2(-376, y);
            }
			if(!isMoving || pauseMenu.instance.isPaused)
			{
                if (walking.isPlaying) walking.Stop();
                if (running.isPlaying) running.Stop();
            }
        }
        #endregion
        private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}
        #region StaminaStuff
        IEnumerator Exhaustion()
		{
			canRun = false;

			yield return new WaitForSeconds(2);

			canRun = true;
		}

		IEnumerator SprintCooldown()
		{
			yield return new WaitForSeconds(0.5f);
			while (!isSprinting && stamina < maxStamina)
			{
				stamina += Time.deltaTime * StaminaRegenRate;
				if (stamina > maxStamina)
				{
					stamina = maxStamina;
				}
				staminabar.fillAmount = stamina / maxStamina;
				yield return null;
			}
		}

		private bool IsCoroutineRunning(string coroutineName)
		{
			return StartCoroutine(coroutineName) != null;
		}
        #endregion
        private void Move()
		{

			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint && stamina > 0 ? SprintSpeed : MoveSpeed;
            if (_input.sprint && stamina > 0)
            {
				stamina -= Time.deltaTime;
				isSprinting = true;
				if (stamina <= 0)
                {
					isSprinting = false;
					stamina = 0;
					StartCoroutine(Exhaustion());
					exhausted.Play();
                }
            }
            else
            {
				isSprinting = false;
				if (canRun && !IsCoroutineRunning("SprintCooldown"))
				{
					StartCoroutine(SprintCooldown());
				}
				
            }

            staminabar.fillAmount = 1f / maxStamina * stamina;





			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}