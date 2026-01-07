using Microsoft.AspNetCore.Mvc;
using JwtOptioncs;
using Microsoft.AspNetCore.Http.HttpResults;
namespace JwtBindJSon;

public class JwtBindOpt
{
    private readonly IConfiguration Configuration;
    public JwtBindOpt(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public JwtOptions GetOptions(){

    var jwtOptions = new JwtOptions();

    Configuration.GetSection(JwtOptions.Jwt).Bind(jwtOptions);
    return jwtOptions;

    }
}