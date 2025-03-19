using UnityEngine;

namespace Game.UI.Views
{
    public class Unit : MonoBehaviour
    {
        public bool jumpTrigger;
        public bool flyTrigger;
        public bool idleTrigger;

        //Changing by jump animation
        public void Jump()
        {
            jumpTrigger = true;
        }

        public void Fly()
        {
            flyTrigger = true;
        }
        public void Idle()
        {
            idleTrigger = true;
        }
    }
}
