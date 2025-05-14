using UnityEditor.SceneManagement;
using UnityEngine;

namespace My2D
{
    //1.FadeOut 효과 시작전 대기 시간 설정(지연 시간)
    //2.FadeOut 효과로 서서히 사라진 후 오브젝트 킬
    public class FadeRemoveBehaviour : StateMachineBehaviour
    {
        #region Variables
        //참조
        private SpriteRenderer spriteRenderer;
        private GameObject removeObject;          //효과후 킬 할 오브젝트

        //지연시간 타이머
        public float delayTime = 1f;
        private float delayCountdown = 0f;

        //페이드 효과 타이머
        public float fadeTime = 1f;
        private float fadeCountdown = 0f;

        private Color startColor;
        #endregion

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //참조
            spriteRenderer = animator.GetComponent<SpriteRenderer>();
            removeObject = animator.transform.parent.gameObject;

            //초기화
            startColor = spriteRenderer.color;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //지연시간 타이머
            if(delayCountdown < delayTime)
            {
                delayCountdown += Time.deltaTime;
            }
            else //지연시간이 지남
            {
                //fadeOut 타이머
                fadeCountdown += Time.deltaTime;
                float newAlpha = 1 - fadeCountdown / fadeTime;
                spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

                if(fadeCountdown >= fadeTime)
                {
                    Destroy(removeObject);
                }

            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }

}
