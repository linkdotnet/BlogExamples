public class TenantService : ITenantGetter, ITenantSetter
{
    public string Tenant { get; private set; }
    
    public void SetTenant(string tenant)
    {
        Tenant = tenant;
    }
}