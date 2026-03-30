using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Steam_TripleBrain.Models
{
    [Keyless]
    public class FriendShip
    {
        
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public string Status { get; set; } = "neutral";
    }
}
