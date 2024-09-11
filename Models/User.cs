using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbCottonDollStore.Models;

public partial class User
{
    [Display(Name = "會員編號")]
    [HiddenInput]
    public string UserID { get; set; } = null!;

    [Display(Name = "手機號碼")]
    [Required(ErrorMessage = "請輸入手機號碼")]
    [RegularExpression("09[0-9]{8}", ErrorMessage = "手機號碼不正確")]
    public string UserPhone { get; set; } = null!;

    [Display(Name = "密碼")]
    [Required(ErrorMessage = "請輸入密碼")]
    [MinLength(8, ErrorMessage = "請輸入8-30位密碼")]
    [MaxLength(30, ErrorMessage = "請輸入8-30位密碼")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "頭像")]
    public string? UserImg { get; set; }

    [Display(Name = "姓名")]
    [Required(ErrorMessage = "請輸入暱稱")]
    [StringLength(30, ErrorMessage = "暱稱長度不可超過30個字")]
    public string Name { get; set; } = null!;

    [Display(Name = "生日")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
    public DateOnly Birthday { get; set; }

    [Display(Name = "性別")]
    public bool? Gender { get; set; }

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "請輸入電子信箱")]
    [DataType(DataType.EmailAddress , ErrorMessage = "請輸入正確的信箱")]
    public string? Email { get; set; }

    [Display(Name = "帳戶")]
    public string? Account { get; set; }

    [Display(Name = "使用者等級")]
    [HiddenInput]
    public string? RankID { get; set; }

    [Display(Name = "註冊日期")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
    [HiddenInput]
    public DateTime RegDate { get; set; }

    [Display(Name = "商店編號")]
    [HiddenInput]
    public string? StoreID { get; set; }

    public virtual ICollection<Order> Order { get; set; } = new List<Order>();
    [Display(Name = "分級")]
    public virtual Rank? Rank { get; set; }

    public virtual ICollection<Store> Store { get; set; } = new List<Store>();
    //[Display(Name = "商店編號")]
    //[HiddenInput]
    //public virtual Store? StoreNavigation { get; set; }
}
