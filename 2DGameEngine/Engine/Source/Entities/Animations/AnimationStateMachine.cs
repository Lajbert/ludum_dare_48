﻿using MonolithEngine.Engine.Source.Components;
using MonolithEngine.Engine.Source.Interfaces;
using MonolithEngine.Source.Entities.Animation;
using MonolithEngine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MonolithEngine.Engine.Source.Entities.Animations
{
    public class AnimationStateMachine : IComponent, IUpdatableComponent, IDrawableComponent
    {
        private List<StateAnimation> animations;

        private HashSet<(string, string)> transitions = new HashSet<(string, string)>();

        private StateAnimation currentAnimation = null;

        private Vector2 offset = Vector2.Zero;

        private StateAnimation animationOverride = null;

        private int? transitionFrame = null;

        public Vector2 Offset {
            get => offset;
            set {
                offset = value;
                foreach (StateAnimation anim in animations) {
                    anim.animation.Offset = offset;
                }
            } }

        public bool UniquePerEntity { get; set; }

        public AnimationStateMachine()
        {
            animations = new List<StateAnimation>();
            UniquePerEntity = true;
        }

        public void RegisterAnimation(string state, AbstractAnimation animation, Func<bool> function = null, int priority = 0)
        {
            if (function == null)
            {
                function = () => (true);
            }
            animation.Offset = Offset;
            StateAnimation anim = new StateAnimation(state, animation, function, priority);
            animations.Add(anim);
            animations.Sort((a, b) => a.priority.CompareTo(b.priority) * -1);
        }

        public void PlayAnimation(string state)
        {
            foreach (StateAnimation anim in animations)
            {
                if (anim.state.Equals(state))
                {
                    animationOverride = anim;
                    animationOverride.animation.Init();
                    return;
                }
            }
            throw new Exception("Requested animation not found");
        }

        private class StateAnimation
        {

            public string state;
            public Func<bool> function;
            public AbstractAnimation animation;
            public int priority;
            public bool played;

            public StateAnimation(string state, AbstractAnimation animation, Func<bool> function = null, int priority = 0)
            {
                this.state = state;
                this.animation = animation;
                this.priority = priority;
                this.function = function;
                played = false;
            }

            public override bool Equals(object obj)
            {
                return obj is StateAnimation animation &&
                       state == animation.state &&
                       EqualityComparer<Func<bool>>.Default.Equals(function, animation.function) &&
                       EqualityComparer<AbstractAnimation>.Default.Equals(this.animation, animation.animation) &&
                       priority == animation.priority;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(state, function, animation, priority);
            }
        }

        public bool HasAnimation(string state)
        {
            foreach (StateAnimation anim in animations)
            {
                if (anim.state.Equals(state))
                {
                    return true;
                }
            }
            return false;
        }

        public AbstractAnimation GetAnimation(string state)
        {
            foreach (StateAnimation anim in animations)
            {
                if (anim.state.Equals(state))
                {
                    return anim.animation;
                }
            }
            return null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (animations.Count == 0)
            {
                return;
            }

            Play(spriteBatch);
        }

        private void Play(SpriteBatch spriteBatch)
        {
            transitionFrame = null;
            if (animationOverride != null && animationOverride.animation.Finished())
            {
                animationOverride = null;
            }
            StateAnimation nextAnimation = Pop();
            if (nextAnimation == null)
            {
                return;
            }
            if (nextAnimation != currentAnimation)
            {
                if (currentAnimation != null)
                {
                    if (transitions.Contains((currentAnimation.state, nextAnimation.state))) {
                        transitionFrame = currentAnimation.animation.GetCurrentFrame();
                    }
                    currentAnimation.animation.Stop();
                    currentAnimation.animation.InvokeStoppedCallback();
                    currentAnimation.animation.AnimationSwitchCallback?.Invoke();
                }
                currentAnimation = nextAnimation;
                currentAnimation.animation.Init(transitionFrame);
                currentAnimation?.animation.InvokeStartedCallback();
            }
            currentAnimation.animation.Play(spriteBatch);
        }

        public void AddFrameTransition(string anim1, string anim2)
        {
            transitions.Add((anim1, anim2));
            transitions.Add((anim2, anim1));
        }

        public void Update()
        {
            if (animations.Count == 0 || currentAnimation == null)
            {
                return;
            }

            currentAnimation.animation.Update();
        }

        private StateAnimation Pop()
        {
            if (animationOverride != null)
            {
                return animationOverride;
            }
            foreach (StateAnimation anim in animations)
            {
                if (anim.function.Invoke())
                {
                    return anim;
                }
            }
            return null;
        }

        public void Destroy()
        {
            foreach (StateAnimation anim in animations)
            {
                anim.animation.Destroy();
            }
        }

        public string GetCurrentAnimationState()
        {
            if (currentAnimation == null)
            {
                return "NULL";
            }
            return currentAnimation.state;
        }

        public void Clear()
        {
            currentAnimation = null;
            animations.Clear();
            transitions.Clear();
            animationOverride = null;
        }
    }
}
