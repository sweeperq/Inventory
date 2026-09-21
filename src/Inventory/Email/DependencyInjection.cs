namespace Inventory.Email;

public static class DependencyInjection
{
    public static IServiceCollection AddAppEmail(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<EmailSettings>()
                .Bind(configuration.GetSection(nameof(EmailSettings)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddScoped<IEmailService, EmailService>();

        services.AddHealthChecks().AddCheck<EmailHealthCheck>("Email");

        return services;
    }
}
