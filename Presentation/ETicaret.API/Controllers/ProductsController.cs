using ETicaret.Application.Repositories;
using ETicaret.Application.RequestParameters;
using ETicaret.Application.Validators;
using ETicaret.Application.ViewModels.Products;
using ETicaret.Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

    [HttpGet("[action]")]
    public IActionResult Get([FromQuery]Pagination pagination)
    {
        var totalCount = _productReadRepository.GetAll(false).Count();
        var products =  _productReadRepository.GetAll(false).Skip(pagination.Page*pagination.Size).Take(pagination.Size).Select(p =>new
        {
            p.Id,
            p.Name,
            p.Stock,
            p.Price,
            p.CreatedDate,
            p.UpdatedDate
        });
        return Ok(new
        {
            totalCount,
            products
        });
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await _productReadRepository.GetByIdAsync(id,false));
    }
    
    [HttpPost]
    [Validator]
    public async Task<IActionResult> Post(VM_Create_Product model)
    {
        
        await _productWriteRepository.AddAsync(new()
        {
            Name = model.Name,
            Price = model.Price,
            Stock = model.Stock,
        });
        await _productWriteRepository.SaveAsync();
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    [Validator]
    public async Task<IActionResult> Put(VM_Update_Product model)
    {
        Product product = await _productReadRepository.GetByIdAsync(model.Id);
        product.Stock = model.Stock;
        product.Name = model.Name;
        product.Price = model.Price;
        await _productWriteRepository.SaveAsync();
        return Ok();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _productWriteRepository.Remove(id);
        await _productWriteRepository.SaveAsync();

        return Ok();
    }
}
