using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CollectionSumMVVM.Models;
using Prism.Commands;

namespace CollectionSumMVVM.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    
    readonly MathModel _model = new MathModel();

    public MainWindowViewModel()
    {
        //таким нехитрым способом мы пробрасываем изменившиеся свойства модели во View
        _model.PropertyChanged += (s, e) => { OnPropertyChanged(e.PropertyName); };
        AddCommand = new DelegateCommand<string>(str => {
            //проверка на валидность ввода - обязанность VM
            int ival;
            if (int.TryParse(str, out ival)) _model.AddValue(ival);
        });
        RemoveCommand = new DelegateCommand<int?>(i => {
            if(i.HasValue) _model.RemoveValue(i.Value);
        });
    }
    public DelegateCommand<string> AddCommand { get; }
    public DelegateCommand<int?> RemoveCommand { get; }
    public int Sum => _model.Sum;
    public ReadOnlyObservableCollection<int> MyValues => _model.MyPublicValues;
}
