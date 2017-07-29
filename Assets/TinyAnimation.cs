using UnityEngine;
using System;

namespace CoatsinkTest
{
    public enum InterpolationMethod { LINEAR, EASE_IN, EASE_OUT, EASE_IN_OUT }

    [Serializable]
    public struct Keyframe
    {
        public Vector3 position;
        public float time;
        public InterpolationMethod method;

        public Keyframe(Vector3 position, float time, InterpolationMethod method)
        {
            this.position = position;
            this.time = time;
            this.method = method;
        }
    }

    [Serializable]
    public class TinyAnimation
    {
        // Public keyframes for quick access from the inspector
        public Keyframe[] keyFrames;

        private int _currentFrameIndex = 0;
        private float _elasped = 0f;

        public Keyframe this[int index] { get { return keyFrames[index]; } }
        public int Length { get { return keyFrames.Length; } }
        public bool IsComplete { get { return _currentFrameIndex == Length - 1; } }
        public TinyAnimation(params Keyframe[] keyFrames)
        {
            this.keyFrames = keyFrames;
        }

        public Vector3 SampleAnimation(float t)
        {
            if (IsComplete)
            {
                Debug.Log("Animation has finished");
                return this[_currentFrameIndex].position;
            }

            // Normalise t between 0-1 and ensure there's no division by 0
            float frameDuration = this[_currentFrameIndex].time;
            t = (frameDuration != 0) ? Mathf.Clamp((t - _elasped) / (frameDuration), 0f, 1f) : 1;
            if (t == 1)
            {
                _elasped += frameDuration;
                ++_currentFrameIndex;
                return this[_currentFrameIndex].position;
            }

            Keyframe currentFrame = this[_currentFrameIndex];
            Keyframe nextFrame = this[_currentFrameIndex + 1];
            switch (currentFrame.method)
            {
                case InterpolationMethod.LINEAR:
                    break;
                case InterpolationMethod.EASE_IN:
                    t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;
                case InterpolationMethod.EASE_OUT:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f); 
                    break;
                case InterpolationMethod.EASE_IN_OUT:
                    t = Mathf.Pow(t, 2) * (3f - 2f * t);
                    break;
            }

            return Lerp(currentFrame.position, nextFrame.position, t);
        }
    

        private Vector3 Lerp(Vector3 v1, Vector3 v2, float t)
        {
            return (1f - t) * v1 + (t * v2);
        }
    }
}