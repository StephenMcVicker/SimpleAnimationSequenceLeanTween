using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel; // LeanTween

namespace SimpleAnimation
{

  [AddComponentMenu("Animations/Simple Sequence")]
  public class SimpleAnimationSequence : MonoBehaviour
  {

    // * *
    [Header("Move Animations")]
    // Move Animation Variables
    public List<SimpleAnimationMove> moveAnimations = new List<SimpleAnimationMove>();
    public bool loopMoveAnimations = false;
    public bool doMoveOnEnable = false;
    private Vector3 previousMoveValue;
    private int loopCountMove;
    private float delayBeforeLoopMove = 0;
    // * *

    // * *
    // Scale Animation Variables
    [Space]
    [Header("Scale Animations")]
    public List<SimpleAnimationScale> scaleAnimations = new List<SimpleAnimationScale>();
    public bool loopScaleAnimations = false;
    public bool doScaleOnEnable = false;
    private Vector3 previousScaleValue;
    private int loopCountScale;
    private float delayBeforeLoopScale = 0;
    // * *

    // * *
    // Rotate Animation Variables
    [Space]
    [Header("Rotate Animations")]
    public List<SimpleAnimationRotate> rotateAnimations = new List<SimpleAnimationRotate>();
    public bool loopRotateAnimations = false;
    public bool doRotateOnEnable = false;
    private Vector3 previousRotateValue;
    private int loopCountRotate;
    private float delayBeforeLoopRotate = 0;
    // * *

    void OnEnable()
    {
      // Move Animations - check if start on enable
      if (doMoveOnEnable == true)
      {
        PlayMoveAnimations();
      }
      // Scale Animations - check if start on enable
      if (doScaleOnEnable == true)
      {
        PlayScaleAnimations();
      }
      // Rotate Animations - check if start on enable
      if (doScaleOnEnable == true)
      {
        PlayRotateAnimations();
      }
    }


    public void PlayAllAnimationsNow()
    {
      // Call this from another script to play all animations
      PlayMoveAnimations();
      PlayScaleAnimations();
      PlayRotateAnimations();
    }

    // * * * * * * * * * *
    #region Move Animation

    public void PlayMoveAnimations()
    {
      var seq = LeanTween.sequence();
      delayBeforeLoopMove = 0;

      // Loop through the animations
      for (int i = 0; i < moveAnimations.Capacity; i++)
      {

        // Get reference of the current animation in the loop
        SimpleAnimationMove animation = moveAnimations[i];

        // Set the previous value to current position of Gameobject if count and loop = 0
        if (i == 0 && loopCountMove == 0)
        {
          previousMoveValue = animation.returnCorrectPositionForWorldOrLocal();
        }

        // Add the position to the previous Move value if MoveBy
        if (animation.moveType == SimpleAnimationMove.MoveType.MoveBy)
        {
          previousMoveValue += animation.moveValue;
        }
        else
        {
          previousMoveValue = animation.moveValue;
        }

        // Append in the start delay to the sequence
        seq.append(animation.startDelay);

        // Append the animation to the sequence
        if (animation.useLocalPosition == true)
        {
          // local space
          seq.append(LeanTween.moveLocal(animation.objToAnimate, previousMoveValue, animation.dur).setEase(animation.curve));
        }
        else
        {
          // World space
          seq.append(LeanTween.move(animation.objToAnimate, previousMoveValue, animation.dur).setEase(animation.curve));
        }

        // Add to the delay before the loop
        delayBeforeLoopMove += animation.startDelay + animation.dur;

        // Check to loop animations
        if (i == moveAnimations.Count - 1 && loopMoveAnimations == true)
        {
          StartCoroutine(RestartAnimationsForLoopMove());
        }

      }

    }

    private IEnumerator RestartAnimationsForLoopMove()
    {
      yield return new WaitForSeconds(delayBeforeLoopMove);

      loopCountMove++;
      delayBeforeLoopMove = 0; // reset the delay - will be processed in next loop
      PlayMoveAnimations();

    }

    #endregion
    // * * * * * * * * * *

    // * * * * * * * * * *
    #region Scale Animation

