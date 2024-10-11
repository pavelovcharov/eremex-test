using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Eremex.AvaloniaUI.Charts;

namespace TestEremex;

public class MyViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public SortedDateTimeDataAdapter DataAdapter { get; set; } = new();

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ObservableCollection<ArchiveValue> Values { get; } = new();
    
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public void Generate()
    {
        var time = DateTimeOffset.Now;
        time = new DateTimeOffset(time.Year, time.Month, time.Day, 0, 0, 0, time.Offset);
        Random r = new Random(1111);
        for (int i = 0; i < 100; i++)
        {
            time = time.AddSeconds(5);
            var  d = r.NextDouble()*10 + 100*Math.Sin(Math.PI*2/100.0*i);
            Values.Add(new ArchiveValue(time,d));
        }
    }
    public void UpdateGraph()
    {
        try
        {
            var results = Values.Where(p => p.DoubleValue.HasValue).Select(p => (p.Time.DateTime, p.DoubleValue!.Value))
                .ToList();
            DataAdapter.Clear();
            DataAdapter.AddRange(results);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}