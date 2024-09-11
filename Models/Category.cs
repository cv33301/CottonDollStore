using CottonDollStore.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbCottonDollStore.Models;

public partial class Category
{
    [Display(Name = "類別編號")]
    public string CategoryID { get; set; } = null!;
    [Display(Name = "類別名稱")]
    public string CategoryName { get; set; } = null!;
    [Display(Name = "父類別")]
    public string? ParentCategory { get; set; }

    public virtual ICollection<Category> InverseParentCategoryNavigation { get; set; } = new List<Category>();
    [Display(Name = "父類別")]
    public virtual Category? ParentCategoryNavigation { get; set; }

    public virtual ICollection<Product> ProductCategory { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductCategoryID_2Navigation { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductCategoryID_3Navigation { get; set; } = new List<Product>();


}
