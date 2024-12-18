using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class HungerBar : MonoBehaviour
{
    private readonly CompositeDisposable _disposable = new();
    
    [SerializeField] private Image _bar;
    
    [Inject]
    public void Construct(DollStatus dollStatus)
    {
        _disposable.Add(dollStatus.Satiety.Subscribe(UpdateBar));
    }

    private void OnDestroy()
    {
        _disposable?.Dispose();
    }

    private void UpdateBar(float value)
    {
        _bar.fillAmount = value;
    }
}