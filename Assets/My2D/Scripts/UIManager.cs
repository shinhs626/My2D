using TMPro;
using UnityEngine;

namespace My2D
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        //데미지 텍스트 프리팹
        public GameObject damageTextPrefab;
        public GameObject healTextPrefab;
        public Canvas canvas;

        //캔버스 위의 스폰위치 가져오기
        private Camera camera;
        [SerializeField]private Vector3 offset;     //캐릭터 머리위로 위치 보정값
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            camera = Camera.main;
        }
        private void OnEnable()
        {
            //이벤트 함수에 함수 등록
            CharacterEvents.characterDamaged += CharacterTakeDamage;
            CharacterEvents.characterHeald += CharacterHeal;
        }
        private void OnDisable()
        {
            //이벤트 함수에 등록된 함수 제거
            CharacterEvents.characterDamaged -= CharacterTakeDamage;
            CharacterEvents.characterHeald -= CharacterHeal;
        }
        #endregion

        #region Custom Method
        public void CharacterTakeDamage(GameObject character ,float damage)
        {
            //프리팹 생성 - 데미지랑 셋팅
            //Debug.Log($"데미지 프리팹 생성 : {damage}");
            Vector3 spawnPosition = camera.WorldToScreenPoint(character.transform.position);
            GameObject textgo = Instantiate(damageTextPrefab, spawnPosition + offset, Quaternion.identity , canvas.transform);

            if (textgo)
            {
                textgo.GetComponent<TextMeshProUGUI>().text = damage.ToString();
            }            
        }
        public void CharacterHeal(GameObject character, float healAmount)
        {
            //프리팹 생성 - 데미지랑 셋팅
            //Debug.Log($"데미지 프리팹 생성 : {damage}");
            Vector3 spawnPosition = camera.WorldToScreenPoint(character.transform.position);
            GameObject textgo = Instantiate(healTextPrefab, spawnPosition + offset, Quaternion.identity, canvas.transform);

            if (textgo)
            {
                textgo.GetComponent<TextMeshProUGUI>().text = healAmount.ToString();
            }
        }
        #endregion
    }

}
