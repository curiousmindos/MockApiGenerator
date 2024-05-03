namespace MockApiWebApplication;

public class ApplicationCache<TValue>
{
    private readonly IDictionary<string, TValue> cache;
    public ApplicationCache() 
    {
        cache = new Dictionary<string, TValue>();
    }

    public IDictionary<string, TValue> GetAll()
    {
        return cache;
    }

    public bool GetByKey(string key, out TValue value)
    {
        return cache.TryGetValue(key, out value!);
    }
    public void Set(string key, TValue value)
    {
        if (cache.ContainsKey(key))
        {
            cache[key] = value;
        }
        else
        {
            cache.Add(key, value);
        }
    }
}
