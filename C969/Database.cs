using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C969
{
    static class Database
    {
        //B.Provide the ability to add, update, and delete customer records in the database, including name, address, and phone number.
        static public bool CustomerRecordGet(int customerId, out string name, out int addressId)
        {
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select addressId, customerName from customer where customerId = @customerId";
            command.Parameters.AddWithValue("@customerId", customerId);
            Session.AddParameters(ref command);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    name = reader.GetString("customerName");
                    addressId = reader.GetInt32("addressId");
                    return true;
                }
                else
                {
                    name = "";
                    addressId = 0;
                    return false;
                }
            }
        }
        static public int CustomerRecordAdd(string name, int addressId)
        {
            if (addressId == 0)
            {
                throw new Exception(Languages.LanguageFill("#internalerror address ID is zero."));
            }
            int customerId = 0;
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "insert Customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                " values (@name, @addressId, 1, now(), @username, now(), @username)";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@addressId", addressId);
            Session.AddParameters(ref command);
            if (command.ExecuteNonQuery() == 1)
            {
                command.CommandText = "select last_insert_id() customerId";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        customerId = reader.GetInt32("customerId");
                    }
                    else
                    {
                        throw new Exception(Languages.LanguageFill("#internalerror retrieving new customer ID from database"));
                    }
                }
            }
            else
            {
                throw new Exception(Languages.LanguageFill("#internalerror inserting new customer record."));
            }
            command.Dispose();
            connection.Dispose();
            return customerId;
        }
        static public bool CustomerRecordUpdate(int customerId, string name, int addressId)
        {
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "update customer set customername = @name, addressId = @addressId, lastUpdate = now(), lastUpdateBy = @username where customerId = @customerId";
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@addressId", addressId);
            command.Parameters.AddWithValue("@customerId", customerId);
            Session.AddParameters(ref command);
            if (command.ExecuteNonQuery() == 1)
            {
                command.Dispose();
                connection.Dispose();
                return true;
            }
            else
            {
                command.Dispose();
                connection.Dispose();
                return false;
            }
        }
        static public bool CustomerRecordDelete(int customerId)
        {
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "update customer set active = 0, lastUpdate = now(), lastUpdateBy = @username where customerId = @customerId";
            command.Parameters.AddWithValue("@customerId", customerId);
            Session.AddParameters(ref command);
            if (command.ExecuteNonQuery() == 1)
            {
                command.Dispose();
                connection.Dispose();
                return true;
            }
            else
            {
                command.Dispose();
                connection.Dispose();
                return false;
            }
        }
        static public bool AddressGet(int addressId, out string address1, out string address2, out int cityId, out string postalCode, out string phone)
        {
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select address, address2, cityId, postalCode, phone from address where addressId = @addressId";
            command.Parameters.AddWithValue("@addressId", addressId);
            Session.AddParameters(ref command);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    address1 = reader.GetString("address");
                    address2 = reader.GetString("address2");
                    cityId = reader.GetInt32("cityId");
                    postalCode = reader.GetString("postalCode");
                    phone = reader.GetString("phone");
                    return true;
                }
                else
                {
                    address1 = "";
                    address2 = "";
                    cityId = 0;
                    postalCode = "";
                    phone = "";
                    return false;
                }
            }
        }
        static public int AddressAdd(string address1, string address2, int cityId, string postalCode, string phone)
        {
            int addressId = 0;
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select AddressID from Address where address = @address and address2 = @address2 and cityId = @cityId and postalCode = @postalCode and phone = @phone";
            command.Parameters.AddWithValue("@address", address1);
            command.Parameters.AddWithValue("@address2", address2);
            command.Parameters.AddWithValue("@cityId", cityId);
            command.Parameters.AddWithValue("@postalCode", postalCode);
            command.Parameters.AddWithValue("@phone", phone);
            Session.AddParameters(ref command);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    addressId = reader.GetInt32("addressId");
                }
                else
                {
                    reader.Close();
                    command.CommandText
                        = "insert into Address(Address, Address2, CityID, PostalCode, Phone, createdate, createdby, lastupdate, lastupdateby)" +
                            "values(@address, @address2, @cityId, @postalCode, @phone, now(), @username, now(), @username) ";
                    if (command.ExecuteNonQuery() == 1)
                    {
                        command.CommandText = "select last_insert_id() addressId";
                        using (MySqlDataReader insertIdReader = command.ExecuteReader())
                        {
                            if (insertIdReader.Read())
                            {
                                addressId = insertIdReader.GetInt32("addressId");
                            }
                            else
                            {
                                throw new Exception(Languages.LanguageFill("#internalerror occurred while retrieving new Address ID from database."));
                            }
                        }
                    }
                    else
                    {
                        throw new Exception(Languages.LanguageFill("#internalerror occurred while creating address record."));
                    }
                }
            }
            command.Dispose();
            connection.Dispose();
            return addressId;
        }
        static public List<string> CityList(string country)
        {
            List<string> cityList = new List<string>();
            try
            {
                MySqlConnection connection = Session.GetDBConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select ct.city from city ct inner join country cn on ct.countryId = cn.countryId where cn.country = @country ";
                command.Parameters.AddWithValue("@country", country);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cityList.Add(reader["city"].ToString());
                    }
                }
                command.Dispose();
                connection.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception(Languages.LanguageFill("#internalerror occurred retrieving city list: " + e.Message));
            }
            return cityList;
        }
        static public List<string> CountryList()
        {
            List<string> countryList = new List<string>();
            try
            {
                MySqlConnection connection = Session.GetDBConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select cn.country from country cn ";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        countryList.Add(reader["country"].ToString());
                    }
                }
                command.Dispose();
                connection.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception(Languages.LanguageFill("#internalerror occurred retrieving country list: " + e.Message));
            }
            return countryList;
        }
        static public bool CityGet(int cityId, out string cityName, out int countryId)
        {
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select city, CountryID from city ct where ct.CityID = @CityID";
            command.Parameters.AddWithValue("@cityId", cityId);
            Session.AddParameters(ref command);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    countryId = reader.GetInt32("CountryID");
                    cityName = reader.GetString("city");
                    command.Dispose();
                    connection.Dispose();
                    return true;
                }
                else
                {
                    countryId = 0;
                    cityName = "";
                    command.Dispose();
                    connection.Dispose();
                    return false;
                }
            }
        }
        static public int CityAdd(string cityName, int countryId)
        {
            int cityId = 0;
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select cityId from city ct where ct.city = @cityName and ct.countryId = @countryId";
            command.Parameters.AddWithValue("@cityName", cityName);
            command.Parameters.AddWithValue("@countryId", countryId);
            Session.AddParameters(ref command);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    cityId = (int)reader["cityId"];
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    command.CommandText
                        = "insert into city(city, countryid, createdate, createdby, lastupdate, lastupdateby)" +
                            "values(@cityName, @countryId, now(), @username, now(), @username) ";
                    if (command.ExecuteNonQuery() == 1)
                    {
                        command.CommandText = "select last_insert_id() cityId";
                        using (MySqlDataReader insertIdReader = command.ExecuteReader())
                        {
                            if (insertIdReader.Read())
                            {
                                cityId = insertIdReader.GetInt32("cityId");
                            }
                            else
                            {
                                throw new Exception(Languages.LanguageFill("#internalerror occurred while retrieving new City ID from database."));
                            }
                        }
                    }
                    else
                    {
                        throw new Exception(Languages.LanguageFill("#internalerror occurred while creating city record."));
                    }
                }
            }
            command.Dispose();
            connection.Dispose();
            return cityId;
        }
        static public int CityAdd(string cityCountryString)
        {
            int cityId = 0;
            if (cityCountryString.Contains(","))
            {
                string cityName;
                string countryName;
                string[] cityCountrySplit = cityCountryString.Split(',');
                cityName = cityCountrySplit[0].Trim();
                countryName = cityCountrySplit[1].Trim();
                int countryId = CountryAdd(countryName);
                CityAdd(cityName, countryId);
            }
            else
            {
                throw new Exception(Languages.LanguageFill("#internalerror String should match format [City, Country]"));
            }
            return cityId;
        }
        static public bool CountryGet(int countryId, out string countryName)
        {
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select country from country cn where cn.countryId = @countryId";
            command.Parameters.AddWithValue("@countryId", countryId);
            Session.AddParameters(ref command);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    countryName = reader.GetString("country");
                    command.Dispose();
                    connection.Dispose();
                    return true;
                }
                else
                {
                    countryName = "";
                    command.Dispose();
                    connection.Dispose();
                    return false;
                }
            }
        }
        static public int CountryAdd(string countryName)
        {
            int countryId = 0;

            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select countryId from country cn where cn.country = @countryName";
            command.Parameters.AddWithValue("@countryName", countryName);
            Session.AddParameters(ref command);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    countryId = (int)reader["countryId"];
                    reader.Close();
                }
                else
                {
                    reader.Close();
                    command.CommandText
                        = "insert into country(country, createdate, createdby, lastupdate, lastupdateby)" +
                            "values(@countryName, now(), @username, now(), @username) ";
                    if (command.ExecuteNonQuery() == 1)
                    {
                        command.CommandText = "select last_insert_id() countryId";
                        using (MySqlDataReader insertIdReader = command.ExecuteReader())
                        {
                            if (insertIdReader.Read())
                            {
                                countryId = insertIdReader.GetInt32("countryId");
                            }
                            else
                            {
                                throw new Exception(Languages.LanguageFill("#internalerror occurred while retrieving new Country ID from database."));
                            }
                        }
                    }
                    else
                    {
                        throw new Exception(Languages.LanguageFill("#internalerror occurred creating country record."));
                    }
                }
            }
            command.Dispose();
            connection.Dispose();
            return countryId;
        }
        public static List<CustomerInfo> CustomerList()
        {
            List<CustomerInfo> customers = new List<CustomerInfo>();
            Dictionary<int, Country> countryDictionary = new Dictionary<int, Country>();
            Dictionary<int, City> cityDictionary = new Dictionary<int, City>();
            Dictionary<int, Address> addressDictionary = new Dictionary<int, Address>();
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select countryId, country from Country";
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    countryDictionary.Add(reader.GetInt32("countryId")
                                            , new Country(reader.GetInt32("countryId")
                                                        , reader.GetString("country")
                                                         )
                                         );
                }
            }
            command.CommandText = "select cityId, city, countryId from City";
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    cityDictionary.Add(reader.GetInt32("cityId")
                                    , new City(reader.GetInt32("cityId")
                                            , reader.GetString("city")
                                            , countryDictionary[reader.GetInt32("countryId")]
                                            )
                                        );
                }
            }
            command.CommandText = "select addressId, address, address2, cityId, postalCode, phone from Address";
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    addressDictionary.Add(reader.GetInt32("addressId")
                                    , new Address(reader.GetInt32("addressId")
                                            , reader.GetString("address")
                                            , reader.GetString("address2")
                                            , cityDictionary[reader.GetInt32("cityId")]
                                            , reader.GetString("postalCode")
                                            , reader.GetString("phone")
                                            )
                                        );
                }
            }
            command.CommandText = "select customerId, customerName, addressId from customer where active = 1";
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    customers.Add(new CustomerInfo(reader.GetInt32("customerId")
                                            , reader.GetString("customerName")
                                            , addressDictionary[reader.GetInt32("addressId")]
                                            )
                                );
                }
            }
            return customers;
        }
        public static int UserAdd(string userName)
        {
            int userId;
            using (MySqlConnection connection = Session.GetDBConnection())
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select userId from user where userName = @newusername ";
                command.Parameters.AddWithValue("@newusername", userName);
                Session.AddParameters(ref command);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userId = reader.GetInt32("userId");
                    }
                    else
                    {
                        command.CommandText = "insert user (userName, password, active, createDate, createdBy, lastUpdate, lastUpdateBy) "
                            + " values (@newusername, '', 1, utc_timestamp(), @username, utc_timestamp(), @username) ";
                        reader.Close();
                        if (command.ExecuteNonQuery() == 1)
                        {
                            userId = (int)command.LastInsertedId;
                        }
                        else
                        {
                            throw new Exception(Languages.LanguageFill("#internalerror inserting user " + userName));
                        }
                    }
                }
            }
            return userId;
        }
        public static bool UserPasswordGet(int userId, out string passwordHash)
        {
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select password from user where userId = @userId";
            command.Parameters.AddWithValue("@userId", userId);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                passwordHash = reader.GetString("password");
                reader.Close();
                command.Dispose();
                connection.Dispose();
                return true;
            }
            else
            {
                passwordHash = "";
                reader.Close();
                command.Dispose();
                connection.Dispose();
                return false;
            }
        }
        public static bool UserPasswordSet(int userId, string passwordHash)
        {
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "update user set password = @passwordHash, lastUpdate = now(), lastUpdateBy = @username where userId = @userId";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@passwordHash", passwordHash);
            Session.AddParameters(ref command);
            if (command.ExecuteNonQuery() == 1)
            {
                command.Dispose();
                connection.Dispose();
                return true;
            }
            else
            {
                command.Dispose();
                connection.Dispose();
                return false;
            }
        }
        public static bool UserGet(int userId, out string userName)
        {
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select userName from user where userId = @userId";
            command.Parameters.AddWithValue("@userId", userId);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                userName = reader.GetString("userName");
                reader.Close();
                command.Dispose();
                connection.Dispose();
                return true;
            }
            else
            {
                userName = "";
                reader.Close();
                command.Dispose();
                connection.Dispose();
                throw new Exception(Languages.LanguageFill("#internalerror #usernotfound"));
            }
        }
        public static bool UserGet(string userName, out int userId)
        {
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select userId from user where userName = @userName";
            command.Parameters.AddWithValue("@userName", userName);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                userId = reader.GetInt32("userId");
                reader.Close();
                command.Dispose();
                connection.Dispose();
                return true;
            }
            else
            {
                userId = 0;
                reader.Close();
                command.Dispose();
                connection.Dispose();
                throw new Exception(Languages.LanguageFill("#usernotfound"));
            }
        }

        //C.Provide the ability to add, update, and delete appointments, capturing the type of appointment and a link to the specific customer record in the database.
        static public bool AppointmentRecordDelete(int appointmentId)
        {
            MySqlConnection connection = Session.GetDBConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "delete from appointment where appointmentId = @appointmentId";
            command.Parameters.AddWithValue("@appointmentId", appointmentId);
            Session.AddParameters(ref command);
            if (command.ExecuteNonQuery() == 1)
            {
                command.Dispose();
                connection.Dispose();
                return true;
            }
            else
            {
                command.Dispose();
                connection.Dispose();
                return false;
            }
        }
        static public Dictionary<int, Country> CountryDictionary()
        {
            Dictionary<int, Country> countryDictionary = new Dictionary<int, Country>();
            using (MySqlConnection connection = Session.GetDBConnection())
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select countryId, country from Country";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            countryDictionary.Add(reader.GetInt32("countryId")
                                                    , new Country(reader.GetInt32("countryId")
                                                                , reader.GetString("country")
                                                                 )
                                                 );
                        }
                    }
                }
            }
            return countryDictionary;
        }
        static public Dictionary<int, City> CityDictionary()
        {
            Dictionary<int, Country> countryDictionary = CountryDictionary();
            Dictionary<int, City> cityDictionary = new Dictionary<int, City>();
            using (MySqlConnection connection = Session.GetDBConnection())
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select cityId, city, countryId from City";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cityDictionary.Add(reader.GetInt32("cityId")
                                            , new City(reader.GetInt32("cityId")
                                                    , reader.GetString("city")
                                                    , countryDictionary[reader.GetInt32("countryId")]
                                                    )
                                                );
                        }
                    }
                }
            }
            return cityDictionary;
        }
        static public Dictionary<int, Address> AddressDictionary()
        {
            Dictionary<int, City> cityDictionary = CityDictionary();
            Dictionary<int, Address> addressDictionary = new Dictionary<int, Address>();
            using (MySqlConnection connection = Session.GetDBConnection())
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select addressId, address, address2, cityId, postalCode, phone from Address";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            addressDictionary.Add(reader.GetInt32("addressId")
                                            , new Address(reader.GetInt32("addressId")
                                                    , reader.GetString("address")
                                                    , reader.GetString("address2")
                                                    , cityDictionary[reader.GetInt32("cityId")]
                                                    , reader.GetString("postalCode")
                                                    , reader.GetString("phone")
                                                    )
                                                );
                        }
                    }
                }
            }
            return addressDictionary;
        }
        static public Dictionary<int, CustomerInfo> CustomerDictionary(bool activeOnly)
        {
            Dictionary<int, Address> addressDictionary = AddressDictionary();
            Dictionary<int, CustomerInfo> customerDictionary = new Dictionary<int, CustomerInfo>();
            using (MySqlConnection connection = Session.GetDBConnection())
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select customerId, customerName, addressId from customer where active = 1 or not @activeOnly ";
                    command.Parameters.AddWithValue("@activeOnly", activeOnly);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customerDictionary.Add(reader.GetInt32("customerId")
                                                    , new CustomerInfo(reader.GetInt32("customerId")
                                                        , reader.GetString("customerName")
                                                        , addressDictionary[reader.GetInt32("addressId")]
                                                        )
                                                    );
                        }
                    }
                }
            }
            return customerDictionary;
        }
        static public Dictionary<int, User> UserDictionary()
        {
            Dictionary<int, User> userDictionary = new Dictionary<int, User>();
            using (MySqlConnection connection = Session.GetDBConnection())
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select userId, userName from user";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userDictionary.Add(reader.GetInt32("userId")
                                                    , new User(reader.GetInt32("userId")
                                                        , reader.GetString("userName")
                                                        )
                                                    );
                        }
                    }
                }
            }
            return userDictionary;
        }
        static public List<Appointment> AppointmentList(int? userId, DateTime start, DateTime end)
        {
            //throw new NotImplementedException();
            //public Appointment(int appointmentId, Customer customer, User user, string title, string description, string location, string type, string url, DateTime start, DateTime end)

            List<Appointment> appointments = new List<Appointment>();
            Dictionary<int, CustomerInfo> customerDictionary = CustomerDictionary(false);
            Dictionary<int, User> userDictionary = UserDictionary();

            using (MySqlConnection connection = Session.GetDBConnection())
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT appointmentId, customerId, userId, title, description, location, contact, type, url, start, end FROM appointment";
                    command.CommandText += " where (userId = @userId or @userId is null)";
                    command.CommandText += " and ( (start > @start and start < @end) "; //start time between start & end
                    command.CommandText += "    or (end > @start and end < @end) "; //end time between start & end
                    command.CommandText += "    or (start < @start and end > @end) ) "; //start time before start and end time after end
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@start", UtcFromLocalTime(start));
                    command.Parameters.AddWithValue("@end", UtcFromLocalTime(end));
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add(new Appointment(reader.GetInt32("appointmentId")
                                                            , customerDictionary[reader.GetInt32("customerId")]
                                                            , userDictionary[reader.GetInt32("userId")]
                                                            , reader.GetString("title")
                                                            , reader.GetString("description")
                                                            , reader.GetString("location")
                                                            , reader.GetString("contact")
                                                            , reader.GetString("type")
                                                            , reader.GetString("url")
                                                            , LocalTimeFromUTC(reader.GetDateTime("start"))
                                                            , LocalTimeFromUTC(reader.GetDateTime("end"))
                                                            )
                                            );
                        }
                    }
                }
            }
            return appointments;
        }
        static public DateTime LocalTimeFromUTC(DateTime UTC)
        {
            DateTimeOffset utcTimeOffset = new DateTimeOffset(UTC, TimeSpan.Zero);
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            DateTimeOffset localTimeOffset = TimeZoneInfo.ConvertTime(utcTimeOffset, localTimeZone);
            return localTimeOffset.DateTime;
        }
        static public DateTime UtcFromLocalTime(DateTime localTime)
        {
            return localTime.ToUniversalTime();

        }
        static public bool ValidateAppointmentTime(DateTime start, DateTime end, int userId, int? appointmentId)
        {
            bool valid = false;
            using (MySqlConnection connection = Session.GetDBConnection())
            {
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select count(*) conflictCount from appointment " +
                        " where userId = @userId and " +
                            " ( " + "(start >= @start and start < @end)" +
                                    " or (end > @start and end <= @end)" +
                                    " or (start <= @start and end >= @end)" +
                            " ) and (appointmentId <> @appointmentId or @appointmentId is null)";
                    command.Parameters.AddWithValue("@start", UtcFromLocalTime(start));
                    command.Parameters.AddWithValue("@end", UtcFromLocalTime(end));
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@appointmentId", appointmentId);

                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.GetInt32("conflictCount") == 0)
                    {
                        valid = true;
                    }
                    reader.Close();
                }
            }
            return valid;
        }
        static public int AppointmentRecordSet(int? appointmentId, int customerId, int userId, string title, string description, string location, string contact, string type, string url, DateTime start, DateTime end)
        {
            int _appointmentId;
            if (!ValidateAppointmentTime(start, end, userId, appointmentId))
            {
                throw new Exception(Languages.LanguageFill("#appointmentconflict"));
            }
            using (MySqlConnection connection = Session.GetDBConnection())
            {
                MySqlCommand command = connection.CreateCommand();
                if (appointmentId.HasValue)
                {
                    command.CommandText = "update appointment " +
                        " set customerId = @customerId, userId = @userId, title = @title, description = @description " +
                        " , location = @location, contact = @contact, type = @type, url = @url, start = @start, end = @end " +
                        " , lastUpdate = now(), lastUpdateBy = @username " +
                        " where appointmentId = @appointmentId ";
                }
                else
                {
                    command.CommandText = "insert appointment " +
                        " (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                        " values (@customerId, @userId, @title, @description, @location, @contact, @type, @url, @start, @end, now(), @username, now(), @username) ";
                }
                command.Parameters.AddWithValue("@appointmentId", appointmentId);
                command.Parameters.AddWithValue("@customerId", customerId);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@location", location);
                command.Parameters.AddWithValue("@contact", contact);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@url", url);
                command.Parameters.AddWithValue("@start", UtcFromLocalTime(start));
                command.Parameters.AddWithValue("@end", UtcFromLocalTime(end));
                Session.AddParameters(ref command);
                if (command.ExecuteNonQuery() == 1)
                {
                    if (!appointmentId.HasValue)
                    {
                        _appointmentId = (int)command.LastInsertedId;
                    }
                    else
                    {
                        _appointmentId = appointmentId.Value;
                    }
                }
                else
                {
                    throw new Exception("Not able to update/insert appointment.");
                }
            }
            return _appointmentId;
        }
        static public void AppointmentRecordGet(int appointmentId, out int customerId, out int userId, out string title, out string description, out string location, out string contact, out string type, out string url, out DateTime start, out DateTime end)
        {
            using (MySqlConnection connection = Session.GetDBConnection())
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select customerId, userId, title, description, location, contact, type, url, start, end from appointment where appointmentId = @appointmentId";
                command.Parameters.AddWithValue("@appointmentId", appointmentId);
                Session.AddParameters(ref command);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        customerId = reader.GetInt32("customerId");
                        userId = reader.GetInt32("userId");
                        title = reader.GetString("title");
                        description = reader.GetString("description");
                        location = reader.GetString("location");
                        contact = reader.GetString("contact");
                        type = reader.GetString("type");
                        url = reader.GetString("url");
                        start = LocalTimeFromUTC(reader.GetDateTime("start"));
                        end = LocalTimeFromUTC(reader.GetDateTime("end"));
                    }
                    else
                    {
                        throw new Exception(Languages.LanguageFill("#internalerror #appointment #notfound"));
                    }
                }
                command.Dispose();
            }
        }
        static public string AdHocToHTML(string query, params object[] sqlParams)
        {
            StringBuilder htmlTable = new StringBuilder();
            using (MySqlConnection connection = Session.GetDBConnection())
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = query;

                for (int i = 0; i < sqlParams.Count(); i++)
                {
                    command.Parameters.AddWithValue("@p" + i.ToString(), sqlParams[i]);
                }
                Session.AddParameters(ref command);
                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        htmlTable.Append("<table style='border-collapse: collapse; width: 100%;'>");
                        htmlTable.Append("<tr>");
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            htmlTable.Append("<th style='border: 1px solid black; padding: 8px';>");
                            htmlTable.Append(reader.GetName(i));
                            htmlTable.Append("</th>");
                        }
                        htmlTable.Append("</tr>");
                        //read from the reader...
                        while (reader.Read())
                        {
                            htmlTable.Append("<tr>");
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                htmlTable.Append("<td style='border: 1px solid black; padding: 8px;'>");
                                htmlTable.Append(reader[i].GetType() == typeof(DateTime) ? LocalTimeFromUTC(reader.GetDateTime(i)).ToString("yyyy-MM-dd h:mm tt") : reader[i].ToString());
                                htmlTable.Append("</td>");
                            }
                            htmlTable.Append("</tr>");
                        }
                        htmlTable.Append("</table>");
                    }
                }
                catch (Exception e)
                {
                    string msg = e.Message + "\nParameters:\n";
                    foreach (MySqlParameter parameter in command.Parameters)
                    {
                        msg += parameter.ParameterName + " = " + parameter.Value.ToString() + "\n";
                    }
                    throw new Exception(msg);
                }
            }
            return htmlTable.ToString();
        }
        static public List<string> AdHocToStringList(string query)
        {
            List<string> output = new List<string>();
            using (MySqlConnection connection = Session.GetDBConnection())
            {
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = query;
                Session.AddParameters(ref command);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!(reader.FieldCount == 1))
                    {
                        throw new Exception(Languages.LanguageFill("#toomanycolumns"));
                    }
                    //read from the reader...
                    while (reader.Read())
                    {
                        output.Add(reader.GetString(0));
                    }
                }
            }
            return output;
        }
    }
}
