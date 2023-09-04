﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Service.OrderAPI.Models
{
    public class OrderDetails
    {
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public virtual OrderHeader? CartHeader { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string? ProductName { get; set; }
        public double Price { get; set; }
    }
}
