using Microsoft.AspNetCore.Mvc;
using ExamApi.Models;

namespace ExamApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> Products = new()
    {
        new Product { Id = 1, Name = "Jacket", Price = 7590 },
        new Product { Id = 2, Name = "T-Shirt", Price = 2310 }
    };

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Products);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }
}
