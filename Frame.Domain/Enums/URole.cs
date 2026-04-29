namespace Frame.Domain.Enums
{
    public enum URole
    {
        None = 0,    // Sentinel value for EF Core (CLR default, means "not set")
        Admin = 1,
        Client = 2
    }
}
