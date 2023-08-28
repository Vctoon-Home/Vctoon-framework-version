using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace VctoonClient.Oidc;

public class AvaloniaCurrentUser : ICurrentUser, ITransientDependency
{
    private static readonly Claim[] EmptyClaimsArray = new Claim[0];

    public virtual bool IsAuthenticated => Id.HasValue;

    public virtual Guid? Id
    {
        get
        {
            var claim = FindClaim("sub")?.Value;
            return claim != null ? Guid.Parse(claim) : (Guid?)null;
        }
    }

    public virtual string? UserName => FindClaim("preferred_username")?.Value;

    public virtual string? Name => FindClaim("given_name")?.Value;

    public virtual string? SurName => null; // 没有提供对应信息

    public virtual string? PhoneNumber => null; // 没有提供对应信息

    public virtual bool PhoneNumberVerified => bool.Parse(FindClaim("phone_number_verified")?.Value ?? "false");

    public virtual string? Email => FindClaim("email")?.Value;

    public virtual bool EmailVerified => bool.Parse(FindClaim("email_verified")?.Value ?? "false");

    public virtual Guid? TenantId => null; // 没有提供对应信息

    public virtual string[] Roles => FindClaims("role").Select(c => c.Value).ToArray();

    private readonly ICurrentPrincipalAccessor _principalAccessor;

    public AvaloniaCurrentUser(ICurrentPrincipalAccessor principalAccessor)
    {
        _principalAccessor = principalAccessor;
    }

    public virtual Claim? FindClaim(string claimType)
    {
        var claim = _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
        return claim;
    }

    public virtual Claim[] FindClaims(string claimType)
    {
        return _principalAccessor.Principal?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;
    }

    public virtual Claim[] GetAllClaims()
    {
        return _principalAccessor.Principal?.Claims.ToArray() ?? EmptyClaimsArray;
    }

    public virtual bool IsInRole(string roleName)
    {
        return FindClaims(AbpClaimTypes.Role).Any(c => c.Value == roleName);
    }
}