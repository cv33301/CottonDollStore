using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbCottonDollStore.Models;

public partial class Product
{
    [Display(Name = "商品編號")]
    //[HiddenInput]
    public string ProID { get; set; } = null!;
    [Display(Name = "商品名稱")]
    [Required(ErrorMessage = "請輸入商品名稱")]
    [StringLength(30, ErrorMessage = "暱稱長度不可超過30個字")]
    public string ProName { get; set; } = null!;
    [Display(Name = "商品資訊")]
    public string? Information { get; set; }
    [Display(Name = "商品圖片")]
    public string? ProImg { get; set; }

    [Display(Name = "類別")]
    public string CategoryID { get; set; } = null!;
    [Display(Name = "更新時間")]
    [HiddenInput]
    public DateTime UpdateTime { get; set; }
    [Display(Name = "人氣指數")]
    [HiddenInput]
    public int? Clicks { get; set; }
    [Display(Name = "標籤2")]
    public string? CategoryID_2 { get; set; }
    [Display(Name = "標籤3")]
    public string? CategoryID_3 { get; set; }

    [Display(Name = "下架")]
    [HiddenInput]
    public bool? Statu { get; set; }

    [Display(Name = "商店編號")]
    public string? StoreID { get; set; }

    [Display(Name = "類別")]
    public virtual Category? Category { get; set; } = null!;
    [Display(Name = "標籤2")]
    public virtual Category? CategoryID_2Navigation { get; set; }
    [Display(Name = "標籤3")]
    public virtual Category? CategoryID_3Navigation { get; set; }

    public virtual ICollection<OrderDetail>? OrderDetail { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductSpec>? ProductSpec { get; set; } = new List<ProductSpec>();

    //public List<ProductSpec> Specs { get; set; } = new List<ProductSpec>();

    [Display(Name = "商店編號")]
    public virtual Store? Store { get; set; }

}
