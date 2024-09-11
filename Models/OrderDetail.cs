using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbCottonDollStore.Models;

public partial class OrderDetail
{
    [Display(Name = "訂單編號")]
    public string OrderID { get; set; } = null!;

    [Display(Name = "規格編號")]
    public string ProSpecID { get; set; } = null!;

    [Display(Name = "商品編號")]
    public string ProID { get; set; } = null!;

    [Display(Name = "商店編號")]
    public string StoreID { get; set; } = null!;

    [Display(Name = "數量")]
    public int Quantity { get; set; }

    [Display(Name = "買家評論")]
    public string? BuyerReview { get; set; }

    [Display(Name = "賣家回覆")]
    public string? SellerRespond { get; set; }

    [Display(Name = "評價")]
    public string? Star { get; set; }

    public virtual Order? Order { get; set; } = null!;

    public virtual Product? Pro { get; set; } = null!;

    public virtual ProductSpec? Spec { get; set; } = null!;

    public virtual Store? Store { get; set; } = null!;
    [Display(Name = "小計")]
    public int Subtotal
    {
        get
        {
            if (Spec == null)
            {
                return 0;
            }
            return Quantity * (int)Spec.Price;
        }
    }
}
