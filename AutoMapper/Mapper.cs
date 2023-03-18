namespace AutoMapper;

public static class Mapper
{
    public static TResult Map<TIn, TResult>(TIn obj) where TResult : new()
    {
        var result = new TResult();

        var inputProperties = typeof(TIn).GetProperties();
        var resultProperties = typeof(TResult).GetProperties();

        foreach (var inputProperty in inputProperties)
        {
            // Find the property that has the same name and type
            var resultProperty = resultProperties.FirstOrDefault(prop => prop.Name == inputProperty.Name && prop.PropertyType == inputProperty.PropertyType);

            // If it isn't writeable, don't try to write the value
            if (resultProperty != null && resultProperty.CanWrite)
            {
                resultProperty.SetValue(result, inputProperty.GetValue(obj));
            }
        }

        return result;
    }
}