    public void PlayScaleAnimations()
    {
      var seq = LeanTween.sequence();
      delayBeforeLoopScale = 0;

      // Loop through the animations
      for (int i = 0; i < scaleAnimations.Capacity; i++)
      {

        // Get reference of the current animation in the loop
        SimpleAnimationScale animation = scaleAnimations[i];

        // Set the previous value to current position of Gameobject if count and loop = 0
        if (i == 0 && loopCountScale == 0)
        {
          previousScaleValue = animation.objToAnimate.transform.localScale;
        }

        // Add the position to the previous Move value if MoveBy
        if (animation.scaleType == SimpleAnimationScale.ScaleType.ScaleBy)
        {
          previousScaleValue += animation.scaleValue;
        }
        else
        {
          previousScaleValue = animation.scaleValue;
        }

        // Append in the start delay to the sequence
        seq.append(animation.startDelay);

        // Append the animation to the sequence
        seq.append(LeanTween.scale(animation.objToAnimate, previousScaleValue, animation.dur).setEase(animation.curve));

        // Add to the delay before the loop
        delayBeforeLoopScale += animation.startDelay + animation.dur;

        // Check to loop animations
        if (i == scaleAnimations.Count - 1 && loopScaleAnimations == true)
        {
          StartCoroutine(RestartAnimationsForLoopScale());
        }

      }

    }

    private IEnumerator RestartAnimationsForLoopScale()
    {
      yield return new WaitForSeconds(delayBeforeLoopScale);

      loopCountScale++;
      delayBeforeLoopScale = 0; // reset the delay - will be processed in next loop
      PlayScaleAnimations();

    }

    #endregion
    // * * * * * * * * * *

    // * * * * * * * * * *
    #region Rotate Animation

    public void PlayRotateAnimations()
    {
      var seq = LeanTween.sequence();
      delayBeforeLoopRotate = 0;

      // Loop through the animations
      for (int i = 0; i < rotateAnimations.Capacity; i++)
      {

        // Get reference of the current animation in the loop
        SimpleAnimationRotate animation = rotateAnimations[i];

        // Set the previous value to current position of Gameobject if count and loop = 0
        if (i == 0 && loopCountRotate == 0)
        {
          previousRotateValue = animation.objToAnimate.transform.localRotation.eulerAngles;
        }

        // Add the position to the previous Move value if MoveBy
        if (animation.rotateType == SimpleAnimationRotate.RotateType.RotateBy)
        {
          previousRotateValue += animation.rotateValue;
        }
        else
        {
          previousRotateValue = animation.rotateValue;
        }

        // Append in the start delay to the sequence
        seq.append(animation.startDelay);

        // Append the animation to the sequence
        seq.append(LeanTween.rotate(animation.objToAnimate, previousRotateValue, animation.dur).setEase(animation.curve));

        // Add to the delay before the loop
        delayBeforeLoopRotate += animation.startDelay + animation.dur;

        // Check to loop animations
        if (i == rotateAnimations.Count - 1 && loopRotateAnimations == true)
        {
          StartCoroutine(RestartAnimationsForLoopRotate());
        }

      }

    }

    private IEnumerator RestartAnimationsForLoopRotate()
    {
      yield return new WaitForSeconds(delayBeforeLoopRotate);

      loopCountRotate++;
      delayBeforeLoopRotate = 0; // reset the delay - will be processed in next loop
      PlayRotateAnimations();

    }

    #endregion
    // * * * * * * * * * *

  }

  [System.Serializable]

  public class SimpleAnimationMove
  {

    public string name;
    public GameObject objToAnimate;
    public Vector3 moveValue;
    public enum MoveType
    {
      MoveTo,
      MoveBy
    }
    public MoveType moveType = MoveType.MoveTo;
    public AnimationCurve curve;
    public bool useLocalPosition = false;
    public float dur = 0;
    public float startDelay = 0;

    public Vector3 returnCorrectPositionForWorldOrLocal()
    {
      Vector3 v = Vector3.zero;
      if (useLocalPosition == true)
      {
        v = objToAnimate.transform.localPosition;
      }
      else
      {
        v = objToAnimate.transform.position;
      }
      return v;
    }

  }

  [System.Serializable]

  public class SimpleAnimationScale
  {

    public string name;
    public GameObject objToAnimate;
    public Vector3 scaleValue;
    public enum ScaleType
    {
      ScaleTo,
      ScaleBy
    }
    public ScaleType scaleType = ScaleType.ScaleTo;
    public AnimationCurve curve;
    public float dur = 0;
    public float startDelay = 0;

  }

  [System.Serializable]

  public class SimpleAnimationRotate
  {

    public string name;
    public GameObject objToAnimate;
    public Vector3 rotateValue;
    public enum RotateType
    {
      RotateTo,
      RotateBy
    }
    public RotateType rotateType = RotateType.RotateTo;
    public AnimationCurve curve;
    public float dur = 0;
    public float startDelay = 0;

  }

}