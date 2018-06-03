using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{

    [Action("Audio/PlaySound")]
    [Help("Plays an audio clip from the game object position")]
    public class PlaySound : GOAction
    {
        [InParam("clip")]
        [Help("The clip that must be played")]
        public AudioClip clip;

        [InParam("volume")]
        [Help("Volume of the clip")]
        public float volume = 1f;

        [InParam("waitUntilFinish")]
        [Help("Wheter the action waits till the end of the clip to be completed")]
        public bool waitUntilFinish = false;

        private float elapsedTime;

        public override void OnStart()
        {
            AudioSource.PlayClipAtPoint(clip, gameObject.transform.position, volume);

            elapsedTime = Time.time;
        }

        public override TaskStatus OnUpdate()
        {
            elapsedTime += Time.deltaTime;
            if (!waitUntilFinish || elapsedTime >= clip.length)
                return TaskStatus.COMPLETED;
            return TaskStatus.RUNNING;
        }
    }
}
