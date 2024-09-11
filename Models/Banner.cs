using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DbCottonDollStore.Models
{
    public partial class Banner
    {
        [Display(Name = "編號")]
        public int BID { get; set; }

        [Display(Name = "廣告圖片")]
        public string BannerImg { get; set; } = null!;

        [Display(Name = "廣告資訊")]
        public string? Information { get; set; }

        [Display(Name = "開始時間")]
        public DateTime StartDate { get; set; }

        [Display(Name = "結束時間")]
        public DateTime EndDate { get; set; }

    }
}
