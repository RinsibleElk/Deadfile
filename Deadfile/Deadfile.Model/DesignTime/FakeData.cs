using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deadfile.Entity;

namespace Deadfile.Model.DesignTime
{
    public static class FakeData
    {
        /// <summary>
        /// Generates some fake data for design time experience, using EN postcodes, simple email addresses and UK mobile numbers.
        /// </summary>
        /// <returns>The fake data.</returns>
        public static IEnumerable<Client> GetFakeClients()
        {
            var li = new List<Client>();
            li.Add(new Client() { Title = "Mr", FirstName = "Sid", MiddleNames = "", LastName = "Abbott", AddressFirstLine = "Birch Avenue", AddressSecondLine = "Putney", AddressThirdLine = "", AddressPostCode = "EN9 6BS", PhoneNumber1 = "07611512751", EmailAddress = "Sid.Abbott@yahoo.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Morticia", MiddleNames = "", LastName = "Addams", AddressFirstLine = "Cemetery Ridge", AddressSecondLine = "USA", AddressThirdLine = "", AddressPostCode = "EN4 9VB", PhoneNumber1 = "07722533250", EmailAddress = "Morticia.Addams@gmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Susan", MiddleNames = "", LastName = "Alexander", AddressFirstLine = "185 West 74th Street", AddressSecondLine = "New York", AddressThirdLine = "", AddressPostCode = "EN1 8ZQ", PhoneNumber1 = "07853107850", EmailAddress = "Susan.Alexander@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Jim", MiddleNames = "", LastName = "Anderson", AddressFirstLine = "607 South Maple Street", AddressSecondLine = "USA", AddressThirdLine = "", AddressPostCode = "EN5 1XD", PhoneNumber1 = "07719651035", EmailAddress = "Jim.Anderson@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Larry", MiddleNames = "", LastName = "Appleton", AddressFirstLine = "711 Calhoun Street", AddressSecondLine = "Chicago", AddressThirdLine = "Illinois", AddressPostCode = "EN10 2FW", PhoneNumber1 = "07805873100", EmailAddress = "Larry.Appleton@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Jon", MiddleNames = "", LastName = "Arbuckle", AddressFirstLine = "711 Maple Street", AddressSecondLine = "USA", AddressThirdLine = "", AddressPostCode = "EN9 6JC", PhoneNumber1 = "07761272610", EmailAddress = "Jon.Arbuckle@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Lew", MiddleNames = "", LastName = "Archer", AddressFirstLine = "8411 1/2 Sunset Boulevard", AddressSecondLine = "Hollywood", AddressThirdLine = "California", AddressPostCode = "EN2 6GP", PhoneNumber1 = "07650476203", EmailAddress = "Lew.Archer@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Tony", MiddleNames = "", LastName = "Stark", AddressFirstLine = "890 Fifth Avenue", AddressSecondLine = "Manhattan", AddressThirdLine = "New York City", AddressPostCode = "EN3 2CX", PhoneNumber1 = "07207040745", EmailAddress = "Tony.Stark@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "", MiddleNames = "", LastName = "Bananaman", AddressFirstLine = "29 Acacia Road", AddressSecondLine = "", AddressThirdLine = "", AddressPostCode = "EN6 2PV", PhoneNumber1 = "07408000609", EmailAddress = "Bananaman@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Raymond", MiddleNames = "", LastName = "Barone", AddressFirstLine = "320 Fowler Street", AddressSecondLine = "Lynbrook", AddressThirdLine = "New York", AddressPostCode = "EN11 3EJ", PhoneNumber1 = "07943603147", EmailAddress = "Raymond.Barone@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Bruce", MiddleNames = "", LastName = "Wayne", AddressFirstLine = "Wayne Manor", AddressSecondLine = "Gotham City", AddressThirdLine = "USA", AddressPostCode = "EN3 5IN", PhoneNumber1 = "07542957089", EmailAddress = "Bruce.Wayne@yahoo.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "Hazel", MiddleNames = "", LastName = "Burke", AddressFirstLine = "123 Marshall Road", AddressSecondLine = "Hydsberg", AddressThirdLine = "New York", AddressPostCode = "EN1 6MQ", PhoneNumber1 = "07371143711", EmailAddress = "Hazel.Burke@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Cornelius", MiddleNames = "", LastName = "Bear", AddressFirstLine = "62 Achewood Court", AddressSecondLine = "", AddressThirdLine = "", AddressPostCode = "EN4 7QJ", PhoneNumber1 = "07255976725", EmailAddress = "Cornelius.Bear@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "", MiddleNames = "", LastName = "Benn", AddressFirstLine = "52 Festive Road", AddressSecondLine = "Putney", AddressThirdLine = "London", AddressPostCode = "EN10 3UF", PhoneNumber1 = "07960390220", EmailAddress = "Benn@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Frank", MiddleNames = "", LastName = "Black", AddressFirstLine = "1910 Ezekiel Drive", AddressSecondLine = "Seattle", AddressThirdLine = "WA", AddressPostCode = "EN4 9PR", PhoneNumber1 = "07254091133", EmailAddress = "Frank.Black@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Sirius", MiddleNames = "", LastName = "Black", AddressFirstLine = "12 Grimmauld Place", AddressSecondLine = "London", AddressThirdLine = "UK", AddressPostCode = "EN9 3VX", PhoneNumber1 = "07780088298", EmailAddress = "Sirius.Black@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Leopold", MiddleNames = "", LastName = "Bloom", AddressFirstLine = "7 Eccles Street", AddressSecondLine = "", AddressThirdLine = "", AddressPostCode = "EN9 1EG", PhoneNumber1 = "07256612102", EmailAddress = "Leopold.Bloom@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Elwood", MiddleNames = "", LastName = "Blues", AddressFirstLine = "1060 West Addison Street", AddressSecondLine = "Chicago", AddressThirdLine = "Illinois", AddressPostCode = "EN6 1EO", PhoneNumber1 = "07997181255", EmailAddress = "Elwood.Blues@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Charlie", MiddleNames = "", LastName = "Bone", AddressFirstLine = "9 Filbert Street", AddressSecondLine = "", AddressThirdLine = "", AddressPostCode = "EN11 8RJ", PhoneNumber1 = "07715384394", EmailAddress = "Charlie.Bone@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Ed", MiddleNames = "", LastName = "Boone", AddressFirstLine = "36 Randolph Street", AddressSecondLine = "Swindon", AddressThirdLine = "", AddressPostCode = "EN4 8BP", PhoneNumber1 = "07598110197", EmailAddress = "Ed.Boone@gmail.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "Nellie", MiddleNames = "", LastName = "Boswell", AddressFirstLine = "30 Kelsall Street", AddressSecondLine = "Liverpool", AddressThirdLine = "", AddressPostCode = "EN3 6GO", PhoneNumber1 = "07454199813", EmailAddress = "Nellie.Boswell@gmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Angela", MiddleNames = "", LastName = "Bower", AddressFirstLine = "3344 Oak Hills Drive", AddressSecondLine = "Fairfield", AddressThirdLine = "Connecticut", AddressPostCode = "EN7 7CF", PhoneNumber1 = "07569872358", EmailAddress = "Angela.Bower@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Mike", MiddleNames = "", LastName = "Brady", AddressFirstLine = "4222 Clinton Way", AddressSecondLine = "Los Angeles", AddressThirdLine = "California", AddressPostCode = "EN6 4NB", PhoneNumber1 = "07286122860", EmailAddress = "Mike.Brady@hotmail.com" });
            li.Add(new Client() { Title = "Dr", FirstName = "Emmett", MiddleNames = "", LastName = "Brown", AddressFirstLine = "1640 Riverside Drive", AddressSecondLine = "Hill Valley", AddressThirdLine = "California", AddressPostCode = "EN9 5P[", PhoneNumber1 = "07488799489", EmailAddress = "Emmett.Brown@yahoo.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Hyacinth", MiddleNames = "", LastName = "Bucket", AddressFirstLine = "Blossom Avenue", AddressSecondLine = "UK", AddressThirdLine = "", AddressPostCode = "EN8 4SZ", PhoneNumber1 = "07152657183", EmailAddress = "Hyacinth.Bucket@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Ferris", MiddleNames = "", LastName = "Bueller", AddressFirstLine = "164 North Dutton Street", AddressSecondLine = "Santa Monica", AddressThirdLine = "CA", AddressPostCode = "EN5 3DI", PhoneNumber1 = "07572575190", EmailAddress = "Ferris.Bueller@hotmail.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "Phoebe", MiddleNames = "", LastName = "Buffay", AddressFirstLine = "5 Morton Street Apt 14", AddressSecondLine = "New York City", AddressThirdLine = "New York", AddressPostCode = "EN7 1J[", PhoneNumber1 = "07023122843", EmailAddress = "Phoebe.Buffay@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Al", MiddleNames = "", LastName = "Bundy", AddressFirstLine = "9764 Jeopardy Lane", AddressSecondLine = "Chicago", AddressThirdLine = "Illinois", AddressPostCode = "EN8 4XN", PhoneNumber1 = "07476938319", EmailAddress = "Al.Bundy@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Archie", MiddleNames = "", LastName = "Bunker", AddressFirstLine = "704 Hauser Street", AddressSecondLine = "New York City", AddressThirdLine = "New York", AddressPostCode = "EN11 1MF", PhoneNumber1 = "07563904839", EmailAddress = "Archie.Bunker@hotmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Edna", MiddleNames = "", LastName = "Burber", AddressFirstLine = "9 Chickweed Lane", AddressSecondLine = "", AddressThirdLine = "", AddressPostCode = "EN1 3ID", PhoneNumber1 = "07880250846", EmailAddress = "Edna.Burber@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Montgomery", MiddleNames = "", LastName = "Burns", AddressFirstLine = "1000 Mammon Lane", AddressSecondLine = "Springfield", AddressThirdLine = "USA", AddressPostCode = "EN1 9UD", PhoneNumber1 = "07692970360", EmailAddress = "Montgomery.Burns@hotmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Minnie", MiddleNames = "", LastName = "Caldwell", AddressFirstLine = "15 Jubilee Terrace", AddressSecondLine = "Weatherfield", AddressThirdLine = "Greater Manchester", AddressPostCode = "EN7 1UO", PhoneNumber1 = "07653326663", EmailAddress = "Minnie.Caldwell@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Drew", MiddleNames = "", LastName = "Carey", AddressFirstLine = "720 Sedgewick", AddressSecondLine = "Cleveland", AddressThirdLine = "Ohio", AddressPostCode = "EN4 6RX", PhoneNumber1 = "07601829305", EmailAddress = "Drew.Carey@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Blake", MiddleNames = "", LastName = "Carrington", AddressFirstLine = "173 Essex Drive", AddressSecondLine = "Denver", AddressThirdLine = "CO", AddressPostCode = "EN2 1DZ", PhoneNumber1 = "07311619161", EmailAddress = "Blake.Carrington@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Sam", MiddleNames = "", LastName = "Malone", AddressFirstLine = "112 1/2 Beacon Street", AddressSecondLine = "Boston", AddressThirdLine = "Massachusetts", AddressPostCode = "EN6 9YO", PhoneNumber1 = "07668364417", EmailAddress = "Sam.Malone@hotmail.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "Elly", MiddleNames = "May", LastName = "Clampett", AddressFirstLine = "518 Crestview Drive", AddressSecondLine = "Beverly Hills", AddressThirdLine = "California", AddressPostCode = "EN5 1OB", PhoneNumber1 = "07556469623", EmailAddress = "Elly.Clampett@yahoo.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Joan", MiddleNames = "", LastName = "Clayton", AddressFirstLine = "700 Block North Wilton Place", AddressSecondLine = "Los Angeles", AddressThirdLine = "California", AddressPostCode = "EN6 1DU", PhoneNumber1 = "07586357495", EmailAddress = "Joan.Clayton@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Theodore", MiddleNames = "", LastName = "Cleaver", AddressFirstLine = "485 Maple Street", AddressSecondLine = "Mayfield", AddressThirdLine = "USA", AddressPostCode = "EN1 4[M", PhoneNumber1 = "07804415409", EmailAddress = "Theodore.Cleaver@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Jason", MiddleNames = "", LastName = "Colby", AddressFirstLine = "Belvedere Mansion", AddressSecondLine = "Los Angeles", AddressThirdLine = "California", AddressPostCode = "EN2 6YO", PhoneNumber1 = "07434740174", EmailAddress = "Jason.Colby@yahoo.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "Roseanne", MiddleNames = "", LastName = "Conner", AddressFirstLine = "714 Delaware", AddressSecondLine = "Lanford", AddressThirdLine = "IL", AddressPostCode = "EN6 9TO", PhoneNumber1 = "07995298668", EmailAddress = "Roseanne.Conner@yahoo.com" });
            li.Add(new Client() { Title = "Dr", FirstName = "Frasier", MiddleNames = "", LastName = "Crane", AddressFirstLine = "Elliott Bay Towers Apartment 1901", AddressSecondLine = "Seattle", AddressThirdLine = "Washington", AddressPostCode = "EN4 8SB", PhoneNumber1 = "07530196951", EmailAddress = "Frasier.Crane@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Richie", MiddleNames = "", LastName = "Cunningham", AddressFirstLine = "565 North Clinton Drive", AddressSecondLine = "Milwaukee", AddressThirdLine = "WI", AddressPostCode = "EN9 1IW", PhoneNumber1 = "07313106029", EmailAddress = "Richie.Cunningham@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Fitzwilliam", MiddleNames = "", LastName = "Darcy", AddressFirstLine = "Pemberley", AddressSecondLine = "Derbyshire", AddressThirdLine = "England", AddressPostCode = "EN11 5RD", PhoneNumber1 = "07883517250", EmailAddress = "Fitzwilliam.Darcy@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "", MiddleNames = "", LastName = "Dethklok", AddressFirstLine = "Mordhaus", AddressSecondLine = "", AddressThirdLine = "", AddressPostCode = "EN4 8TZ", PhoneNumber1 = "07060575796", EmailAddress = "Dethklok@yahoo.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Blanche", MiddleNames = "", LastName = "Devereaux", AddressFirstLine = "6151 Richmond Street", AddressSecondLine = "Miami", AddressThirdLine = "Florida", AddressPostCode = "EN5 7PP", PhoneNumber1 = "07834878986", EmailAddress = "Blanche.Devereaux@hotmail.com" });
            li.Add(new Client() { Title = "The", FirstName = "", MiddleNames = "", LastName = "Doctor", AddressFirstLine = "Smithwood Manor", AddressSecondLine = "Allen Road", AddressThirdLine = "Kent", AddressPostCode = "EN1 9MY", PhoneNumber1 = "07079817988", EmailAddress = "Doctor@hotmail.com" });
            li.Add(new Client() { Title = "Dr", FirstName = "John", MiddleNames = "", LastName = "Dolittle", AddressFirstLine = "Oxenthorpe Road", AddressSecondLine = "Puddleby-on-the-Marsh", AddressThirdLine = "Slopshire", AddressPostCode = "EN8 2UP", PhoneNumber1 = "07491283599", EmailAddress = "John.Dolittle@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Donald", MiddleNames = "", LastName = "Duck", AddressFirstLine = "1313 Webfoot Walk", AddressSecondLine = "Duckburg", AddressThirdLine = "Calisota", AddressPostCode = "EN2 6MP", PhoneNumber1 = "07083831918", EmailAddress = "Donald.Duck@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Vernon", MiddleNames = "", LastName = "Dursley", AddressFirstLine = "4 Privet Drive", AddressSecondLine = "Little Whinging", AddressThirdLine = "Surrey", AddressPostCode = "EN3 4GP", PhoneNumber1 = "07590563117", EmailAddress = "Vernon.Dursley@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Tyler", MiddleNames = "", LastName = "Durden", AddressFirstLine = "420 Paper St", AddressSecondLine = "Wilmington", AddressThirdLine = "DE 19886", AddressPostCode = "EN5 5HO", PhoneNumber1 = "07114359729", EmailAddress = "Tyler.Durden@yahoo.com" });
            li.Add(new Client() { Title = "Lord", FirstName = "", MiddleNames = "", LastName = "Emsworth", AddressFirstLine = "Blandings Castle", AddressSecondLine = "Shropshire", AddressThirdLine = "", AddressPostCode = "EN8 3WP", PhoneNumber1 = "07465028989", EmailAddress = "Emsworth@hotmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Florida", MiddleNames = "", LastName = "Evans", AddressFirstLine = "321 North Gilbert", AddressSecondLine = "Chicago", AddressThirdLine = "IL", AddressPostCode = "EN9 9DJ", PhoneNumber1 = "07324880848", EmailAddress = "Florida.Evans@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "JR", MiddleNames = "", LastName = "Ewing", AddressFirstLine = "Southfork Ranch", AddressSecondLine = "Braddock County", AddressThirdLine = "Texas", AddressPostCode = "EN10 9CW", PhoneNumber1 = "07782816647", EmailAddress = "JR.Ewing@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Reed", MiddleNames = "", LastName = "Richards", AddressFirstLine = "The Baxter Building", AddressSecondLine = "New York", AddressThirdLine = "New York", AddressPostCode = "EN5 2NI", PhoneNumber1 = "07462311145", EmailAddress = "Reed.Richards@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Basil", MiddleNames = "", LastName = "Fawlty", AddressFirstLine = "Fawlty Towers Hotel", AddressSecondLine = "Torquay", AddressThirdLine = "Torbay", AddressPostCode = "EN5 7JR", PhoneNumber1 = "07346167389", EmailAddress = "Basil.Fawlty@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Ned", MiddleNames = "", LastName = "Flanders", AddressFirstLine = "740 Evergreen Terrace", AddressSecondLine = "Springfield", AddressThirdLine = "", AddressPostCode = "EN4 7SJ", PhoneNumber1 = "07322451193", EmailAddress = "Ned.Flanders@gmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Jessica", MiddleNames = "", LastName = "Fletcher", AddressFirstLine = "698 Candlewood Lane", AddressSecondLine = "Cabot Cove", AddressThirdLine = "ME", AddressPostCode = "EN1 1XZ", PhoneNumber1 = "07764948142", EmailAddress = "Jessica.Fletcher@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Phileas", MiddleNames = "", LastName = "Fogg", AddressFirstLine = "7 Savile Row", AddressSecondLine = "Burlington Gardens", AddressThirdLine = "London", AddressPostCode = "EN7 2OZ", PhoneNumber1 = "07690273549", EmailAddress = "Phileas.Fogg@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Doug", MiddleNames = "", LastName = "Funnie", AddressFirstLine = "21 Jumbo Street", AddressSecondLine = "Bluffington", AddressThirdLine = "USA", AddressPostCode = "EN6 1XU", PhoneNumber1 = "07162453621", EmailAddress = "Doug.Funnie@yahoo.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "Monica", MiddleNames = "", LastName = "Geller", AddressFirstLine = "425 Grove Street Apartment 20", AddressSecondLine = "New York", AddressThirdLine = "New York", AddressPostCode = "EN2 9EV", PhoneNumber1 = "07401122738", EmailAddress = "Monica.Geller@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Peter", MiddleNames = "", LastName = "Griffin", AddressFirstLine = "31 Spooner Street", AddressSecondLine = "Quahog", AddressThirdLine = "Rhode Island", AddressPostCode = "EN1 4SG", PhoneNumber1 = "07040801568", EmailAddress = "Peter.Griffin@yahoo.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "Prue", MiddleNames = "", LastName = "Halliwell", AddressFirstLine = "1329 Prescott Street", AddressSecondLine = "San Francisco", AddressThirdLine = "California", AddressPostCode = "EN6 6FC", PhoneNumber1 = "07805980192", EmailAddress = "Prue.Halliwell@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Tony", MiddleNames = "", LastName = "Hancock", AddressFirstLine = "23 Railway Cuttings", AddressSecondLine = "East Cheam", AddressThirdLine = "", AddressPostCode = "EN10 9FS", PhoneNumber1 = "07120121664", EmailAddress = "Tony.Hancock@gmail.com" });
            li.Add(new Client() { Title = "Sir", FirstName = "Richard", MiddleNames = "", LastName = "Hannay", AddressFirstLine = "Fosse Manor", AddressSecondLine = "Oxfordshire", AddressThirdLine = "", AddressPostCode = "EN2 6H[", PhoneNumber1 = "07057853450", EmailAddress = "Richard.Hannay@yahoo.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Thelma", MiddleNames = "", LastName = "Harper", AddressFirstLine = "1 Old Decatur Road", AddressSecondLine = "Raytown", AddressThirdLine = "USA", AddressPostCode = "EN7 5NT", PhoneNumber1 = "07925198482", EmailAddress = "Thelma.Harper@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Farley", MiddleNames = "", LastName = "Hatcher", AddressFirstLine = "25 West 68th Street", AddressSecondLine = "New York", AddressThirdLine = "New York", AddressPostCode = "EN1 2VU", PhoneNumber1 = "07651002960", EmailAddress = "Farley.Hatcher@yahoo.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Carrie", MiddleNames = "", LastName = "Heffernan", AddressFirstLine = "3223 Aberdeen Avenue", AddressSecondLine = "Rego Park", AddressThirdLine = "New York", AddressPostCode = "EN5 1KK", PhoneNumber1 = "07881824741", EmailAddress = "Carrie.Heffernan@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Earl", MiddleNames = "", LastName = "Hickey", AddressFirstLine = "Room 231 The Palms Motel", AddressSecondLine = "9005 Lincoln Blvd", AddressThirdLine = "Camden", AddressPostCode = "EN4 7EN", PhoneNumber1 = "07194243016", EmailAddress = "Earl.Hickey@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Hank", MiddleNames = "", LastName = "Hill", AddressFirstLine = "84 Rainey Street", AddressSecondLine = "Arlen", AddressThirdLine = "TX", AddressPostCode = "EN7 5OX", PhoneNumber1 = "07703264900", EmailAddress = "Hank.Hill@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Sherlock", MiddleNames = "", LastName = "Holmes", AddressFirstLine = "221B Baker Street", AddressSecondLine = "London", AddressThirdLine = "UK", AddressPostCode = "EN1 5VM", PhoneNumber1 = "07204499266", EmailAddress = "Sherlock.Holmes@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Cliff", MiddleNames = "", LastName = "Huxtable", AddressFirstLine = "10 Stigwood Avenue", AddressSecondLine = "Brooklyn", AddressThirdLine = "New York", AddressPostCode = "EN11 8BR", PhoneNumber1 = "07800134638", EmailAddress = "Cliff.Huxtable@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Frederick", MiddleNames = "", LastName = "Twistleton", AddressFirstLine = "Ickenham Hall", AddressSecondLine = "Ickenham", AddressThirdLine = "Hants", AddressPostCode = "EN11 8VX", PhoneNumber1 = "07741808525", EmailAddress = "Frederick.Twistleton@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Richard", MiddleNames = "Henry", LastName = "Benson", AddressFirstLine = "No. 1 Bleek Street", AddressSecondLine = "", AddressThirdLine = "", AddressPostCode = "EN9 9FR", PhoneNumber1 = "07751791344", EmailAddress = "Richard.Benson@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Clark", MiddleNames = "", LastName = "Kent", AddressFirstLine = "344 Clinton St. Apt. 3B", AddressSecondLine = "Metropolis", AddressThirdLine = "USA", AddressPostCode = "EN5 5UZ", PhoneNumber1 = "07760597711", EmailAddress = "Clark.Kent@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Eugene", MiddleNames = "", LastName = "Krabs", AddressFirstLine = "3541 Anchor Way", AddressSecondLine = "Bikini Bottom", AddressThirdLine = "Pacific Ocean", AddressPostCode = "EN7 9VL", PhoneNumber1 = "07656953616", EmailAddress = "Eugene.Krabs@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Ralpha", MiddleNames = "", LastName = "Kramden", AddressFirstLine = "328 1/2 Chauncey Street", AddressSecondLine = "Brooklyn", AddressThirdLine = "New York", AddressPostCode = "EN3 8VF", PhoneNumber1 = "07840597350", EmailAddress = "Ralpha.Kramden@gmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Patty", MiddleNames = "", LastName = "Lane", AddressFirstLine = "8 Remsen Drive", AddressSecondLine = "Brooklyn Heights", AddressThirdLine = "New York City", AddressPostCode = "EN9 7CQ", PhoneNumber1 = "07401989996", EmailAddress = "Patty.Lane@hotmail.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "Laverne", MiddleNames = "", LastName = "DeFazio", AddressFirstLine = "730 Knapp Avenue", AddressSecondLine = "Milwaukee", AddressThirdLine = "Wisconsin", AddressPostCode = "EN1 4YD", PhoneNumber1 = "07824408796", EmailAddress = "Laverne.DeFazio@yahoo.com" });
            li.Add(new Client() { Title = "Professor", FirstName = "Russ", MiddleNames = "", LastName = "Lawrence", AddressFirstLine = "803 N. Dutton Drive", AddressSecondLine = "California", AddressThirdLine = "USA", AddressPostCode = "EN8 1VG", PhoneNumber1 = "07970817540", EmailAddress = "Russ.Lawrence@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Leopold", MiddleNames = "", LastName = "Bloom", AddressFirstLine = "7 Eccles Street", AddressSecondLine = "Dublin 7", AddressThirdLine = "Ireland", AddressPostCode = "EN2 2WP", PhoneNumber1 = "07417214630", EmailAddress = "Leopold.Bloom@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Thomas", MiddleNames = "", LastName = "Magnum", AddressFirstLine = "11435 18th Avenue", AddressSecondLine = "Oahu", AddressThirdLine = "HI", AddressPostCode = "EN4 5PK", PhoneNumber1 = "07077795789", EmailAddress = "Thomas.Magnum@yahoo.com" });
            li.Add(new Client() { Title = "Miss", FirstName = "Jane", MiddleNames = "", LastName = "Marple", AddressFirstLine = "Danemead", AddressSecondLine = "High Street", AddressThirdLine = "St Mary Mead", AddressPostCode = "EN7 5CE", PhoneNumber1 = "07143280047", EmailAddress = "Jane.Marple@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Victor", MiddleNames = "", LastName = "Meldrew", AddressFirstLine = "19 Riverbank", AddressSecondLine = "UK", AddressThirdLine = "", AddressPostCode = "EN9 6HE", PhoneNumber1 = "07069333858", EmailAddress = "Victor.Meldrew@yahoo.com" });
            li.Add(new Client() { Title = "Miss", FirstName = "Mindy", MiddleNames = "", LastName = "McConnell", AddressFirstLine = "1619 Pine Street", AddressSecondLine = "Boulder", AddressThirdLine = "Colorado", AddressPostCode = "EN8 4FE", PhoneNumber1 = "07951077772", EmailAddress = "Mindy.McConnell@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Fibber", MiddleNames = "", LastName = "McGee", AddressFirstLine = "79 Wistful Vista", AddressSecondLine = "USA.", AddressThirdLine = "", AddressPostCode = "EN1 8XG", PhoneNumber1 = "07927513830", EmailAddress = "Fibber.McGee@yahoo.com" });
            li.Add(new Client() { Title = "Miss", FirstName = "Daria", MiddleNames = "", LastName = "Morgendorffer", AddressFirstLine = "1111 Glen Oaks Lane", AddressSecondLine = "Lawndale USA", AddressThirdLine = "", AddressPostCode = "EN9 2UM", PhoneNumber1 = "07622617667", EmailAddress = "Daria.Morgendorffer@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Fox", MiddleNames = "", LastName = "Mulder", AddressFirstLine = "2630 Hegal Place Apt. 42", AddressSecondLine = "Alexandria", AddressThirdLine = "Virginia", AddressPostCode = "EN11 1ZL", PhoneNumber1 = "07163484672", EmailAddress = "Fox.Mulder@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Herman", MiddleNames = "", LastName = "Munster", AddressFirstLine = "1313 Mockingbird Lane", AddressSecondLine = "Mockingbird Heights", AddressThirdLine = "USA", AddressPostCode = "EN8 5WN", PhoneNumber1 = "07816550811", EmailAddress = "Herman.Munster@yahoo.com" });
            li.Add(new Client() { Title = "Captain", FirstName = "Tony", MiddleNames = "", LastName = "Nelson", AddressFirstLine = "1020 Palm Drive", AddressSecondLine = "Cocoa Beach", AddressThirdLine = "FL", AddressPostCode = "EN1 2DU", PhoneNumber1 = "07954649213", EmailAddress = "Tony.Nelson@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Will", MiddleNames = "", LastName = "Navidson", AddressFirstLine = "Succoth and Ash Tree Lane", AddressSecondLine = "", AddressThirdLine = "", AddressPostCode = "EN3 4QI", PhoneNumber1 = "07624331050", EmailAddress = "Will.Navidson@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "", MiddleNames = "", LastName = "Newman", AddressFirstLine = "129 West 81st Street Apartment 5E", AddressSecondLine = "New York", AddressThirdLine = "New York", AddressPostCode = "EN8 9VN", PhoneNumber1 = "07853383002", EmailAddress = "Newman@hotmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Hilda", MiddleNames = "", LastName = "Ogden", AddressFirstLine = "13 Coronation Street", AddressSecondLine = "Weatherfield", AddressThirdLine = "Greater Manchester", AddressPostCode = "EN2 7LJ", PhoneNumber1 = "07307189560", EmailAddress = "Hilda.Ogden@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Oscar", MiddleNames = "", LastName = "Madison", AddressFirstLine = "1049 Park Avenue", AddressSecondLine = "New York", AddressThirdLine = "New York", AddressPostCode = "EN10 6GH", PhoneNumber1 = "07242316104", EmailAddress = "Oscar.Madison@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "George", MiddleNames = "", LastName = "Owens", AddressFirstLine = "200 Spring Valley Road", AddressSecondLine = "Beaver Falls", AddressThirdLine = "PA", AddressPostCode = "EN4 6VD", PhoneNumber1 = "07847559096", EmailAddress = "George.Owens@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Paddington", MiddleNames = "", LastName = "Bear", AddressFirstLine = "32 Windsor Gardens", AddressSecondLine = "London", AddressThirdLine = "", AddressPostCode = "EN9 2LN", PhoneNumber1 = "07866164791", EmailAddress = "Paddington.Bear@gmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Shirley", MiddleNames = "", LastName = "Partridge", AddressFirstLine = "698 Sycamore Road", AddressSecondLine = "San Pueblo", AddressThirdLine = "CA", AddressPostCode = "EN7 6NI", PhoneNumber1 = "07644486081", EmailAddress = "Shirley.Partridge@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Reginald", MiddleNames = "Iolanthe", LastName = "Perrin", AddressFirstLine = "12 Coleridge Close", AddressSecondLine = "Climthorpe", AddressThirdLine = "London", AddressPostCode = "EN9 7CO", PhoneNumber1 = "07743717926", EmailAddress = "Reginald.Perrin@yahoo.com" });
            li.Add(new Client() { Title = "Miss", FirstName = "Adrianna", MiddleNames = "", LastName = "Pennino", AddressFirstLine = "1818 Tusculum St", AddressSecondLine = "Philadelphia", AddressThirdLine = "PA 19134-3416", AddressPostCode = "EN4 4XQ", PhoneNumber1 = "07485718370", EmailAddress = "Adrianna.Pennino@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Robert", MiddleNames = "Simpson", LastName = "Petrie", AddressFirstLine = "448 Bonnie Meadow Road", AddressSecondLine = "New Rochelle", AddressThirdLine = "New York", AddressPostCode = "EN4 6PX", PhoneNumber1 = "07408852556", EmailAddress = "Robert.Petrie@hotmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Sophia", MiddleNames = "", LastName = "Petrillo", AddressFirstLine = "6151 Richmond Street", AddressSecondLine = "Miami Beach", AddressThirdLine = "Florida", AddressPostCode = "EN5 3IG", PhoneNumber1 = "07045046724", EmailAddress = "Sophia.Petrillo@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "", MiddleNames = "", LastName = "Phil", AddressFirstLine = "1595 I Street NW", AddressSecondLine = "Washington", AddressThirdLine = "DC", AddressPostCode = "EN8 6PB", PhoneNumber1 = "07205923720", EmailAddress = "Phil@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Sam", MiddleNames = "", LastName = "Pickles", AddressFirstLine = "1 Cloud Street", AddressSecondLine = "West Leederville", AddressThirdLine = "Perth", AddressPostCode = "EN4 6GN", PhoneNumber1 = "07582501667", EmailAddress = "Sam.Pickles@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Hercule", MiddleNames = "", LastName = "Poirot", AddressFirstLine = "Whitehaven Mansions Apt 56B", AddressSecondLine = "Sandhurst Square", AddressThirdLine = "London", AddressPostCode = "EN5 9BE", PhoneNumber1 = "07399115188", EmailAddress = "Hercule.Poirot@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Solar", MiddleNames = "", LastName = "Pons", AddressFirstLine = "71B Praed Street", AddressSecondLine = "London", AddressThirdLine = "UK", AddressPostCode = "EN1 2TB", PhoneNumber1 = "07033808306", EmailAddress = "Solar.Pons@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Wilbur", MiddleNames = "", LastName = "Post", AddressFirstLine = "17230 Valley Road", AddressSecondLine = "USA", AddressThirdLine = "", AddressPostCode = "EN5 5TL", PhoneNumber1 = "07558402197", EmailAddress = "Wilbur.Post@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Harry", MiddleNames = "", LastName = "Potter", AddressFirstLine = "The Cupboard under the Stairs", AddressSecondLine = "4 Privet Drive", AddressThirdLine = "Little Whinging", AddressPostCode = "EN8 9BL", PhoneNumber1 = "07343448843", EmailAddress = "Harry.Potter@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Screech", MiddleNames = "", LastName = "Powers", AddressFirstLine = "88 Edgemont", AddressSecondLine = "Palisades", AddressThirdLine = "California", AddressPostCode = "EN9 6[T", PhoneNumber1 = "07518973578", EmailAddress = "Screech.Powers@gmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Lucy", MiddleNames = "", LastName = "Ricardo", AddressFirstLine = "623 East 68th Street Apt 4A", AddressSecondLine = "New York", AddressThirdLine = "New York", AddressPostCode = "EN1 9TN", PhoneNumber1 = "07767320411", EmailAddress = "Lucy.Ricardo@gmail.com" });
            li.Add(new Client() { Title = "Miss", FirstName = "Mary", MiddleNames = "", LastName = "Richards", AddressFirstLine = "Apartment D 119 North Weatherly Avenue", AddressSecondLine = "Minneapolis", AddressThirdLine = "MN", AddressPostCode = "EN10 2VZ", PhoneNumber1 = "07988602874", EmailAddress = "Mary.Richards@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Jim", MiddleNames = "", LastName = "Rockford", AddressFirstLine = "2354 Pacific Coast Highway", AddressSecondLine = "California", AddressThirdLine = "USA", AddressPostCode = "EN2 5[G", PhoneNumber1 = "07459505588", EmailAddress = "Jim.Rockford@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Dudley", MiddleNames = "", LastName = "Rush", AddressFirstLine = "Highgate Avenue", AddressSecondLine = "Highgate", AddressThirdLine = "", AddressPostCode = "EN6 6DI", PhoneNumber1 = "07767492333", EmailAddress = "Dudley.Rush@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Lamont", MiddleNames = "", LastName = "Sanford", AddressFirstLine = "9114 S. Central Ave.", AddressSecondLine = "Watts", AddressThirdLine = "Los Angeles", AddressPostCode = "EN5 7PF", PhoneNumber1 = "07992303696", EmailAddress = "Lamont.Sanford@gmail.com" });
            li.Add(new Client() { Title = "Dr", FirstName = "Clark", MiddleNames = "", LastName = "Savage", AddressFirstLine = "86th floor Empire State Building", AddressSecondLine = "New York", AddressThirdLine = "New York", AddressPostCode = "EN10 5LX", PhoneNumber1 = "07491600802", EmailAddress = "Clark.Savage@gmail.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "Dana", MiddleNames = "", LastName = "Scully", AddressFirstLine = "3170 W. 53 Rd Apt 35", AddressSecondLine = "Annapolis", AddressThirdLine = "Maryland", AddressPostCode = "EN7 9NV", PhoneNumber1 = "07916273165", EmailAddress = "Dana.Scully@yahoo.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Lynette", MiddleNames = "", LastName = "Scavo", AddressFirstLine = "4355 Wisteria Lane", AddressSecondLine = "Fairview", AddressThirdLine = "Eagle State", AddressPostCode = "EN7 2HR", PhoneNumber1 = "07448526887", EmailAddress = "Lynette.Scavo@hotmail.com" });
            li.Add(new Client() { Title = "Dr", FirstName = "Jason", MiddleNames = "", LastName = "Seaver", AddressFirstLine = "15 Robin Hood Lane", AddressSecondLine = "Huntington", AddressThirdLine = "Long Island", AddressPostCode = "EN7 5MN", PhoneNumber1 = "07215690918", EmailAddress = "Jason.Seaver@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Jerry", MiddleNames = "", LastName = "Seinfeld", AddressFirstLine = "129 West 81st Street Apartment 5A", AddressSecondLine = "New York", AddressThirdLine = "New York", AddressPostCode = "EN4 6JW", PhoneNumber1 = "07705769042", EmailAddress = "Jerry.Seinfeld@hotmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Angelica", MiddleNames = "", LastName = "Serralde", AddressFirstLine = "Heriberto Frias 15", AddressSecondLine = "Mexico City.", AddressThirdLine = "", AddressPostCode = "EN4 7ZB", PhoneNumber1 = "07671813724", EmailAddress = "Angelica.Serralde@gmail.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "", MiddleNames = "", LastName = "Sethe", AddressFirstLine = "124 Bluestone Road", AddressSecondLine = "Cincinnati", AddressThirdLine = "OH", AddressPostCode = "EN7 5XE", PhoneNumber1 = "07787572268", EmailAddress = "Sethe@hotmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Ena", MiddleNames = "", LastName = "Sharples", AddressFirstLine = "16 Coronation Street", AddressSecondLine = "Weatherfield", AddressThirdLine = "Greater Manchester", AddressPostCode = "EN9 7BC", PhoneNumber1 = "07622536535", EmailAddress = "Ena.Sharples@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Homer", MiddleNames = "", LastName = "Simpson", AddressFirstLine = "742 Evergreen Terrace", AddressSecondLine = "Springfield", AddressThirdLine = "USA", AddressPostCode = "EN2 8XO", PhoneNumber1 = "07968957421", EmailAddress = "Homer.Simpson@hotmail.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Marge", MiddleNames = "", LastName = "Simpson", AddressFirstLine = "15201 Maple Systems Road", AddressSecondLine = "Cypress Creek", AddressThirdLine = "USA", AddressPostCode = "EN6 8YY", PhoneNumber1 = "07549286613", EmailAddress = "Marge.Simpson@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Tony", MiddleNames = "", LastName = "Soprano", AddressFirstLine = "633 Stag Trail Road", AddressSecondLine = "North Caldwell", AddressThirdLine = "New Jersey", AddressPostCode = "EN8 1K[", PhoneNumber1 = "07123359041", EmailAddress = "Tony.Soprano@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "SpongeBob", MiddleNames = "", LastName = "SquarePants", AddressFirstLine = "124 Conch Street", AddressSecondLine = "Bikini Bottom", AddressThirdLine = "Pacific Ocean", AddressPostCode = "EN3 4QB", PhoneNumber1 = "07904538785", EmailAddress = "SpongeBob.SquarePants@gmail.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "Sabrina", MiddleNames = "", LastName = "Spellman", AddressFirstLine = "133 Collins Road", AddressSecondLine = "Westbridge", AddressThirdLine = "USA", AddressPostCode = "EN2 6[W", PhoneNumber1 = "07683905062", EmailAddress = "Sabrina.Spellman@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Patrick", MiddleNames = "", LastName = "Star", AddressFirstLine = "120 Conch Street", AddressSecondLine = "Bikini Bottom", AddressThirdLine = "Pacific Ocean", AddressPostCode = "EN5 7WC", PhoneNumber1 = "07844692331", EmailAddress = "Patrick.Star@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Michael", MiddleNames = "", LastName = "Steadman", AddressFirstLine = "1710 Bryn Mawr Avenue", AddressSecondLine = "Philadelphia", AddressThirdLine = "PA", AddressPostCode = "EN7 4JB", PhoneNumber1 = "07261880541", EmailAddress = "Michael.Steadman@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Adam", MiddleNames = "", LastName = "Stephens", AddressFirstLine = "1164 Morning Glory Circle", AddressSecondLine = "Westport", AddressThirdLine = "CT", AddressPostCode = "EN1 4[X", PhoneNumber1 = "07601140192", EmailAddress = "Adam.Stephens@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Harold", MiddleNames = "", LastName = "Steptoe", AddressFirstLine = "24 Oil Drum Lane", AddressSecondLine = "Shepherd's Bush", AddressThirdLine = "", AddressPostCode = "EN2 2PV", PhoneNumber1 = "07911728719", EmailAddress = "Harold.Steptoe@yahoo.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Donna", MiddleNames = "", LastName = "Stone", AddressFirstLine = "4-3926 Hillsdale", AddressSecondLine = "", AddressThirdLine = "", AddressPostCode = "EN7 8[N", PhoneNumber1 = "07730229741", EmailAddress = "Donna.Stone@yahoo.com" });
            li.Add(new Client() { Title = "Dr", FirstName = "", MiddleNames = "", LastName = "Strange", AddressFirstLine = "177A Bleecker Street", AddressSecondLine = "Greenwich Village", AddressThirdLine = "New York", AddressPostCode = "EN8 4FJ", PhoneNumber1 = "07370236335", EmailAddress = "Strange@hotmail.com" });
            li.Add(new Client() { Title = "Miss", FirstName = "Buffy", MiddleNames = "", LastName = "Summers", AddressFirstLine = "1630 Revello Drive", AddressSecondLine = "Sunnydale", AddressThirdLine = "CA", AddressPostCode = "EN5 9VG", PhoneNumber1 = "07170340931", EmailAddress = "Buffy.Summers@yahoo.com" });
            li.Add(new Client() { Title = "Ms", FirstName = "Lois", MiddleNames = "", LastName = "Lane", AddressFirstLine = "1938 Sulivan Lane", AddressSecondLine = "Metropolis", AddressThirdLine = "USA", AddressPostCode = "EN11 8[G", PhoneNumber1 = "07954206181", EmailAddress = "Lois.Lane@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Willie", MiddleNames = "", LastName = "Tanner", AddressFirstLine = "167 Hemdale Street", AddressSecondLine = "Los Angeles", AddressThirdLine = "California", AddressPostCode = "EN8 2RL", PhoneNumber1 = "07148149972", EmailAddress = "Willie.Tanner@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Danny", MiddleNames = "", LastName = "Tanner", AddressFirstLine = "1882 Gerard Street", AddressSecondLine = "San Francisco", AddressThirdLine = "California", AddressPostCode = "EN11 8ZD", PhoneNumber1 = "07278538264", EmailAddress = "Danny.Tanner@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Albert", MiddleNames = "", LastName = "Tatlock", AddressFirstLine = "1 Coronation Street", AddressSecondLine = "Weatherfield", AddressThirdLine = "Greater Manchester", AddressPostCode = "EN6 8XJ", PhoneNumber1 = "07948028779", EmailAddress = "Albert.Tatlock@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Andy", MiddleNames = "", LastName = "Taylor", AddressFirstLine = "322 Maple", AddressSecondLine = "Mayberry", AddressThirdLine = "North Carolina", AddressPostCode = "EN1 5WB", PhoneNumber1 = "07707315429", EmailAddress = "Andy.Taylor@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Tim", MiddleNames = "", LastName = "Taylor", AddressFirstLine = "510 Glenview", AddressSecondLine = "Detroit", AddressThirdLine = "Michigan", AddressPostCode = "EN9 7LH", PhoneNumber1 = "07793047876", EmailAddress = "Tim.Taylor@yahoo.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Etheline", MiddleNames = "", LastName = "Tenenbaum", AddressFirstLine = "111 Archer Avenue", AddressSecondLine = "New York City", AddressThirdLine = "New York", AddressPostCode = "EN7 5FB", PhoneNumber1 = "07341503213", EmailAddress = "Etheline.Tenenbaum@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Squidward", MiddleNames = "", LastName = "Tentacles", AddressFirstLine = "122 Conch Street", AddressSecondLine = "Bikini Bottom", AddressThirdLine = "Pacific Ocean", AddressPostCode = "EN7 5RR", PhoneNumber1 = "07302937926", EmailAddress = "Squidward.Tentacles@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Eric", MiddleNames = "", LastName = "Thursley", AddressFirstLine = "13 Midden Lane", AddressSecondLine = "Pseudopolis", AddressThirdLine = "Sto Plains", AddressPostCode = "EN7 2DG", PhoneNumber1 = "07485201957", EmailAddress = "Eric.Thursley@yahoo.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Dahlia", MiddleNames = "", LastName = "Travers", AddressFirstLine = "47 Charles Street", AddressSecondLine = "Mayfair", AddressThirdLine = "London", AddressPostCode = "EN5 7EZ", PhoneNumber1 = "07199025132", EmailAddress = "Dahlia.Travers@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Jack", MiddleNames = "", LastName = "Tripper", AddressFirstLine = "834 Ocean Vista Avenue #201", AddressSecondLine = "Santa Monica", AddressThirdLine = "California", AddressPostCode = "EN6 2WV", PhoneNumber1 = "07810309101", EmailAddress = "Jack.Tripper@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Rodney", MiddleNames = "", LastName = "Trotter", AddressFirstLine = "Nelson Mandela House", AddressSecondLine = "Peckham", AddressThirdLine = "London", AddressPostCode = "EN11 8[V", PhoneNumber1 = "07675502138", EmailAddress = "Rodney.Trotter@yahoo.com" });
            li.Add(new Client() { Title = "Mrs", FirstName = "Joy", MiddleNames = "", LastName = "Turner", AddressFirstLine = "Pimmit Hills Trailer Park Space C-13", AddressSecondLine = "Camden", AddressThirdLine = "USA", AddressPostCode = "EN6 8IZ", PhoneNumber1 = "07013964602", EmailAddress = "Joy.Turner@yahoo.com" });
            li.Add(new Client() { Title = "Miss", FirstName = "Rose", MiddleNames = "", LastName = "Tyler", AddressFirstLine = "Flat 48 Bucknall House", AddressSecondLine = "Powell Estate", AddressThirdLine = "London", AddressPostCode = "EN2 7LF", PhoneNumber1 = "07328689410", EmailAddress = "Rose.Tyler@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Samuel", MiddleNames = "", LastName = "Vimes", AddressFirstLine = "Ramkin Manor", AddressSecondLine = "Scoone Avenue", AddressThirdLine = "Ankh-Morpork", AddressPostCode = "EN5 7QG", PhoneNumber1 = "07518500027", EmailAddress = "Samuel.Vimes@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "", MiddleNames = "", LastName = "Wallace", AddressFirstLine = "62 West Wallaby Street", AddressSecondLine = "Wigan", AddressThirdLine = "Lancs", AddressPostCode = "EN11 6WN", PhoneNumber1 = "07158799612", EmailAddress = "Wallace@hotmail.com" });
            li.Add(new Client() { Title = "Miss", FirstName = "Brenda", MiddleNames = "", LastName = "Walsh", AddressFirstLine = "933 Hillcrest Drive", AddressSecondLine = "Beverly Hills", AddressThirdLine = "California", AddressPostCode = "EN2 3NK", PhoneNumber1 = "07985560778", EmailAddress = "Brenda.Walsh@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "", MiddleNames = "", LastName = "Waluigi", AddressFirstLine = "12 Grimace Lane", AddressSecondLine = "Mushroom Kingdom", AddressThirdLine = "", AddressPostCode = "EN9 6ZH", PhoneNumber1 = "07399519151", EmailAddress = "Waluigi@gmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "George", MiddleNames = "", LastName = "Weasley", AddressFirstLine = "93 Diagon Alley", AddressSecondLine = "London", AddressThirdLine = "", AddressPostCode = "EN11 6JU", PhoneNumber1 = "07363806752", EmailAddress = "George.Weasley@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Hal", MiddleNames = "", LastName = "Wilkerson", AddressFirstLine = "12334 Maple Blvd", AddressSecondLine = "", AddressThirdLine = "", AddressPostCode = "EN6 4NT", PhoneNumber1 = "07594782262", EmailAddress = "Hal.Wilkerson@yahoo.com" });
            li.Add(new Client() { Title = "Lord", FirstName = "Peter", MiddleNames = "", LastName = "Wimsey", AddressFirstLine = "110a Piccadilly", AddressSecondLine = "London", AddressThirdLine = "", AddressPostCode = "EN3 7OX", PhoneNumber1 = "07854963766", EmailAddress = "Peter.Wimsey@gmail.com" });
            li.Add(new Client() { Title = "Major", FirstName = "Charles", MiddleNames = "Emerson", LastName = "Winchester", AddressFirstLine = "16 Briarcliff Lane", AddressSecondLine = "Boston", AddressThirdLine = "Massachusetts", AddressPostCode = "EN8 9VS", PhoneNumber1 = "07159281441", EmailAddress = "Charles.Winchester@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Kenny", MiddleNames = "", LastName = "Madison", AddressFirstLine = "Surfside 6", AddressSecondLine = "Miami Beach", AddressThirdLine = "", AddressPostCode = "EN2 9[Z", PhoneNumber1 = "07449285666", EmailAddress = "Kenny.Madison@hotmail.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Carl", MiddleNames = "", LastName = "Winslow", AddressFirstLine = "263 Pinehurst", AddressSecondLine = "Chicago", AddressThirdLine = "Illinois", AddressPostCode = "EN4 4RR", PhoneNumber1 = "07626812099", EmailAddress = "Carl.Winslow@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Nero", MiddleNames = "", LastName = "Wolfe", AddressFirstLine = "454 West 35th Street", AddressSecondLine = "New York", AddressThirdLine = "New York", AddressPostCode = "EN5 6BY", PhoneNumber1 = "07039913050", EmailAddress = "Nero.Wolfe@hotmail.com" });
            li.Add(new Client() { Title = "Professor", FirstName = "Charles", MiddleNames = "", LastName = "Xavier", AddressFirstLine = "1407 Graymalkin Lane", AddressSecondLine = "Salem Center", AddressThirdLine = "New York", AddressPostCode = "EN11 1QG", PhoneNumber1 = "07330233072", EmailAddress = "Charles.Xavier@yahoo.com" });
            li.Add(new Client() { Title = "Mr", FirstName = "Anton", MiddleNames = "", LastName = "Zarnak", AddressFirstLine = "13 China Ave", AddressSecondLine = "", AddressThirdLine = "", AddressPostCode = "EN3 8GW", PhoneNumber1 = "07257830704", EmailAddress = "Anton.Zarnak@gmail.com" });
            return li;
        }

        private static readonly string[] roadNames =
            new string[]
            {
                    "Old Park Ridings",
                    "Old Park Avenue",
                    "Firs Avenue",
                    "Wellington Road",
                    "Baker Street",
                    "Station Road",
                    "High Road",
                    "New Street",
                    "Church Street",
                    "Great Portland Street",
                    "Oxford Street",
                    "Cecil Road",
                    "Grange Park Avenue",
                    "Park Drive",
                    "Crossways",
                    "Wells Street",
                    "Trinity Avenue",
                    "Mortimer Road",
                    "Store Street",
                    "Haileybury Crescent",
                    "Wishbone Road",
                    "Green Lanes",
                    "Itoje Road",
                    "Vardy Street",
                    "Mahrez Avenue",
                    "Hamilton Crescent",
                    "Roberts Green",
                    "North Road",
                    "Thompson Avenue",
                    "Bale Road",
                    "Murray Street",
                    "Harrington Street"
            };
        private static readonly string[] neighbourhoods =
            new string[]
            {
                    "",
                    "Enfield",
                    "Barnet",
                    "Potters Bar",
                    "Harringay",
                    "Wood Green",
            };
        private static readonly string[] postCodes =
            new string[]
            {
                    "EN1",
                    "EN2",
                    "EN3",
                    "EN4",
                    "EN5",
                    "EN6",
                    "EN7",
                    "EN8",
                    "EN9",
                    "EN10",
                    "EN11",
            };

        private static Job MakeFakeJob(int clientId, Random random)
        {
            var firstLine = (random.Next(500) + 1) + " " + roadNames[random.Next(roadNames.Length)];
            var secondLine = neighbourhoods[random.Next(neighbourhoods.Length)];
            var postCode = postCodes[random.Next(postCodes.Length)] + " " + random.Next(10) + ((char)(random.Next(26) + 65)) + ((char)(random.Next(26) + 65));
            return new Job()
            {
                ClientId = clientId,
                AddressFirstLine = firstLine,
                AddressSecondLine = secondLine,
                AddressPostCode = postCode
            };
        }

        public static void AddFakeJobs(DeadfileContext dbContext)
        {
            var random = new Random(64);
            foreach (var client in dbContext.Clients)
            {
                var numJobsToAdd = random.Next(3);
                for (int i = 0; i < numJobsToAdd; i++)
                {
                    dbContext.Jobs.Add(MakeFakeJob(client.ClientId, random));
                }
            }
        }

        private static int lastPaulSamsonInvoiceReference = 1000;
        private static int lastImagine3DInvoiceReference = 50;

        private static Dictionary<int, List<Tuple<Company, int>>> _addedInvoicesForJob = new Dictionary<int, List<Tuple<Company, int>>>();

        private static void AddSingleFakeInvoice(DeadfileContext dbContext, int clientId, int jobId, Random random)
        {
            var creationDate = new DateTime(2015, 1, 1).AddDays(random.Next(500));
            var company = (random.Next(2) == 0) ? Company.PaulSamsonCharteredSurveyorLtd : Company.Imagine3DLtd;
            int reference;
            if (company == Company.PaulSamsonCharteredSurveyorLtd)
                reference = lastPaulSamsonInvoiceReference++;
            else
                reference = lastImagine3DInvoiceReference++;
            var netAmount = (double)random.Next(100, 500);
            var invoice =
                new Invoice()
                    {
                        ClientId = clientId,
                        CreatedDate = creationDate,
                        Status = InvoiceStatus.Created,
                        Company = company,
                        InvoiceReference = reference,
                        GrossAmount = netAmount * (company == Company.PaulSamsonCharteredSurveyorLtd ? 1.2 : 1.0),
                        NetAmount = netAmount
                    };
            dbContext.Invoices.Add(invoice);
            _addedInvoicesForJob[jobId].Add(new Tuple<Company, int>(company, reference));
        }

        public static void AddFakeInvoices(DeadfileContext dbContext)
        {
            var random = new Random(354);
            foreach (var job in dbContext.Jobs)
            {
                var numInvoicesToAdd = random.Next(3);
                _addedInvoicesForJob.Add(job.JobId, new List<Tuple<Company, int>>());
                for (int i = 0; i < numInvoicesToAdd; i++)
                {
                    AddSingleFakeInvoice(dbContext, job.ClientId, job.JobId, random);
                }
            }
        }

        public static void AddFakeLocalAuthorities(DeadfileContext dbContext)
        {
            foreach (var localAuthority in GetFakeLocalAuthorities())
            {
                dbContext.LocalAuthorities.Add(localAuthority);
            }
        }

        public static void AddFakeApplications(DeadfileContext dbContext)
        {
            var random = new Random(712);
            var localAuthorities = (from la in dbContext.LocalAuthorities select la.Name).ToArray();
            foreach (var job in dbContext.Jobs)
            {
                var numApplicationsToAdd = random.Next(3);
                for (int i = 0; i < numApplicationsToAdd; i++)
                {
                    var r = random.Next(2);
                    var applicationType = (r == 0) ? ApplicationType.BuildingControlBuildingNotice : ApplicationType.FullPlanningPermission;
                    var creationDate = new DateTime(2015, 1, 1).AddDays(random.Next(500));
                    var localAuthority = localAuthorities[random.Next(localAuthorities.Length)];
                    var reference = random.Next(10000);
                    dbContext.Applications.Add(new Application()
                    {
                        CreationDate = creationDate,
                        JobId = job.JobId,
                        Type = applicationType,
                        LocalAuthority = localAuthority,
                        LocalAuthorityReference = localAuthority + "_" + reference
                    });
                }
            }
        }

        public static void AddFakeQuotations(DeadfileContext dbContext)
        {
            var homerSimpsonQuotations = new string[]
            {
                "Weaseling out of things is important to learn. It's what separates us from the animals... Except the weasel.",
                "Books are useless! I only ever read one book, To Kill A Mockingbird, and it gave me absolutely no insight on how to kill mockingbirds!",
                "Fame was like a drug. But what was even more like a drug were the drugs.",
                "Son, when you participate in sporting events, it’s not whether you win or lose: it’s how drunk you get.",
                "You don’t like your job, you don’t strike. You go in every day and do it really half-assed. That’s the American way.",
                "Facts are meaningless. You could use facts to prove anything that’s even remotely true!"
            };
            foreach (var homerSimpsonQuotation in homerSimpsonQuotations)
            {
                dbContext.Quotations.Add(new Quotation()
                {
                    Author = "Homer Simpson",
                    Phrase = homerSimpsonQuotation
                });
            }
            var ericCartmanQuotations = new string[]
            {
                "It's a man's obligation to stick his boneration in a woman's separation, this sort of penetration will increase the population of the younger generation.",
                "I'm not just sure. I'm HIV positive.",
                "Don't you know the first rule of physics? Anything that's fun costs at least eight dollars.",
                "Stan, your dog is a gay homosexual!"
            };
            foreach (var ericCartmanQuotation in ericCartmanQuotations)
            {
                dbContext.Quotations.Add(new Quotation()
                {
                    Author = "Eric Cartman",
                    Phrase = ericCartmanQuotation
                });
            }
        }



        public static IEnumerable<Application> GetFakeApplications()
        {
            var random = new Random(2135);
            var jobId = 17;
            var numApplicationsToAdd = 3;
            var li = new List<Application>();
            var localAuthorities = GetFakeLocalAuthorities().ToArray();
            for (int i = 0; i < numApplicationsToAdd; i++)
            {
                var r = random.Next(2);
                var applicationType = (r == 0) ? ApplicationType.BuildingControlBuildingNotice : ApplicationType.HouseholderPlanningPermission;
                var creationDate = new DateTime(2015, 1, 1).AddDays(random.Next(500));
                var localAuthority = localAuthorities[random.Next(localAuthorities.Length)];
                var reference = random.Next(10000);
                li.Add(new Application()
                {
                    CreationDate = creationDate,
                    JobId = jobId,
                    Type = applicationType,
                    LocalAuthority = localAuthority.Name,
                    LocalAuthorityReference = localAuthority + "_" + reference
                });
            }
            return li;
        }

        /// <summary>
        /// These are actually real but hey.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<LocalAuthority> GetFakeLocalAuthorities()
        {
            var li = new string[]
            {
                "Adur District Council",
                "Allerdale Borough Council",
                "Amber Valley Borough Council",
                "Arun District Council",
                "Ashfield District Council",
                "Ashford Borough Council",
                "Aylesbury Vale District Council",
                "Babergh District Council",
                "Barnet London Borough",
                "Barnsley Metropolitan Borough Council",
                "Barrow-in-Furness Borough Council",
                "Basildon District Council",
                "Basingstoke and Deane Borough Council",
                "Bassetlaw District Council",
                "Bath and North East Somerset Council",
                "Bedford Borough Council",
                "Birmingham City Council",
                "Blaby District Council",
                "Blackburn with Darwen Borough Council",
                "Blackpool Borough Council",
                "Blaenau Gwent County Borough Council",
                "Bolsover District Council",
                "Bolton Metropolitan Borough Council",
                "Borough of Broxbourne Council",
                "Borough of Poole",
                "Boston Borough Council",
                "Bournemouth Borough Council",
                "Bracknell Forest Borough Council",
                "Bradford Metropolitan District Council",
                "Braintree District Council",
                "Breckland District Council",
                "Brecon Beacons National Park",
                "Brent London Borough Council",
                "Brentwood Borough Council",
                "Bridgend County Borough Council",
                "Brighton and Hove City Council",
                "Bristol City Council",
                "Broadland District Council",
                "Broads Authority",
                "Bromsgrove District Council",
                "Broxtowe Borough Council",
                "Buckinghamshire County Council",
                "Burnley Borough Council",
                "Bury Metropolitan Borough Council",
                "Caerphilly County Borough Council",
                "Calderdale Metropolitan Borough Council",
                "Cambridge City Council",
                "Cambridgeshire County Council",
                "Camden Council",
                "Cannock Chase District Council",
                "Canterbury City Council",
                "Cardiff Council",
                "Carlisle City Council",
                "Carmarthenshire County Council",
                "Castle Point Borough Council",
                "Central Bedfordshire Council",
                "Ceredigion County  Council",
                "Charnwood Borough Council",
                "Chelmsford City Council",
                "Cheltenham Borough Council",
                "Cherwell District Council",
                "Cheshire East Council",
                "Cheshire West and Chester Council",
                "Chesterfield Borough Council",
                "Chichester District Council",
                "Chiltern District Council",
                "Chorley Borough Council",
                "Christchurch Borough Council",
                "City and County of Swansea",
                "City of London",
                "City of York Council",
                "Colchester Borough Council",
                "Conwy County Borough Council",
                "Copeland Borough Council",
                "Corby Borough Council",
                "Cornwall Council",
                "Cotswold District Council",
                "Coventry City Council",
                "Craven District Council",
                "Crawley Borough Council",
                "Croydon Council",
                "Cumbria County Council",
                "Cyngor Bwrdeistref Sirol Rhondda Cynon Taf",
                "Cyngor Sir Dinas a Sir Caerdydd",
                "Dacorum Borough Council",
                "Darlington Borough Council",
                "Dartford Borough Council",
                "Dartmoor National Park Authority",
                "Daventry District Council",
                "Denbighshire County Council",
                "Derby City Council",
                "Derbyshire County Council",
                "Derbyshire Dales District Council",
                "Devon County Council",
                "Doncaster Metropolitan Borough Council",
                "Dorset County Council",
                "Dover District Council",
                "Dudley Metropolitan Borough Council",
                "Durham County Council",
                "Ealing Council London Borough",
                "East Cambridgeshire District Council",
                "East Devon District Council",
                "East Dorset District Council",
                "East Hampshire District Council",
                "East Hertfordshire Council",
                "East Lindsey District Council",
                "East Northamptonshire District Council",
                "East Riding of Yorkshire Council",
                "East Staffordshire Borough Council",
                "East Sussex County Council",
                "Eastbourne Borough Council",
                "Eastleigh Borough Council",
                "Eden District Council",
                "Elmbridge Borough Council",
                "Enfield Council",
                "Epping Forest District Council",
                "Epsom and Ewell Borough Council",
                "Erewash Borough Council",
                "Essex County Council",
                "Exeter City Council",
                "Exmoor National Park",
                "Fareham Borough Council",
                "Fenland District Council",
                "Flintshire County Council",
                "Forest Heath District Council",
                "Forest of Dean District Council",
                "Fylde Borough Council",
                "Gateshead Metropolitan Borough Council",
                "Gedling Borough Council",
                "Gloucester City Council",
                "Gloucestershire County Council",
                "Gosport Borough Council",
                "Gravesham Borough Council",
                "Great Yarmouth Borough Council",
                "Guildford Borough Council",
                "Gwynedd Council",
                "Halton Borough Council",
                "Hambleton District Council",
                "Hampshire County Council",
                "Harborough District Council",
                "Haringey Council London Borough",
                "Harlow Council",
                "Harrogate Borough Council",
                "Harrow Council London Borough",
                "Hart District Council",
                "Hartlepool Borough Council",
                "Hastings Borough Council",
                "Havant Borough Council",
                "Herefordshire Council",
                "Hertfordshire County Council",
                "Hertsmere Borough Council",
                "High Peak Borough Council",
                "Hinckley and Bosworth Borough Council",
                "Horsham District Council",
                "Hounslow Council London Borough",
                "Hull City Council",
                "Huntingdonshire District Council",
                "Hyndburn Borough Council",
                "Ipswich Borough Council",
                "Isle Of Wight Council",
                "Isle of Anglesey County Council",
                "Isles of Scilly Council",
                "Islington Council",
                "Kent County Council",
                "Kettering Borough Council",
                "Kings Lynn and West Norfolk Borough Council",
                "Kingston upon Thames Royal Borough of",
                "Kirklees Metropolitan Council",
                "Knowsley Metropolitan Borough Council",
                "Lake District National Park",
                "Lambeth Council London Borough",
                "Lancashire County Council",
                "Lancaster City Council",
                "Leeds City Council",
                "Leicester City Council",
                "Leicestershire County Council",
                "Lewes District Council",
                "Lichfield District Council",
                "Lincoln City Council",
                "Lincolnshire County Council",
                "Liverpool City Council",
                "London Borough Of Bexley",
                "London Borough of Barking and Dagenham",
                "London Borough of Hackney",
                "London Borough of Havering",
                "London Borough of Hillingdon",
                "London Borough of Lewisham",
                "London Borough of Newham Council",
                "London Borough of Redbridge",
                "London Borough of Richmond upon Thames",
                "London Borough of Tower Hamlets",
                "London Legacy Development Corporation",
                "Luton Borough Council",
                "Maidstone Borough Council",
                "Maldon District Council",
                "Malvern Hills District Council",
                "Manchester City Council",
                "Mansfield District Council",
                "Medway Council",
                "Melton Borough Council",
                "Mendip District Council",
                "Merthyr Tydfil County Borough Council",
                "Merton Council London Borough",
                "Mid Suffolk District Council",
                "Mid Sussex District Council",
                "Middlesbrough Borough Council",
                "Milton Keynes Council",
                "Mole Valley District Council",
                "Monmouthshire County Council",
                "Neath Port Talbot County Borough Council",
                "New Forest District Council",
                "New Forest National Park",
                "Newark and Sherwood District Council",
                "Newcastle City Council",
                "Newcastle under Lyme Borough Council",
                "Newport City Council",
                "Norfolk County Council",
                "North Devon Council",
                "North Dorset District Council",
                "North East Derbyshire District Council",
                "North East Lincolnshire Council",
                "North Hertfordshire District Council",
                "North Kesteven District Council",
                "North Lincolnshire Council",
                "North Somerset Council",
                "North Tyneside Council",
                "North Warwickshire Borough Council",
                "North West Leicestershire District Council",
                "North York Moors National Park",
                "North Yorkshire County Council",
                "Northampton Borough Council",
                "Northamptonshire County Council",
                "Northumberland County Council",
                "Northumberland National Park Authority",
                "Norwich City Council",
                "Nottingham City Council",
                "Nottinghamshire County Council",
                "Nuneaton and Bedworth Borough Council",
                "Oadby and Wigston Borough Council",
                "Oldham Metropolitan Borough Council",
                "Oxford City Council",
                "Oxfordshire County Council",
                "Pembrokeshire Coast National Park",
                "Pembrokeshire County Council",
                "Pendle Borough Council",
                "Peterborough City Council",
                "Plymouth City Council",
                "Portsmouth City Council",
                "Powys County Council",
                "Preston City Council",
                "Purbeck District Council",
                "Reading Borough Council",
                "Redcar and Cleveland Borough Council",
                "Redditch Borough Council",
                "Reigate and Banstead Borough Council",
                "Rhondda Cynon Taf County Borough Council",
                "Ribble Valley Borough Council",
                "Rochdale Metropolitan Borough Council",
                "Rochford District Council",
                "Rossendale Borough Council",
                "Rother District Council",
                "Rotherham MBC",
                "Royal Borough of Greenwich",
                "Royal Borough of Windsor and Maidenhead",
                "Rugby Borough Council",
                "Runnymede Borough Council",
                "Rushcliffe Borough Council",
                "Rushmoor Borough Council",
                "Rutland County Council",
                "Ryedale District Council",
                "Salford City Council",
                "Sandwell Metropolitan Borough Council",
                "Scarborough Borough Council",
                "Sedgemoor District Council",
                "Sefton Council",
                "Sevenoaks District Council",
                "Sheffield City Council",
                "Shepway District Council",
                "Shropshire Council",
                "Slough Borough Council",
                "Snowdonia National Park",
                "Solihull Metropolitan Borough Council",
                "Somerset County Council",
                "South Bucks District Council",
                "South Cambridgeshire District Council",
                "South Derbyshire District Council",
                "South Downs National Park Authority",
                "South Downs National Park Authority (Adur DC)",
                "South Downs National Park Authority (Arun DC)",
                "South Downs National Park Authority (Brighton and Hove City Council)",
                "South Downs National Park Authority (Chichester DC)",
                "South Downs National Park Authority (East Hampshire DC)",
                "South Downs National Park Authority (East Sussex County Council)",
                "South Downs National Park Authority (Hampshire County Council)",
                "South Downs National Park Authority (Horsham DC)",
                "South Downs National Park Authority (Lewes DC)",
                "South Downs National Park Authority (Mid Sussex DC)",
                "South Downs National Park Authority (Wealden DC)",
                "South Downs National Park Authority (Winchester City Council)",
                "South Downs National Park Authority (Worthing BC)",
                "South Gloucestershire Council",
                "South Hams District Council",
                "South Holland District Council",
                "South Kesteven District Council",
                "South Lakeland District Council",
                "South Norfolk District Council",
                "South Northamptonshire Council",
                "South Oxfordshire District Council",
                "South Ribble Borough Council",
                "South Somerset District Council",
                "South Staffordshire Council",
                "Southampton City Council",
                "Southend-on-Sea Borough Council",
                "Southwark Council ",
                "Spelthorne Borough Council",
                "St Albans District Council",
                "St Edmundsbury Borough Council",
                "St Helens Council",
                "Stafford Borough Council",
                "Staffordshire County Council",
                "Staffordshire Moorlands District Council",
                "Stevenage Borough Council",
                "Stockport Metropolitan Borough Council",
                "Stockton-on-Tees Borough Council",
                "Stoke-on-Trent City Council",
                "Stratford on Avon District Council",
                "Stroud District Council",
                "Suffolk Coastal District Council",
                "Suffolk County Council",
                "Surrey County Council",
                "Surrey Heath Borough Council",
                "Sutton Council London Borough",
                "Swale Borough Council",
                "Swindon Borough Council",
                "Tameside Metropolitan Borough Council",
                "Tamworth Borough Council",
                "Tandridge District Council",
                "Taunton Deane Borough Council",
                "Teignbridge District Council",
                "Telford and Wrekin Council",
                "Tendring District Council",
                "Test Valley Borough Council",
                "Tewkesbury Borough Council",
                "Thanet District Council",
                "The London Borough of Bromley",
                "The London Borough of Hammersmith and Fulham",
                "The Royal Borough of Kensington and Chelsea",
                "Three Rivers District Council",
                "Thurrock Thames Gateway Development Corporation",
                "Tonbridge and Malling Borough Council",
                "Torbay Council",
                "Torfaen County Borough Council",
                "Torridge District Council",
                "Trafford Metropolitan Borough Council",
                "Tunbridge Wells Borough Council",
                "Uttlesford District Council",
                "Vale of Glamorgan Council",
                "Vale of White Horse District Council",
                "Wakefield Metropolitan District Council",
                "Walsall Council",
                "Waltham Forest Council",
                "Wandsworth Borough Council",
                "Warrington Borough Council",
                "Warwick District Council",
                "Warwickshire County Council",
                "Watford Borough Council",
                "Waveney District Council",
                "Waverley Borough Council",
                "Wealden District Council",
                "Wellingborough Borough Council",
                "Welwyn Hatfield Borough Council",
                "West Berkshire Council",
                "West Devon Borough Council",
                "West Dorset District Council",
                "West Lancashire Borough Council",
                "West Lindsey District Council",
                "West Oxfordshire District Council",
                "West Somerset District Council",
                "West Sussex County Council",
                "Westminster City Council",
                "Weymouth and Portland Borough Council",
                "Wigan Council",
                "Wiltshire Council",
                "Winchester City Council",
                "Wirral Metropolitan Borough Council",
                "Woking Borough Council",
                "Wokingham Borough Council",
                "Wolverhampton City Council",
                "Worcester City Council",
                "Worcestershire County Council",
                "Worthing Borough Council",
                "Wrexham County Borough Council",
                "Wychavon District Council",
                "Wycombe District Council",
                "Wyre Council",
                "Wyre Forest District Council",
                "Yorkshire Dales National Park"
            };
            return
                li.Select(
                    (s) =>
                        ((s == "Enfield Council")
                            ? new LocalAuthority() {Name = s, Url = "https://new.enfield.gov.uk/"}
                            : ((s == "Arun District Council")
                                ? new LocalAuthority() {Name = s, Url = "http://www.arun.gov.uk/"}
                                : new LocalAuthority() {Name = s})));
        }

        public static void SetUpJobInvoiceMappings(DeadfileContext dbContext)
        {
            foreach (var job in _addedInvoicesForJob)
            {
                var jobId = job.Key;
                foreach (var invoiceMapping in job.Value)
                {
                    var invoiceIds =
                        (from invoice in dbContext.Invoices
                         where invoice.Company == invoiceMapping.Item1
                         where invoice.InvoiceReference == invoiceMapping.Item2
                         select invoice.InvoiceId).ToArray();
                    if (invoiceIds.Length != 1) throw new ApplicationException("Screwed up");
                    var invoiceId = invoiceIds[0];
                    dbContext.JobInvoiceMappings.Add(new JobInvoiceMapping() {JobId = jobId, InvoiceId = invoiceId});
                }
            }
        }
    }
}
