public interface ISavable<T>
{
    public T SaveTo();

    public void LoadFrom(T _data);
}