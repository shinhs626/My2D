using UnityEngine;

namespace My2D
{
    //떨어진 아이템을 픽업하면 아이템 효과를 나타낸다
    //Health 충전
    //필드에서 아이템이 회전한다
    public class PickUpItem : MonoBehaviour
    {
        #region Variables
        //회전속도
        private Vector3 spinRotateSpeed = new Vector3(0f,100f,0f);

        //Health 충전
        [SerializeField]private float restoreHealth = 30f;

        private AudioSource pickUpSource;
        #endregion

        #region Unity Event Method
        private void Update()
        {
            //회전
            this.transform.eulerAngles += spinRotateSpeed * Time.deltaTime;

            pickUpSource = this.transform.GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log($"플레이어 Hp 회복 {restoreHealth}");
            Damageable damageable = collision.GetComponent<Damageable>();
            if (damageable)
            {
                bool isHeal = damageable.Heal(restoreHealth);

                if (isHeal)
                {
                    if (pickUpSource)
                    {
                        //사운드 효과
                        pickUpSource.PlayOneShot(pickUpSource.clip);
                    }

                    Destroy(this.transform.gameObject);
                }                
            }

        }
        #endregion
    }

}
