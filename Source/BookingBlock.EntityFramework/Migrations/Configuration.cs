using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookingBlock.EntityFramework.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<BookingBlock.EntityFramework.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "BookingBlock.EntityFramework.ApplicationDbContext";
        }

        private const string BusinessTypesList = @"Accessories
Airlines
Airport Shuttles
Allergists
Anesthesiologists
Apartments
Appliances
Art Schools
Art Supplies
Audiologist
Bankruptcy Law
Barre Classes
Bikes
Blow Dry/Out Services
Bookstores
Boot Camps
Boxing
Business Law
Candy Stores
Cantonese
Cardiologists
Cards & Stationery
Cards & Stationery
Champagne Bars
Cheese Shops
Children's Clothing
Chocolatiers & Shops
Cocktail Bars
Columbian
Comic Books
Commercial Real Estate
Cooking Schools
Cosmetic Dentists
Cosmetic Surgeons
Cosmetology Schools
Costumes
CPR Classes
Criminal Defense Law
Dance Schools
Dance Studios
Data Recovery
Department Stores
Dermatologists
Diagnostic Imaging
Dim Sum
Dive Bars
Divorce & Family Law
Dog Parks
Dog Walkers
Dominican
Driving Schools
DUI Law
Ear Nose & Throat
Egyptian
Employment Law
Endodontists
Estate Planning Law
Ethnic Food
Event Photography
Fabric Stores
Family Practice
Fertility
First Aid Classes
Flight Instruction
Florists
Formal Wear
Framing
Free Diving
Fruits & Veggies
Furniture Stores
Gastroenterologist
Gay Bars
General Dentistry
General Litigation
Gerontologists
Gift Shops
Golf Equipment
Gyms
Hair Extensions
Hair Stylists
Haitian
Hardware Stores
Hats
Health Markets
Herbs & Spices
Home Décor
Home Staging
Hookah Bars
Hot Tub & Pool
Immigration Law
Internal Medicine
Kitchen & Bath
Laboratory Testing
Language Schools
Laser Hair Removal
Leather Goods
Lebanese
Limos
Lingerie
Lounges
Martial Arts
Massage Schools
Maternity Wear
Mattresses
Meat Shops
Men's Clothing
Men's Hair Salons
Mobile Phone Repair
Mortgage Brokers
Music & DVDs
Naturopathic/Holistic
Neurologist
Newspapers & Magazines
Nurseries & Gardening
Obstretricians & Gynecologists
Oncologist
Ophthalmologists
Oral Surgeons
Orthodontists
Orthopedists
Osteopathic Physicians
Outdoor Gear
Pediatric Dentists
Pediatricians
Periodontists
Personal Injury Law
Pet Boarding/Pet Sitting
Pet Groomers
Pet Training
Pilates
Plus Size Fashion
Podiatrists
Proctologists
Property Management
Psychiatrists
Public Transportation
Pubs
Puerto Rican
Pulmonologists
Real Estate Agents
Real Estate Law
Real Estate Services
Salvadoran
Scuba Diving
Seafood Markets
Senegalese
Session Photography
Shanghainese
Shared Office Spaces
Shoe Stores
Skate Parks
South African
Sports Bars
Sports Medicine
Sports Wear
Sports Wear
Spray Tanning
Surf Shop
Swimming Lessons/Schools
Swimming Lessons/Schools
Swimwear
Szechuan
Tai Chi
Tanning Beds
Tattoo Removal
Taxis
Trainers
Trinidadian
University Housing
Urologists
Used, Vintage & Consignment
Venezuelan
Videos & Video Game Rental
Vinyl Records
Vocational & Technical School
Wine Bars
Women's Clothing
Yoga
Active Life
Amateur Sports Teams
Amusement Parks
Aquariums
Archery
Badminton
Basketball Courts
Beaches
Bike Rentals
Boating
Bowling
Climbing
Disc Golf
Diving
Free Diving
Scuba Diving
Fishing
Fitness & Instruction
Barre Classes
Boot Camps
Boxing
Dance Studios
Gyms
Martial Arts
Pilates
Swimming Lessons/Schools
Tai Chi
Trainers
Yoga
Go Karts
Golf
Gun/Rifle Ranges
Gymnastics
Hang Gliding
Hiking
Horse Racing
Horseback Riding
Hot Air Balloons
Kiteboarding
Lakes
Laser Tag
Leisure Centers
Mini Golf
Mountain Biking
Paddleboarding
Paintball
Parks
Dog Parks
Skate Parks
Playgrounds
Rafting/Kayaking
Recreation Centers
Rock Climbing
Skating Rinks
Skydiving
Soccer
Spin Classes
Sports Clubs
Squash
Summer Camps
Surfing
Swimming Pools
Tennis
Trampoline Parks
Tubing
Zoos
Arts & Entertainment
Arcades
Art Galleries
Botanical Gardens
Casinos
Cinema
Cultural Center
Festivals
Jazz & Blues
Museums
Music Venues
Opera & Ballet
Performing Arts
Professional Sports Teams
Psychics & Astrologers
Race Tracks
Social Clubs
Stadiums & Arenas
Ticket Sales
Wineries
Automotive
Auto Detailing
Auto Glass Services
Auto Loan Providers
Auto Parts & Supplies
Auto Repair
Boat Dealers
Body Shops
Car Dealers
Car Stereo Installation
Car Wash
Gas & Service Stations
Motorcycle Dealers
Motorcycle Repair
Oil Change Stations
Parking
RV Dealers
Smog Check Stations
Tires
Towing
Truck Rental
Windshield Installation & Repair
Beauty & Spas
Barbers
Cosmetics & Beauty Supply
Day Spas
Eyelash Service
Hair Extensions
Hair Removal
Laser Hair Removal
Hair Salons
Blow Dry/Out Services
Hair Extensions
Hair Stylists
Men's Hair Salons
Makeup Artists
Massage
Medical Spas
Nail Salons
Permanent Makeup
Piercing
Rolfing
Skin care
Tanning
Spray Tanning
Tanning Beds
Tattoo
Education
Adult Education
College Counseling
Colleges & Universities
Educational Services
Elementary Schools
Middle Schools & High Schools
Preschools
Private Tutors
Religious Schools
Special Education
Specialty Schools
Art Schools
CPR Classes
Cooking Schools
Cosmetology Schools
Dance Schools
Driving Schools
First Aid Classes
Flight Instruction
Language Schools
Massage Schools
Swimming Lessons/Schools
Vocational & Technical School
Test Preparation
Tutoring Centers
Event Planning & Services
Bartenders
Boat Charters
Cards & Stationery
Caterers
Clowns
DJs
Hotels
Magicians
Musicians
Officiants
Part & Event Planning
Party Bus Rentals
Party Equipment Rentals
Party Supplies
Personal Chefs
Photographers
Event Photography
Session Photography
Venues & Event Spaces
Videographers
Wedding Planning
Financial Services
Banks & Credit Unions
Check Cashing/Pay-day Loans
Financial Advising
Insurance
Investing
Tax Services
Food
Bagels
Bakeries
Beer, Wine & Spirits
Breweries
Bubble Tea
Butcher
CSA
Coffee & Tea
Convenience Stores
Desserts
Do-It-Yourself Food
Donuts
Farmers Market
Food Delivery Services
Food Trucks
Gelato
Grocery
Ice Cream & Frozen Yogust
Internet Cafes
Juice Bars & Smoothies
Pretzels
Shaved Ice
Specialty Food
Candy Stores
Cheese Shops
Chocolatiers & Shops
Ethnic Food
Fruits & Veggies
Health Markets
Herbs & Spices
Meat Shops
Seafood Markets
Street Vendors
Tea Rooms
Wineries
Health & Medical
Acupuncture
Cannabis Clinics
Chiropractors
Counseling & Mental Health
Dentists
Cosmetic Dentists
Endodontists
General Dentistry
Oral Surgeons
Orthodontists
Pediatric Dentists
Periodontists
Diagnostic Services
Diagnostic Imaging
Laboratory Testing
Doctors
Allergists
Anesthesiologists
Audiologist
Cardiologists
Cosmetic Surgeons
Dermatologists
Ear Nose & Throat
Family Practice
Fertility
Gastroenterologist
Gerontologists
Internal Medicine
Naturopathic/Holistic
Neurologist
Obstretricians & Gynecologists
Oncologist
Ophthalmologists
Orthopedists
Osteopathic Physicians
Pediatricians
Podiatrists
Proctologists
Psychiatrists
Pulmonologists
Sports Medicine
Tattoo Removal
Urologists
Hearing Aid Providers
Home Health Care
Hospice
Hospitals
Lactation Services
Laser Eye Surgery/Lasik
Massage Therapy
Medical Centers
Medical Spas
Medical Transportation
Midwives
Nutritionists
Occupational Therapy
Optometrists
Physical Therapy
Reflexology
Rehabilitation Center
Retirement Homes
Speech Therapists
Traditional Chinese Medicine
Urgent Care
Weight Loss Centers
Home Services
Building Supplies
Carpet Installation
Carpeting
Contractors
Damage Restoration
Electricians
Flooring
Garage Door Services
Gardeners
Handyman
Heating & Air Conditioning/HVAC
Home Cleaning
Home Inspectors
Home Organization
Home Theatre Installation
Home Window Tinting
Interior Design
Internet Service Providers
Irrigation
Keys & Locksmith
Landscape Architects
Landscaping
Lighting Fixtures & Equipment
Masonry/Concrete
Movers
Painters
Plumbing
Pool Cleaners
Real Estate
Apartments
Commercial Real Estate
Home Staging
Mortgage Brokers
Property Management
Real Estate Agents
Real Estate Services
Shared Office Spaces
University Housing
Roofing
Security Systems
Shades & Blinds
Solar Installation
Television Service Providers
Tree Services
Utilities
Window Washing
Windows Installation
Hotels & Travel
Airports
Bed & Breakfast
Campgrounds
Car Rental
Guest Houses
Hostels
Hotels
Motorcycle Rental
RV Parks
RV Rental
Resorts
Ski Resorts
Tours
Train Stations
Transportation
Airlines
Airport Shuttles
Limos
Public Transportation
Taxis
Travel Services
Vacation Rental Agents
Vacation Rentals
Local Flavor
Yelp Events
Local Services
Appliances & Repair
Bail Bondsmen
Bike Repair/Maintenance
Carpet Cleaning
Child Care & Day Care
Community Service/Non-Profit
Couriers & Delivery Services
Dry Cleaning & Laundry
Electronics Repair
Funeral Services & Cemeteries
Furniture Reupholstery
IT Services & Computer Repair
Data Recovery
Mobile Phone Repair
Jewelry Repair
Junk Removal & Hauling
Nanny Services
Notaries
Pest Control
Printing Services
Recording & Rehearsal Studios
Recycling Center
Screen Printing
Screen Printing/T-Shirt Printing
Self Storage
Sewing & Alterations
Shipping Centers
Shoe Repair
Snow Removal
Watch Repair
Mass Media
Print Media
Radio Stations
Television Stations
Nightlife
Adult Entertainment
Bars
Champagne Bars
Cocktail Bars
Dive Bars
Gay Bars
Hookah Bars
Lounges
Pubs
Sports Bars
Wine Bars
Comedy Clubs
Country Dance Halls
Dance Clubs
Jazz & Blues
Karaoke
Music Venues
Piano Bars
Pool Halls
Pets
Animal Shelters
Horse Boarding
Pet Services
Dog Walkers
Pet Boarding/Pet Sitting
Pet Groomers
Pet Training
Pet Stores
Veterinarians
Professional Services
Accountants
Advertising
Architects
Boat Repair
Career Counseling
Editorial Services
Employment Agencies
Graphic Design
Internet Service Providers
Lawyers
Bankruptcy Law
Business Law
Criminal Defense Law
DUI Law
Divorce & Family Law
Employment Law
Estate Planning Law
General Litigation
Immigration Law
Personal Injury Law
Real Estate Law
Legal Services
Life Coach
Marketing
Matchmakers
Office Cleaning
Payroll Services
Personal Assistants
Private Investigation
Public Relations
Talent Agencies
Taxidermy
Translation Services
Video/Film Production
Web Design
Public Services & Government
Courthouses
Departments of Motor Vehicles
Embassy
Fire Departments
Landmarks & Historical Buildings
Libraries
Police Departments
Post Offices
Real Estate
Apartments
Commercial Real Estate
Home Staging
Mortgage Brokers
Property Management
Real Estate Agents
Real Estate Services
Shared Office Spaces
University Housing
Religious Organizations
Buddhist Temples
Churches
Hindu Temples
Mosques
Synagogues
Restaurants
Afghan
African
Senegalese
South African
American(New)
American(Traditional)
Arabian
Argentine
Armenian
Asian Fusion
Australian
Austrian
Bangladeshi
Barbeque
Basque
Belgian
Brasseries
Brazilian
Breakfast & Brunch
British
Buffets
Burgers
Burmese
Cafes
Cafeteria
Cajun/Creole
Cambodian
Caribbean
Dominican
Haitian
Puerto Rican
Trinidadian
Catalan
Cheesesteaks
Chicken Wings
Chinese
Cantonese
Dim Sum
Shanghainese
Szechuan
Comfort Food
Creperies
Cuban
Czech
Delis
Diners
Ethiopian
Fast Food
Filipino
Fish & Chips
Fondue
Food Court
Food Stands
French
Gastropubs
German
Gluten-Free
Greek
Halal
Hawaiian
Himalayan/Nepalese
Hot Dogs
Hot Pot
Hungarian
Iberian
Indian
Indonesian
Irish
Italian
Japanese
Korean
Kosher
Laotian
Latin American
Columbian
Salvadoran
Venezuelan
Live/Raw Food
Malaysian
Meditteranean
Mexican
Middle Eastern
Egyptian
Lebanese
Modern European
Mongolian
Pakistani
Persian/Iranian
Peruvian
Pizza
Polish
Portuguese
Russian
Salad
Sandwiches
Scandinavian
Scottish
Seafood
Singaporean
Slovakian
Soul Food
Soup
Southern
Spanish
Steakhouses
Sushi Bars
Taiwanese
Tapas Bars
Tapas/Small Plates
Tex-Mex
Thai
Turkish
Ukranian
Vegan
Vegetarian
Vietnamese
Shopping
Adult
Antiques
Art Galleries
Arts & Crafts
Art Supplies
Cards & Stationery
Costumes
Fabric Stores
Framing
Auction Houses
Baby Gear & Furniture
Bespoke Clothing
Books, Mags, Music & Video
Bookstores
Comic Books
Music & DVDs
Newspapers & Magazines
Videos & Video Game Rental
Vinyl Records
Bridal
Computers
Cosmetics & Beauty Supply
Department Stores
Discount Store
Drugstores
Electronics Repair
Eyewear & Opticians
Fashion
Accessories
Children's Clothing
Department Stores
Formal Wear
Hats
Leather Goods
Lingerie
Maternity Wear
Men's Clothing
Plus Size Fashion
Shoe Stores
Sports Wear
Surf Shop
Swimwear
Used, Vintage & Consignment
Women's Clothing
Fireworks
Flea Markets
Flowers & Gifts
Cards & Stationery
Florists
Gift Shops
Golf Equipment Shops
Guns & Ammo
Hobby Shops
Home & Garden
Appliances
Furniture Stores
Hardware Stores
Home Décor
Hot Tub & Pool
Kitchen & Bath
Mattresses
Nurseries & Gardening
Jewelry
Knitting Supplies
Luggage
Medical Supplies
Mobile Phones
Motorcycle Gear
Musical Instruments & Teachers
Office Equipment
Outlet Stores
Pawn Shops
Personal Shopping
Photography Stores & Services
Pool & Billiards
Pop-up Shops
Shopping Centers
Sporting Goods
Bikes
Golf Equipment
Outdoor Gear
Sports Wear
Thrift Stores
Tobacco Shops
Toy Stores
Trophy Shops
Uniforms
Watches
Wholesale Stores
Wigs
";
        Random rand = new Random();

        public const string Alphabet =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string GenerateString(int size)
        {
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);
        }

        private BusinessType[] CreateBusinessTypes()
        {
            List<BusinessType> b = new List<BusinessType>();

            StringReader stringReader = new StringReader(BusinessTypesList);

            while (true)
            {
                var l = stringReader.ReadLine();

                if (l == null)
                {
                    break;
                }

                var name = l.Trim();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (!b.Any(b2 => b2.Name == name))
                    {
                        b.Add(new BusinessType() { Name = name });
                    }

                }
            }


            return b.ToArray();
        }


        private void SeedBusinessTypes(ApplicationDbContext context)
        {
            foreach (BusinessType businessType in CreateBusinessTypes())
            {
                context.BusinessTypes.Add(businessType);

            }


            context.SaveChanges();
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // if there arn't any business types in the database
            if (!context.BusinessTypes.Any())
            {

                // seed the business types.
                this.SeedBusinessTypes(context);
            }


            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            ApplicationUserManager applicationUserManager = new ApplicationUserManager(userStore);

            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);

            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            var webmaster = new ApplicationUser();

            webmaster.Title = "Dr.";
            webmaster.FirstName = "bookingblock";
            webmaster.LastName = "webmaster";

            webmaster.Email = "webmaster@bookingblock.azurewebsites.net";
            webmaster.UserName = "webmaster@bookingblock.azurewebsites.net";

            IdentityResult result = applicationUserManager.CreateAsync(webmaster, "Enterprise2016!")
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            

            var roleCreateResult =
                roleManager.CreateAsync(new IdentityRole("webmaster"))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();


            if (result.Succeeded && roleCreateResult.Succeeded)
            {
                var addToRoleResult =
    applicationUserManager.AddToRoleAsync(webmaster.Id, "webmaster")
        .ConfigureAwait(false)
        .GetAwaiter()
        .GetResult();
            }


            base.Seed(context);
        }
    }
}
