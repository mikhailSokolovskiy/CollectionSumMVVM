using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CollectionSumMVVM.Models;

public class MathModel : INotifyPropertyChanged
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
    
    private readonly ObservableCollection<int> _myValues = new ObservableCollection<int>();
    public readonly ReadOnlyObservableCollection<int> MyPublicValues;
    public MathModel() {
        MyPublicValues = new ReadOnlyObservableCollection<int>(_myValues);
    }
    
    //добавление в коллекцию числа и уведомление об изменении суммы
    public void AddValue(int value) {
        _myValues.Add(value);
        OnPropertyChanged("Sum");
    }
    
    public void RemoveValue(int index) {
        //проверка на валидность удаления из коллекции - обязанность модели
        if (index >= 0 && index < _myValues.Count) _myValues.RemoveAt(index);
        OnPropertyChanged("Sum");
    }
    
    public int Sum => MyPublicValues.Sum(); //сумма
}