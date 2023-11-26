using System.Windows.Forms;

namespace C969
{
    class Address
    {
        int addressId1;
        string addressLine1;
        string addressLine2;
        City cityName;
        string zipCode;
        string phoneNumber;
        public Address(int addressId)
        {
            if (addressId == 0)
            {
                MessageBox.Show("Need valid addressID");
            }
            //grabs the address from the database
            if (Database.AddressGet(addressId, out addressLine1, out addressLine2, out int cityId, out zipCode, out phoneNumber))
            {
                addressId1 = addressId;
                cityName = new City(cityId);
            }
            else
            {
                MessageBox.Show("Address object does not exist with primary key.");
            }

        }
        public Address(string address1, string address2, City city, string postalCode, string phone)
        {
            //creates new address within the database
            addressId1 = Database.AddressAdd(address1, address2, city.ID, postalCode, phone);
            addressLine1 = address1;
            addressLine2 = address2;
            cityName = city;
            zipCode = postalCode;
            phoneNumber = phone;
        }
        public Address(int addressId, string address1, string address2, City city, string postalCode, string phone)
        {
            addressId1 = addressId;
            addressLine1 = address1;
            addressLine2 = address2;
            cityName = city;
            zipCode = postalCode;
            phoneNumber = phone;
        }
        public string DisplayAddress()
        {
            return addressLine1 + (addressLine2.Length > 0 ? " " : "") + addressLine2;
        }
        public string Address1
        {
            get { return addressLine1; }
        }
        public string Address2
        {
            get { return addressLine2; }
        }
        public City City
        {
            get { return cityName; }
        }
        public string PostalCode
        {
            get { return zipCode; }
        }
        public string Phone
        {
            get { return phoneNumber; }
        }
        public int ID
        {
            get { return addressId1; }
        }
    }
    class City
    {
        int cityId;
        string cityName;
        Country country;

        public int ID
        {
            get { return cityId; }
        }
        public string Name
        {
            get { return cityName; }
        }
        public Country Country
        {
            get { return country; }
        }
        public City(int cityId1, string cityName1, Country country1)
        {
            cityId = cityId1;
            cityName = cityName1;
            country = country1;
        }
        public City(int cityId1)
        {
            if (Database.CityGet(cityId1, out string cityName1, out int countryId))
            {
                country = new Country(countryId);
                cityName = cityName1;
                cityId = cityId1;
            }
            else
            {
                MessageBox.Show(Languages.LanguageFill("$city ID " + cityId.ToString() + " $notfound"));
            }
        }
        public City(string cityName1, int countryId)
        {
            //this adds a city to the database
            country = new Country(countryId);
            cityName = cityName1;
            cityId = Database.CityAdd(this.Name, country.ID);
        }
        public City(string cityName1, Country country1)
        {
            // grabs the city Id from database.
            cityName = cityName1;
            country = country1;
            cityId = Database.CityAdd(this.Name, country.ID);
        }
    }
    class Country
    {
        int countryId;
        string countryName;
        public int ID
        {
            get { return countryId; }
        }
        public string Name
        {
            get { return countryName; }
        }
        public Country(int countryId1)
        {
            // grabs the country from the database
            if (Database.CountryGet(countryId1, out string countryName1))
            {
                countryName = countryName1;
                countryId = countryId1;
            }
            else
            {
                MessageBox.Show(Languages.LanguageFill("$country ID " + countryId.ToString() + " $notfound"));
            }
        }
        public Country(string countryName1)
        {
            //creates a new country in the database
            countryId = Database.CountryAdd(countryName1);
            countryName = countryName1;
        }
        public Country(int countryId1, string countryName1)
        {
            countryId = countryId1;
            countryName = countryName1;
        }
    }
}
