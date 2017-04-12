namespace TexasHoldem.Bridges
{
    public interface IBridge
    {
        bool register(string v1, string v2);
        bool isUserExist(string v);
        bool deleteUser(string eladkamin);
    }
}