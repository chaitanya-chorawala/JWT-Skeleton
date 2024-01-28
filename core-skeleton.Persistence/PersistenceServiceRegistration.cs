using core_skeleton.Core.Contract.Common;
using core_skeleton.Core.Contract.Repository;
using core_skeleton.Core.Contract.Service;
using core_skeleton.Persistence.Common;
using core_skeleton.Persistence.Configuration;
using core_skeleton.Persistence.Repository;
using core_skeleton.Persistence.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace core_skeleton.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddTransient<ApplicationDbContext>();
        services.TryAddScoped<IClaimPrincipalAccessor, ClaimPrincipalAccessor>();
        services.TryAddScoped<ITokenGenerator, TokenGenerator>();
        services.TryAddScoped<IAuthService, AuthService>();
        services.TryAddScoped<IUserRepo, UserRepo>();

        return services;
    }
}
