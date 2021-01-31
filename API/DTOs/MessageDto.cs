using System;

namespace API.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
       // public AppUser Sender { get; set; }                          not req.         // related property AppUser Sender and Recipient both specified
       public string SenderPhotoUrl { get; set; }                     // + added
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public string RecipientPhotoUrl { get; set; }                 // + added
        //public AppUser Recipient { get; set; }                    // - removed
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } // = DateTime.Now; - removed initialisation
      
      //   public bool SenderDeleted { get; set; }       - removed
       //  public bool RecipientDeleted { get; set; }   - removed
    }
}