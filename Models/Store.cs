using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbCottonDollStore.Models;

public partial class Store
{
    [Display(Name = "商店編號")]
    [HiddenInput]
    public string StoreID { get; set; } = null!;
    [Display(Name = "會員編號")]
    [HiddenInput]
    public string? UserID { get; set; }
    [Display(Name = "商店名稱")]
    public string? StoreName { get; set; }
    [Display(Name = "商店資訊")]
    [DataType(DataType.MultilineText)]
    public string? StoreInformation { get; set; }

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();

    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Product> Product { get; set; } = new List<Product>();

    public virtual ICollection<ProductSpec> ProductSpec { get; set; } = new List<ProductSpec>();
    [Display(Name = "會員編號")]
    public virtual User? User { get; set; }

    //public virtual ICollection<User> UserNavigation { get; set; } = new List<User>();
}
