﻿using ETicaret.Application.Abstractions.Storage;
using ETicaret.Application.Features.Commands.Product.CreateProduct;
using ETicaret.Application.Features.Commands.Product.RemoveProduct;
using ETicaret.Application.Features.Commands.Product.UpdateProduct;
using ETicaret.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ETicaret.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ETicaret.Application.Features.Queries.Product.GetAllProduct;
using ETicaret.Application.Features.Queries.Product.GetByIdProduct;
using ETicaret.Application.Features.Queries.ProductImageFile.GetProductImages;
using ETicaret.Application.Repositories;
using ETicaret.Application.RequestParameters;
using ETicaret.Application.Validators;
using ETicaret.Application.ViewModels.Products;
using ETicaret.Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ETicaret.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
    {
       var response = await _mediator.Send(getAllProductQueryRequest); 
       return Ok(response);
    }


    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getbyIdProductQueryRequest)
    {
        var result = await _mediator.Send(getbyIdProductQueryRequest);
        return Ok(result);
        
    }


    [HttpPost]
    [Validator]
    public async Task<IActionResult> Post(CreateProductCommandRequest model)
    {
        var response = await _mediator.Send(model);
        return StatusCode((int)HttpStatusCode.Created);
    }


    [HttpPut]
    [Validator]
    public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
    {
        var response = await _mediator.Send(updateProductCommandRequest);
        return Ok();

    }


    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
    {
        var resposne = await _mediator.Send(removeProductCommandRequest);
        return Ok();
    }


    [HttpPost("[action]")]
    public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest productImageCommandRequest)
    {
        productImageCommandRequest.Files = Request.Form.Files;

        var response = await _mediator.Send(productImageCommandRequest);
        
        return Ok();
    }


    [HttpGet("[action]/{Id}")]
    public async Task<IActionResult> GetImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
    {
        var response = await _mediator.Send(getProductImagesQueryRequest);
        return Ok(response);    
    }


    [HttpDelete("[action]/{Id}")]
    public async Task<IActionResult> DeleteProductImage([FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest,string imageId)
    {
        removeProductImageCommandRequest.ImageId = imageId; 
        var response = await _mediator.Send(removeProductImageCommandRequest);
        return Ok();
    }


}
