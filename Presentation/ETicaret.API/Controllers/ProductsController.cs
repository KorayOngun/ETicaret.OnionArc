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

    private readonly IOrderReadRepository _orderReadRepository;
    private readonly IOrderWriteRepository _orderWriteRepository;

    public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IOrderReadRepository orderWriteRepository, IOrderWriteRepository orderReadRepository)
    {
        _productWriteRepository = productWriteRepository;
        _productReadRepository = productReadRepository;
        _orderReadRepository = orderWriteRepository;
        _orderWriteRepository = orderReadRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var item = await _orderReadRepository.GetByIdAsync("dc5469b7-f141-4c1c-8ddc-7fa71697b470");
        item.Address = "Güncellenen adres";
        _orderWriteRepository.SaveAsync();
        return Ok(item);
    }

  

}
