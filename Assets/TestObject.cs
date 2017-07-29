using UnityEngine;
using CoatsinkTest;
using System;
using Smoothing = CoatsinkTest.InterpolationMethod;

public class TestObject : MonoBehaviour
{
    public enum TestCase { INTERPOLATION_TYPES, STRESS_TEST, CUSTOM }
    public TestCase testCase = TestCase.INTERPOLATION_TYPES;
    public TinyAnimation animCtrl;

    void Start()
    {
        // For the sake of testing the TinyAnimation class create two points and animate back and forth 
        // between these at the same time intervals or random dependent on the chosen test case.
        CoatsinkTest.Keyframe[] frames = null;

        switch (testCase)
        {
            case TestCase.INTERPOLATION_TYPES:
            {
                frames = new CoatsinkTest.Keyframe[]
                {
                    new CoatsinkTest.Keyframe(new Vector3(5f, 2.5f, 0f), 5f, Smoothing.LINEAR),
                    new CoatsinkTest.Keyframe(new Vector3(-5f, -2.5f, 0f), 5f, Smoothing.EASE_IN),
                    new CoatsinkTest.Keyframe(new Vector3(5f, 2.5f, 0f), 5f, Smoothing.EASE_OUT),
                    new CoatsinkTest.Keyframe(new Vector3(-5f, -2.5f, 0f), 5f, Smoothing.EASE_IN_OUT),
                    new CoatsinkTest.Keyframe(new Vector3(5f, 2.5f, 0f), 5f, Smoothing.EASE_IN_OUT)
                };

                break;
            }
            case TestCase.STRESS_TEST:
            {
                int stressLvl = 10000000;
                frames = new CoatsinkTest.Keyframe[stressLvl];
                for (int i = 0; i < stressLvl; ++i)
                {
                        frames[i] = (i % 2 == 0) ?
                               new CoatsinkTest.Keyframe(new Vector3(-5f, -2.5f, 0f), 2, Smoothing.LINEAR) :
                               new CoatsinkTest.Keyframe(new Vector3(5f, 2.5f, 0f), 2, Smoothing.LINEAR);
                }

                break;
            }
            case TestCase.CUSTOM:
            {
                if (animCtrl.Length == 0)
                    throw new ArgumentException("Animation has no keyframes");

                break;
            }
        }

        animCtrl = new TinyAnimation(frames);
    }

    void Update()
    {
        if (animCtrl.Length > 0 && !animCtrl.IsComplete)
            transform.position = animCtrl.SampleAnimation(Time.time);
    }
}
