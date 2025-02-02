using Microsoft.AspNetCore.Mvc;

namespace asp_rest_model.Controllers;

[ApiController]
[Route("/")]
public class RootController: ControllerBase
{
    [HttpGet("/teste")]
    public string TestConnection() => "Funcionando";
}