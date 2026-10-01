using Microsoft.AspNetCore.Mvc;
using ExamApi.Models;

namespace ExamApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private static readonly List<Category> Categories = new()
    {
        new Category { Id = 1, Name = "Electronics" },
        new Category { Id = 2, Name = "Accessories" }
    };

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Categories);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var category = Categories.FirstOrDefault(c => c.Id == id);

        if (category == null)
            return NotFound();

        return Ok(category);
    }
}
