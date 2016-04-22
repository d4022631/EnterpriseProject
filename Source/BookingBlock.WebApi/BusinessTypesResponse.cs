namespace BookingBlock.WebApi
{
    public class BusinessTypesResponse
    {
        public BusinessTypeInfoList Results { get; set; } = new BusinessTypeInfoList();

        public int Total { get; set; }
    }
}