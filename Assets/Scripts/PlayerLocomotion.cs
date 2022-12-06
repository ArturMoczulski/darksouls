using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
	public class PlayerLocomotion : MonoBehaviour
	{
		PlayerStats playerStats;
		PlayerManager playerManager;
		Transform cameraObject;
		InputHandler inputHandler;
		CapsuleCollider capsuleCollider;
        AudioSource audioSource;
        AudioClips audioClips;
		public Vector3 moveDirection;

		[HideInInspector]
		public Transform myTransform;

		[HideInInspector]
		public AnimatorHandler animatorHandler;

		public new Rigidbody rigidbody;

		//public GameObject normalCamera;

		[Header("Ground & Air Detection Stats")]
		[SerializeField]
		float groundDetectionRayStartPoint = 0.5f;
		[SerializeField]
		float minimumDistanceNeededToBeginFall = 1f;
		[SerializeField]
		float groundDirectionRayDistance = 0f;
		LayerMask ignoreForGroundCheck;
		public float inAirTimer;

		[Header("Movement Stats")]
		[SerializeField]
		float movementSpeed = 5;
		[SerializeField]
		float rotationSpeed = 10;
		[SerializeField]
		float sprintSpeed = 7;
		[SerializeField]
		float fallSpeed = 15f;
		[SerializeField]
		float rollSpeed = 1.2f;

		public float GetMovementSpeed()
		{
			return movementSpeed;
        }

        private void Start()
        {
			playerStats	= GetComponent<PlayerStats>();
			playerManager = GetComponent<PlayerManager>();
			cameraObject = Camera.main.transform;
			rigidbody = GetComponent<Rigidbody>();
			inputHandler = GetComponent<InputHandler>();
			animatorHandler = GetComponentInChildren<AnimatorHandler>();
            audioClips = GetComponent<AudioClips>();
            audioSource = GetComponent<AudioSource>();
			capsuleCollider = GetComponent<CapsuleCollider>(); 
			myTransform = transform;
			animatorHandler.Initialize();

			playerManager.isInAir = false;
			ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }

		#region Movement
		Vector3 targetPosition;
		Vector3 normalVector = Vector3.up;

        private void HandleRotation(float delta)
		{
			Vector3 targetDirection = cameraObject.forward * inputHandler.vertical;
			targetDirection += cameraObject.right * inputHandler.horizontal;

			targetDirection.Normalize();
			targetDirection.y = 0;

			if (targetDirection == Vector3.zero)
				targetDirection = myTransform.forward;

			Quaternion targetRotation =
				Quaternion.Slerp(
					Quaternion.LookRotation(targetDirection),
					myTransform.rotation,
					rotationSpeed * delta
					);

			myTransform.rotation = targetRotation;
        }

		public void HandleMovement(float delta)
		{
			if (inputHandler.rollFlag)
				return;

			if (playerManager.isInteracting)
				return;

			if (playerManager.isInAir)
				return;

			moveDirection = cameraObject.forward * inputHandler.vertical;
			moveDirection += cameraObject.right * inputHandler.horizontal;
			moveDirection.Normalize();
			moveDirection.y = 0;

			float speed = movementSpeed;
			float moveAmount = inputHandler.moveAmount;

			if (inputHandler.sprintFlag && CanSprint())
			{
				speed = sprintSpeed;
				playerManager.isSprinting = true;
            }
			else
			{ 
				playerManager.isSprinting = false;
            }

			if (moveAmount > 0 && !CanRun())
			{
				if (speed > movementSpeed / 3)
				{
					speed = movementSpeed / 3;
					moveAmount = 0.33f;
				}
			}

			moveDirection *= speed;

			Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);

			rigidbody.velocity = projectedVelocity;

			animatorHandler.UpdateAnimatorValues(moveAmount, 0, playerManager.isSprinting);

			if (inputHandler.moveAmount > 0 && !audioSource.isPlaying)
			{
                audioSource.clip = audioClips.footsteps001;
                audioSource.Play();
			}

			if (inputHandler.moveAmount == 0 && audioSource.clip == audioClips.footsteps001)
			{
				audioSource.Stop();
			}

			if (animatorHandler.canRotate) { 
				HandleRotation(delta);
            }
        }

		public bool CanSprint()
		{
			return playerStats.currentStamina > playerStats.maxStamina / 2;
        }
		public bool CanRun()
		{
			return playerStats.currentStamina > playerStats.maxStamina / 3;
        }

		public void HandleRollingAndSprinting(float delta)
		{ 

			if (animatorHandler.animator.GetBool("isInteracting"))
			{
				return;
            }

			if (inputHandler.rollFlag)
			{
				moveDirection = cameraObject.forward * inputHandler.vertical;
				moveDirection += cameraObject.right * inputHandler.horizontal;

				if (inputHandler.moveAmount > 0)
				{ 
					animatorHandler.PlayTargetAnimation("Paladin_RunToRoll_001", true);
					rigidbody.velocity *= rollSpeed;
					moveDirection.y = 0;

					Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                }
				//else
				//{ 
				//	animatorHandler.PlayTargetAnimation("Paladin_Backstep_002_NotInPlace", true);
    //            }
			}
        }

		public void HandleFalling(float delta, Vector3 moveDirection)
		{
			playerManager.isGrounded = false;
			RaycastHit hit;
			Vector3 origin = myTransform.position;
			origin.y += groundDetectionRayStartPoint;

			// stop changing position if the character
			// hits something in front making it a straight
			// downwards fall
			if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
			{
				moveDirection = Vector3.zero;
			}

			// apply freefall forces
			if (playerManager.isInAir)
			{
				rigidbody.AddForce(Vector3.down * fallSpeed * 25);
			}

			origin += moveDirection.normalized * groundDirectionRayDistance;
			targetPosition = myTransform.position;

			Debug.DrawRay(origin, Vector3.down * minimumDistanceNeededToBeginFall, Color.red, 0.01f, false);

			// Handle Animations and State

			// there is a surface under the character's legs
			if (Physics.Raycast(origin, Vector3.down, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
			{
				//Debug.Log("Surface detected under the character");

				// update state to indicate the character is
				// now on the ground
				playerManager.isGrounded = true;
				normalVector = hit.normal;
				targetPosition.y = hit.point.y;

				if (playerManager.isInAir)
				{
					// but it did just fall
					Debug.Log("The character did just fall");

					// stop the character when it hits the ground
					rigidbody.velocity = Vector3.zero;
					rigidbody.AddForce(moveDirection * fallSpeed / 10f);

					if (inAirTimer > 0.5f)
					{
						Debug.Log("From high enough to play the landing animation");
						// play landing animation
						Debug.Log("You were in the air for " + inAirTimer);
                        animatorHandler.PlayTargetAnimation("Paladin_Fall_Landing_001", true);
                        inAirTimer = 0;
					}
					else
					{
						Debug.Log("From not high enough to play the falling animation");
						// small fall so no need for the landing animtion
						animatorHandler.PlayTargetAnimation("Empty", false);
						inAirTimer = 0;
					}

					// update player state to indicate that the
					// character is now on the ground
					playerManager.isInAir = false;
				}

			}
			// there is no surface under the character's legs
			else
			{ 
				//Debug.Log("There is no surface under the character's legs");

				// if the character has been on the ground
				if (playerManager.isGrounded)
				{
					Debug.Log("But the character was grounded previously so changing state to not grounded");
					// update the character sate to reflect
					// being on the ground
					playerManager.isGrounded = false;
                }

				// the character has not been in the air proviously
				if (playerManager.isInAir == false)
				{
					Debug.Log("And the character was not in the air before so play falling animation and update the isInAir state");

					if (playerManager.isInteracting == false)
					{
						// play the falling animation
						Debug.Log("Play falling animation");
						animatorHandler.PlayTargetAnimation("Paladin_Fall_Idle_001", true);
						animatorHandler.UpdateAnimatorValues(0, 0, false);
					}

					// add a bit of a forward push when falling so the collider
					// doesn't get caught on the edge
					rigidbody.AddForce(transform.forward * capsuleCollider.radius * 1000);

					playerManager.isInAir = true;
                }
            }

			// move the character
			myTransform.position = targetPosition;
        }
        #endregion
    }

}