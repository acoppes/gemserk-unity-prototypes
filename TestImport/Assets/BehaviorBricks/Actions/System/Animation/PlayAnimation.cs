using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    [Action("Animation/PlayAnimation")]
    [Help("Plays an animation in the game object")]
    public class PlayAnimation : GOAction
    {
        [InParam("animationClip")]
        [Help("The clip that must be played")]
        public AnimationClip animationClip;

        [InParam("crossFadeTime", DefaultValue = 0.25f)]
        [Help("Period of time to fade this animation in and fade other animations out")]
        public float crossFadeTime = 0.25f;

        [InParam("animationWrap")]
        [Help("Wrapping mode of the animation")]
        public WrapMode animationWrap = WrapMode.Loop;

        [InParam("waitUntilFinish")]
        [Help("Wheter the action waits till the end of the animation to be completed")]
        public bool waitUntilFinish;

        private float elapsedTime;

        public override void OnStart()
        {
            Animation animation = gameObject.GetComponent<Animation>();
            animation.AddClip(animationClip, animationClip.name);

            animation[animationClip.name].wrapMode = animationWrap;
            animation.CrossFade(animationClip.name, crossFadeTime);

            elapsedTime = Time.time;
        }

        public override TaskStatus OnUpdate()
        {
            elapsedTime += Time.deltaTime;
            if (!waitUntilFinish || elapsedTime >= animationClip.length - crossFadeTime)
                return TaskStatus.COMPLETED;
            return TaskStatus.RUNNING;
        }
    }
}
