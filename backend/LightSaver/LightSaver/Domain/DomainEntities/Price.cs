using System.ComponentModel.DataAnnotations;

namespace LightSaver
{
    public class Price
    {
        [Key]
        public string Id { get; set; }
        public DateTime PriceDate { get; set; } 
        public int IntervalStart { get; set; }
        public int IntervalEnd { get; set; }
        public decimal MWHPricePCB { get; set; }
        public decimal MWHPriceCYM { get; set; }


    }
}
