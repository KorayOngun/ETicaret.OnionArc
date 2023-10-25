using ETicaret.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;

    public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
    {
        _productWriteRepository = productWriteRepository;
        _productReadRepository = productReadRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        //await _productWriteRepository.AddRangeAsync(new(){
        //        new(){Id = Guid.NewGuid(), Name = "Product 1", Price = 100, CreatedDate = DateTime.UtcNow, Stock = 10},
        //        new(){Id = Guid.NewGuid(), Name = "Product 2", Price = 1400, CreatedDate = DateTime.UtcNow, Stock = 10},
        //        new(){Id = Guid.NewGuid(), Name = "Product 13", Price = 1300, CreatedDate = DateTime.UtcNow, Stock = 10}
        //    });
        //await _productWriteRepository.SaveAsync();
        //return Ok();

        var data = await _productReadRepository.GetByIdAsync("1e358dc9-39bf-4940-a36c-f2a3ad59859d",false);
        data.Name = "degistirme deneme";
        await _productWriteRepository.SaveAsync();
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var item = await _productReadRepository.GetByIdAsync(id);
        return Ok(item);
    }


}
