using UnityEngine;
using UnityEngine.InputSystem;

namespace My2D
{
    //플레이어를 제어하는 클래스
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //강체
        private Rigidbody2D rb2D;

        //Animator
        public Animator animator;

        //Touch
        private TouchingDirection touchingDirection;

        //이동        
        //걷는 속도 - 좌우로 걷는다
        [SerializeField] private float walkSpeed = 4f;
        //뛰는 속도 - 좌우로 뛴다
        [SerializeField] private float runSpeed = 7f;
        //점프시 좌우 이동 속도
        [SerializeField] private float airSpeed = 5f;

        //이동 입력값
        private Vector2 inputMove;

        //키 입력
        [SerializeField] private bool isMoving = false;
        [SerializeField] private bool isRunning = false;
        //[SerializeField] private 

        //반전
        //캐릭터 이미지가 바라보는 방향 상태 - 오른쪽 바라보면 true
        private bool isFacingRight = true;

        //점프키를 눌렀을때 위로 올라가는 속도값
        [SerializeField] private float jumpVelocity = 5f;
        #endregion

        #region Property
        //이동 키입력값 - 애니메이션 파라미터 셋팅
        public bool IsMoving
        {
            get { return isMoving; }
            set 
            { 
                isMoving = value;
                animator.SetBool(AnimationString.isMoving, value);
            }
        }
        public bool IsRunning
        {
            get { return isRunning; }
            set
            {
                isRunning = value;
                animator.SetBool(AnimationString.isRunning, value);
            }
        }
        //현재 이동 속도 셋팅
        public float CurrentSpeed
        {
            get
            {
                //인풋값이 들어왔을때 && 벽에 부딪히지 않을때
                if (IsMoving && touchingDirection.IsWall == false)
                {
                    if (touchingDirection.IsGround)
                    {
                        if (isRunning)  //시프트를 누르고 있을때
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }                        
                    }
                    else //공중에 떠 있을때
                    {
                        return airSpeed;
                    }                 
                }
                else
                {
                    return 0f;//Idle State
                }

            }
        }

        //캐릭터 이미지가 바라보는 방향 상태 - 오른쪽 바라보면 true
        public bool IsFacingRight
        {
            get
            {
                return isFacingRight;
            }
            set
            {
                //반전구현
                if(IsFacingRight != value)
                {
                    this.transform.localScale *= new Vector2(-1, 1);
                }

                isFacingRight = value;
            }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = this.GetComponent<Rigidbody2D>();
            touchingDirection = this.GetComponent<TouchingDirection>();  // 추가

        }

        private void FixedUpdate()
        {
            //인풋값에 따라 플레이어 좌우 이동
            rb2D.linearVelocity = new Vector2(inputMove.x * CurrentSpeed, rb2D.linearVelocity.y);

            //애니메이션 속도값 셋팅
            animator.SetFloat(AnimationString.yVelocity, rb2D.linearVelocityY);
        }
        #endregion

        #region Custom Method
        public void OnMove(InputAction.CallbackContext context)
        {
            inputMove = context.ReadValue<Vector2>();
            //입력값에 따른 반전
            SetFacingDirection(inputMove);

            //인풋 값이 들어오면 IsMoving 파라미터 셋팅
            IsMoving = (inputMove != Vector2.zero);

        }
        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                IsRunning = true;
            }
            else if(context.canceled)
            {
                IsRunning = false;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (touchingDirection.IsGround == false)
                return;

            if (context.started)
            {
                //속도 연산
                rb2D.linearVelocity = new Vector2(rb2D.linearVelocityX, jumpVelocity);

                //애니메이션
                animator.SetTrigger(AnimationString.jumpTrriger);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                animator.SetTrigger(AnimationString.attackTrriger);
            }
        }

        //반전, 바라보는 방향 전환 - 입력값에 따라
        void SetFacingDirection(Vector2 moveInput)
        {
            //좌로 이동, 우로 이동
            if(moveInput.x > 0f && IsFacingRight == false)    //왼쪽을 바라보고 있고 우로 이동
            {
                IsFacingRight = true;
            }
            else if(moveInput.x < 0f && IsFacingRight == true)   //오른쪽을 바라보고 있고 좌로 이동
            {
                IsFacingRight = false;
            }
        }
        #endregion

    }
}
