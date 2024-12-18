using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class EnergyBar : MonoBehaviour
{
    private readonly CompositeDisposable _disposable = new();
    
    [SerializeField] private Image _bar;
    
    [Inject]
    public void Construct(DollStatus dollStatus)
    {
        _disposable.Add(dollStatus.Energy.Subscribe(UpdateBar));
    }
    
    private void OnDestroy()
    {
        _disposable?.Dispose();
    }

    private void UpdateBar(float value)
    {
        Debug.Log("Energy: " + value);
        _bar.fillAmount = value;
    }
}