using UnityEngine;
using UnityEngine.InputSystem;

namespace My2D
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D rb2D;

        [SerializeField] private float walkSpeed = 4f;
        private Vector2 moveInput;

        #region Unity Event Method
        private void Awake()
        {
            //참조값 가져오기
            rb2D = this.GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            //input 값에 따라 플레이어 좌우 이동
            rb2D.linearVelocity = new Vector2(moveInput.x * walkSpeed, rb2D.linearVelocity.y);
        }
        #endregion

        #region Custom Method
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        #endregion             
    }
}
