public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantSetter tenantSetter)
    {
        // Check the headers and retrieve the TenantId
        // If there is none - return a 400 immediately
        var tenantId = context.Request.Headers["TenantId"];

        if (string.IsNullOrEmpty(tenantId))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("TenantId header is required");
            return;
        }

        // You can also have a mapping here, or check if that tenant exists in the first place
        tenantSetter.SetTenant(tenantId);

        await _next(context);
    }
}