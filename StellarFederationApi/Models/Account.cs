using System.ComponentModel.DataAnnotations;

namespace StellarFederationApi.Models
{
    public class Account
    {
        [Required]
        public string Federation { get; set; }
        [Required]
        public string Address { get; set; }
        public string? Memo { get; set; }
        [Required]
        public MemoType MemoType { get; set; }
    }
}