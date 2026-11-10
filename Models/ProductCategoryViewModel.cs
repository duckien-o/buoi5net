using System;

namespace day5_new.Models;

public class ProductCategoryViewModel
{
public int Id { get; set; }
public string Name { get; set; }
public string Description { get; set; } = string.Empty;
public decimal Price { get; set; }

public string CategoryName { get; set; }= string.Empty;
    

}
