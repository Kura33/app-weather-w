using System.Collections.Generic;

namespace weatherAppW
{
    public class Cities
    {
        public List<Data> Data { get; set; }
    }
    public class Data
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string RegionCode { get; set; }
    }
}
