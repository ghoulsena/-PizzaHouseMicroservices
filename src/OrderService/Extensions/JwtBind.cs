using JwtOptioncs;

namespace JwtBindOpt;

public class JwtBindOpt
{
    private readonly IConfiguration _configuration;
    public JwtBindOpt(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public JwtOptions GetOptions()
    {
        var JwtOptions = new JwtOptions();
        _configuration.GetSection(JwtOptions.Jwt).Bind(JwtOptions);
        return JwtOptions;
    }
}