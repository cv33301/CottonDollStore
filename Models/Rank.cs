using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbCottonDollStore.Models;

public partial class Rank
{
    [Display(Name = "分級編號")]
    public string RankID { get; set; } = null!;
    [Display(Name = "分級名稱")]
    public string RankName { get; set; } = null!;

    public virtual ICollection<User> User { get; set; } = new List<User>();
}
