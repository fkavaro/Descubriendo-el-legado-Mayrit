using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Controller for handling animations via Animator component.
/// </summary>
public class AnimationController
{
    readonly MonoBehaviour _entity;
    readonly IBehaviourEntity _behaviourEntity;
    readonly Animator _animator;

    readonly int _idleAnim = Animator.StringToHash("Idle")
        , _walkAnim = Animator.StringToHash("Walk")
        , _runAnim = Animator.StringToHash("Run")
        , _preJumpAnim = Animator.StringToHash("PreJump")
        , _jumpAnim = Animator.StringToHash("Jump")
        , _afterJumpAnim = Animator.StringToHash("AfterJump")
        ;

    int _currentAnimation, _lastAnimation;

    // Constructor
    public AnimationController(MonoBehaviour entity, IBehaviourEntity behaviourEntity, Animator animator)
    {
        _entity = entity;
        _behaviourEntity = behaviourEntity;
        _animator = animator;
    }

    #region PUBLIC METHODS
    /// <summary>
    /// Crossfade to new animation.
    /// </summary>
    public virtual void ChangeAnimationTo(int newAnimation, float duration = 0.2f)
    {
        // Not same as current
        if (_currentAnimation != newAnimation)
        {
            // Update last animation
            _lastAnimation = _currentAnimation;
            _currentAnimation = newAnimation;
        }

        // Interpolate transition to new animation
        _animator.CrossFade(newAnimation, duration);
    }

    /// <summary>
    /// Crossfade to previous animation.
    /// </summary>
    public virtual void ChangeToPreviousAnimation(float duration = 0.2f)
    {
        ChangeAnimationTo(_lastAnimation, duration);
    }

    /// <returns> True if the current animation is finished, false otherwise.</returns>
    public virtual bool IsCurrentAnimationFinished()
    {
        // Get current animation state info
        AnimatorStateInfo currentStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // If the animation is looping, it's never 'finished'
        if (currentStateInfo.loop)
            return false;

        // For non-looping animations, check if normalizedTime >= 1
        return currentStateInfo.normalizedTime >= 1f;
    }

    public bool IsAnimationFinished(int animation)
    {
        // Get current animation state info
        AnimatorStateInfo currentStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        // Given animation is the current one
        if (currentStateInfo.shortNameHash == animation)
            return IsCurrentAnimationFinished();
        else
            return false;
    }

    public void PlayAnimationCertainTime(float waitTime, int animation, string animationName, Action onComplete = null)
    {
        _entity.StartCoroutine(PlayAnimationCertainTimeCoroutine(waitTime, animation, animationName, onComplete));
    }

    public void PlayAnimationRandomTime(int animation, string animationName, Action onComplete = null)
    {
        int waitTime = UnityEngine.Random.Range(5, 21);
        _entity.StartCoroutine(PlayAnimationCertainTimeCoroutine(waitTime, animation, animationName, onComplete));
    }

    public IEnumerator PlayAnimationCertainTimeCoroutine(float waitTime, int animation, string animationName, Action onComplete = null)
    {
        if (_behaviourEntity.IsExecutionPaused) yield break;
        _behaviourEntity.IsExecutionPaused = true;

        if (waitTime >= 2f)
            ChangeAnimationTo(animation);
        yield return new WaitForSeconds(waitTime);

        _behaviourEntity.IsExecutionPaused = false;
        onComplete?.Invoke();
    }

    public void ChangeToIdle()
    {
        ChangeAnimationTo(_idleAnim);
    }

    public void ChangeToWalk()
    {
        ChangeAnimationTo(_walkAnim);
    }

    public void ChangeToRun()
    {
        ChangeAnimationTo(_runAnim);
    }

    public void ChangeToPreJump()
    {
        ChangeAnimationTo(_preJumpAnim);
    }

    public void ChangeToJump()
    {
        ChangeAnimationTo(_jumpAnim);
    }

    public void ChangeToAfterJump()
    {
        ChangeAnimationTo(_afterJumpAnim);
    }

    public bool IsIdleAnimationFinished()
    {
        return IsAnimationFinished(_idleAnim);
    }

    public bool IsWalkAnimationFinished()
    {
        return IsAnimationFinished(_walkAnim);
    }

    public bool IsRunAnimationFinished()
    {
        return IsAnimationFinished(_runAnim);
    }

    public bool IsPreJumpAnimationFinished()
    {
        return IsAnimationFinished(_preJumpAnim);
    }

    public bool IsJumpAnimationFinished()
    {
        return IsAnimationFinished(_jumpAnim);
    }

    public bool IsAfterJumpAnimationFinished()
    {
        return IsAnimationFinished(_afterJumpAnim);
    }
    #endregion
}
