namespace BookingBlock.WebApplication.ApiControllers
{
    public abstract class PostcodeResponse : ApiResponse
    {
        public string Postcode { get; set; }
    }
}