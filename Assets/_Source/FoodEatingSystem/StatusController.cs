using UnityEngine;
using VContainer;

namespace FoodEatingSystem
{
    public class StatusController : MonoBehaviour
    {
        private const float _statusDropFrequency = 1f;
        
        [SerializeField] private float _secondsForCompleteExhaustion;
        [SerializeField] private float _secondsForCompleteTiredness;
        private DollStatus _dollStatus;
        
        private float _elapsedTime;
        
        [Inject]
        public void Construct(DollStatus dollStatus)
        {
            _dollStatus = dollStatus;
        }
        
        private void Update()
        {
            SatietyUpdate();
        }

        private void SatietyUpdate()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _statusDropFrequency)
            {
                SatietyDrop();
                _elapsedTime = 0;
            }
        }
        
        private void SatietyDrop()
        {
            _dollStatus.Satiety.Value -= _statusDropFrequency/_secondsForCompleteExhaustion * _dollStatus.Satiety.Max;
            _dollStatus.Energy.Value -= _statusDropFrequency/_secondsForCompleteTiredness * _dollStatus.Energy.Max;
        }
        
        public void EatFood(float satiety)
        {
            
            _dollStatus.Satiety.Value += satiety;
        }
    }
}
