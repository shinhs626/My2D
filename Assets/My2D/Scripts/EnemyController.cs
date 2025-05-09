using UnityEditor.Tilemaps;
using UnityEngine;

namespace My2D
{
    //이동 방향
    public enum WalkableDirection
    {
        Left,
        Right
    }

    //Enemy Character를 관리하는 클래스
    [RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
    public class EnemyController : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody2D rb2D;
        private TouchingDirection touchingDirection;

        private Vector2 enemyMove = Vector2.right;

        [SerializeField] private float walkSpeed = 4f;

        //현재 이동 방향을 저장
        private WalkableDirection walkableDirection = WalkableDirection.Right;

        // Flip이 이미 실행됐는지 체크
        private bool isFacingWall = false;
        #endregion

        #region Property
        public WalkableDirection WalkableDirection
        {
            get
            {
                return walkableDirection;
            }
            set
            {
                walkableDirection = value;
            }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = this.GetComponent<Rigidbody2D>();
            touchingDirection = this.GetComponent<TouchingDirection>();
        }
        private void FixedUpdate()
        {
            //좌우 이동
            rb2D.linearVelocity = new Vector2(enemyMove.x * walkSpeed, rb2D.linearVelocityY);

            if (touchingDirection.IsWall && touchingDirection.IsGround)
            {

            }
        }
        #endregion

        #region Custom Method
        public void Flip()
        {
            Debug.Log("방향전환");
            if(WalkableDirection == WalkableDirection.Left)
            {
                WalkableDirection = WalkableDirection.Right;
                Debug.Log("방향전환 오른쪽");
            }
            else if(WalkableDirection == WalkableDirection.Right)
            {
                WalkableDirection = WalkableDirection.Left;
                Debug.Log("방향전환 왼쪽");
            }
            else
            {
                Debug.Log("에러");
            }
        }
        #endregion
    }

}
