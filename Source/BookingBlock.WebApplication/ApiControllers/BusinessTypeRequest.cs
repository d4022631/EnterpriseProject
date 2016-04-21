namespace BookingBlock.WebApplication.ApiControllers
{
    public abstract class BusinessTypeRequest
    {
        /// <summary>
        /// The name of the business type
        /// </summary>
        public string Name { get; set; }
    }
}