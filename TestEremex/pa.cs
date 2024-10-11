using System;

namespace TestEremex;

//[MessagePackObject]
public record ArchiveValue
{
    public ArchiveValue(DateTimeOffset time, object value)
    {
        Time = time; Value = value;
    }
    
 //   [Key(0)]
    public DateTimeOffset Time { get; set; }
    
   // [Key(1)]
    public object Value { get; }

//    [IgnoreMember]
    public DateTime TimeValue => Time.DateTime; 
    
//    [IgnoreMember]
    public double? DoubleValue => GetValue<double>(); 

    public T? GetValue<T>()
    {
        if (Value is T val)
            return val;
        
        return default;
    }
    
    
}