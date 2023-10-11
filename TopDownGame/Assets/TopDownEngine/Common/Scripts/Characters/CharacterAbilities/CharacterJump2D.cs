using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

namespace MoreMountains.TopDownEngine
{
	/// <summary>
	/// Add this ability to a character and it'll be able to jump in 2D. This means no actual movement, only the collider turned off and on. Movement will be handled by the animation itself.
	/// </summary>
	[AddComponentMenu("TopDown Engine/Character/Abilities/Character Jump 2D")]
	public class CharacterJump2D : CharacterAbility 
	{
		/// the duration of the jump
		[Tooltip("the duration of the jump")]
		public float JumpDuration = 1f;
		/// whether or not jump should be proportional to press (if yes, releasing the button will stop the jump)
		[Tooltip("whether or not jump should be proportional to press (if yes, releasing the button will stop the jump)")]
		public bool JumpProportionalToPress = true;
		/// the minimum amount of time after the jump starts before releasing the jump has any effect
		[Tooltip("the minimum amount of time after the jump starts before releasing the jump has any effect")]
		public float MinimumPressTime = 0.4f;
		/// the feedback to play when the jump starts
		[Tooltip("the feedback to play when the jump starts")]
		public MMFeedbacks JumpStartFeedback;
		/// the feedback to play when the jump stops
		[Tooltip("the feedback to play when the jump stops")]
		public MMFeedbacks JumpStopFeedback;

		protected CharacterButtonActivation _characterButtonActivation;
		protected bool _jumpStopped = false;
		protected float _jumpStartedAt = 0f;
		protected bool _buttonReleased = false;
		protected const string _jumpingAnimationParameterName = "Jumping";
		protected const string _hitTheGroundAnimationParameterName = "HitTheGround";
		protected int _jumpingAnimationParameter;
		protected int _hitTheGroundAnimationParameter;

				//내가 선언
		Rigidbody2D rigid;
		Vector3 dirVec;
		GameObject scanObject;
		public float Speed;
		float h;
		float v;
		bool isHorizonMove;
		/// <summary>
		/// On init we grab our components
		/// </summary>
		protected override void Initialization()
		{
			base.Initialization ();
			_characterButtonActivation = _character?.FindAbility<CharacterButtonActivation> ();
			JumpStartFeedback?.Initialization(this.gameObject);
			JumpStopFeedback?.Initialization(this.gameObject);
		}

		/// <summary>
		/// On HandleInput we watch for jump input and trigger a jump if needed
		/// </summary>
		protected override void HandleInput()
		{
			        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
        bool hUp = Input.GetButtonUp("Horizontal");
        bool vUp= Input.GetButtonUp("Vertical");
			base.HandleInput();
			// if movement is prevented, or if the character is dead/frozen/can't move, we exit and do nothing
			if (!AbilityAuthorized
			    || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal))
			{
				return;
			}
			if (_inputManager.JumpButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
			{
				JumpStart();
			}
			if (_inputManager.JumpButton.State.CurrentState == MMInput.ButtonStates.ButtonUp)
			{
				_buttonReleased = true;
			}
			   if (hDown)
        {
            isHorizonMove = true;
            if(h == 1)
            {
                dirVec = Vector3.right;
            }
            else if(h == -1)
            {
                dirVec = Vector3.left;
            }
        }
        else if (vDown)
        {
            isHorizonMove = false;
            if (v == 1)
            {
                dirVec = Vector3.up;
            }
            else if (v == -1)
            {
                dirVec = Vector3.down;
            }
        }
        else if(hUp || vUp) 
        {
            isHorizonMove = h != 0;
            if(!(h == 0 && v == 0))
            {
                dirVec = isHorizonMove ? new Vector3(h, 0, 0) : new Vector3(0, v, 0);
            }
        }
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            Debug.Log("안녕? 나는 "+ scanObject.name + "야.");
        }
		}
void FixedUpdate()
{
        //Ray
        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object")); 
        if(rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
}

		/// <summary>
		/// On process ability, we stop the jump if needed
		/// </summary>
		public override void ProcessAbility()
		{
			if (_movement.CurrentState == CharacterStates.MovementStates.Jumping)
			{
				if (!_jumpStopped)
				{
					if (Time.time - _jumpStartedAt >= JumpDuration)
					{
						JumpStop();
					}
					else
					{
						_movement.ChangeState(CharacterStates.MovementStates.Jumping);
					}
				}
				if (_buttonReleased
				    && !_jumpStopped
				    && JumpProportionalToPress
				    && (Time.time - _jumpStartedAt > MinimumPressTime))
				{
					JumpStop();
				}
			}
		}

		/// <summary>
		/// Starts a jump
		/// </summary>
		public virtual void JumpStart()
		{
			if (!EvaluateJumpConditions())
			{
				return;
			}
			_movement.ChangeState(CharacterStates.MovementStates.Jumping);	
			MMCharacterEvent.Trigger(_character, MMCharacterEventTypes.Jump);
			JumpStartFeedback?.PlayFeedbacks(this.transform.position);
			PlayAbilityStartFeedbacks();

			_jumpStopped = false;
			_jumpStartedAt = Time.time;
			_buttonReleased = false;
		}

		/// <summary>
		/// Stops the jump
		/// </summary>
		public virtual void JumpStop()
		{
			_jumpStopped = true;
			_movement.ChangeState(CharacterStates.MovementStates.Idle);
			_buttonReleased = false;
			JumpStopFeedback?.PlayFeedbacks(this.transform.position);
			StopStartFeedbacks();
			PlayAbilityStopFeedbacks();
		}

		/// <summary>
		/// Returns true if jump conditions are met
		/// </summary>
		/// <returns></returns>
		protected virtual bool EvaluateJumpConditions()
		{
			if (!AbilityAuthorized)
			{
				return false;
			}
			if (_characterButtonActivation != null)
			{
				if (_characterButtonActivation.AbilityAuthorized
				    && _characterButtonActivation.InButtonActivatedZone)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Adds required animator parameters to the animator parameters list if they exist
		/// </summary>
		protected override void InitializeAnimatorParameters()
		{
			RegisterAnimatorParameter (_jumpingAnimationParameterName, AnimatorControllerParameterType.Bool, out _jumpingAnimationParameter);
			RegisterAnimatorParameter (_hitTheGroundAnimationParameterName, AnimatorControllerParameterType.Bool, out _hitTheGroundAnimationParameter);
		}

		/// <summary>
		/// At the end of each cycle, sends Jumping states to the Character's animator
		/// </summary>
		public override void UpdateAnimator()
		{
			MMAnimatorExtensions.UpdateAnimatorBool(_animator, _jumpingAnimationParameter, (_movement.CurrentState == CharacterStates.MovementStates.Jumping),_character._animatorParameters, _character.RunAnimatorSanityChecks);
			MMAnimatorExtensions.UpdateAnimatorBool (_animator, _hitTheGroundAnimationParameter, _controller.JustGotGrounded, _character._animatorParameters, _character.RunAnimatorSanityChecks);
		}
	}
}