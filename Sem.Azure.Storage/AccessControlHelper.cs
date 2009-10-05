namespace Sem.Azure.Storage
{
    using System;
    using System.IdentityModel.Claims;
    using System.ServiceModel;

    /// <summary>
    /// generic helper class for azure access control
    /// </summary>
    public static class AccessControlHelper
    {
        public static void DemandActionClaim(string claimValue)
        {
            foreach (var claimSet in OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets)
            {
                foreach (var claim in claimSet)
                {
                    if (
                        !CheckClaim(
                             claim.ClaimType,
                             claim.Resource.ToString(),
                             "http://docs.oasis-open.org/wsfed/authorization/200706/claims/action",
                             claimValue))
                    {
                        continue;
                    }

                    if (IsIssuedByAcs(claimSet))
                    {
                        return;
                    }
                }
            }

            throw new FaultException("Access denied.");
        }

        static bool IsIssuedByAcs(ClaimSet claimSet)
        {
            foreach (var claim in claimSet.Issuer)
            {
                if (CheckClaim(claim.ClaimType, claim.Resource.ToString(), "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dns", "*.accesscontrol.windows.net"))
                {
                    return true;
                }
            }

            return false;
        }

        static bool CheckClaim(string claimType, string claimValue, string expectedClaimType, string expectedClaimValue)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(claimType, expectedClaimType) &&
                   StringComparer.OrdinalIgnoreCase.Equals(claimValue, expectedClaimValue);
        }
    }
}
