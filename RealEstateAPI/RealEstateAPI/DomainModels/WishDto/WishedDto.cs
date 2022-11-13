namespace RealEstateAPI.DomainModels.WishDto
{
    public class WishedDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int SellRent { get; set; }
        public string Name { get; set; }
        public string PropertyType { get; set; }
        public string FurnishingType { get; set; }
        public int Price { get; set; }
        public int BHK { get; set; }
        public int BuiltArea { get; set; }
        public string City { get; set; }
        public bool ReadyToMove { get; set; }
        public string photo { get; set; }
        public int db_RegisterId { get; set; }
    }
}
