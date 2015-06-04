using Microsoft.AspNet.Mvc;

namespace HelloWeb.Controllers
{		
	[Route("api/home")]
	public class HomeController : Controller
	{
		
		[HttpGet("Teste")]		
		public IActionResult Teste() 
		{						
			Response.StatusCode = 200;
			return new ObjectResult(new 
			{
				Name = "Douglas",
				Idade = 18	
			});
		}
		
	}
}