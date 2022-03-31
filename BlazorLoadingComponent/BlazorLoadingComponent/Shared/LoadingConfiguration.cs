using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlazorLoadingComponent.Shared;

public class LoadingConfiguration : INotifyPropertyChanged
{
    private string _title;
    private int _currentStep;
    private int _totalSteps;
    private bool _isLoading;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    public int CurrentStep
    {
        get => _currentStep;
        set
        {
            _currentStep = value;
            OnPropertyChanged();
        }
    }

    public int TotalSteps
    {
        get => _totalSteps;
        set
        {
            _totalSteps = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public int GetPercentage()
    {
        return TotalSteps > 0 ? (int)((double)CurrentStep / (double)TotalSteps * 100) : 0;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}