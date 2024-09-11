using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbCottonDollStore.Models;

public partial class ProductSpec
{
    [Display(Name = "規格編號")]
    //[HiddenInput]
    public string ProSpecID { get; set; } = null!;
    [ForeignKey("Pro")]
    [Display(Name = "商品編號")]
    [HiddenInput]
    public string ProID { get; set; } = null!;

    [Display(Name = "商店編號")]
    [HiddenInput]
    public string StoreID { get; set; } = null!;

    [Display(Name = "規格圖片")]
    public string? SpecImg { get; set; }
    [Display(Name = "規格")]
    [Required(ErrorMessage = "請輸入商品規格")]
    public string Spec { get; set; } = null!;
    [Display(Name = "數量")]
    [Required(ErrorMessage = "請輸入商品數量")]
    [Range(0, int.MaxValue, ErrorMessage = "商品數量不可低於 0")]
    public int Quantity { get; set; }
    [Display(Name = "單價")]
    [Required(ErrorMessage = "請輸入商品價格")]
    [Range(0, int.MaxValue, ErrorMessage = "商品價格不可低於 0")]
    [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
    public decimal Price { get; set; }
    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

    public virtual Product? Pro { get; set; } = null!;

    public virtual Store? Store { get; set; } = null!;
}
