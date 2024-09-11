using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbCottonDollStore.Models;

public partial class Order
{
    [Display(Name = "訂單編號")]
    public string OrderID { get; set; } = null!;
    [Display(Name = "商店編號")]
    public string StoreID { get; set; } = null!;
    [Display(Name = "會員編號")]
    public string UserID { get; set; } = null!;
    [Display(Name = "下單日期")]
    [DisplayFormat(DataFormatString = "{0:MM/dd HH:mm}")]
    public DateTime? OrderDate { get; set; }
    [Display(Name = "付款方式")]
    public string? Payment { get; set; }
    [Display(Name = "運送方式")]
    public string? Logistics { get; set; }
    [Display(Name = "寄件編號")]
    public string? ConNumber { get; set; }
    [Display(Name = "備貨")]
    [DisplayFormat(DataFormatString = "{0:MM/dd  HH:mm}")]
    public DateTime? PreDate { get; set; }
    [Display(Name = "出貨日")]
    [DisplayFormat(DataFormatString = "{0:MM/dd  HH:mm}")]
    public DateTime? ShipDate { get; set; }
    [Display(Name = "送達日")]
    [DisplayFormat(DataFormatString = "{0:MM/dd  HH:mm}")]
    public DateTime? DeliveryDate { get; set; }
    [Display(Name = "取件日")]
    [DisplayFormat(DataFormatString = "{0:MM/dd  HH:mm}")]
    public DateTime? PickupDate { get; set; }
    [Display(Name = "訂單狀態")]
    public bool? Statu { get; set; }
    public virtual ICollection<OrderDetail>? OrderDetail { get; set; } = new List<OrderDetail>();
    [Display(Name = "商店編號")]
    public virtual Store? Store { get; set; }
    [Display(Name = "會員編號")]
    public virtual User? User { get; set; }
}
