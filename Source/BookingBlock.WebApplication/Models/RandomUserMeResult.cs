namespace BookingBlock.WebApplication.Models
{
    public class RandomUserMeResult
    {
        public string gender { get; set; }
        public RandomUserMeName name { get; set; }
        public RandomUserMeLocation location { get; set; }
        public string email { get; set; }
        public RandomUserMeLogin login { get; set; }
        public int registered { get; set; }
        public int dob { get; set; }
        public string phone { get; set; }
        public string cell { get; set; }
        public RandomUserMeId id { get; set; }
        public RandomUserMePicture picture { get; set; }
        public string nat { get; set; }
    }
}