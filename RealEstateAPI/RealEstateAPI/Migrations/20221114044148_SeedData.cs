using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateAPI.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DECLARE @UserID as INT
                    --------------------------
                    --Create User
                    --------------------------


                    SET @UserID = (select ID from Db_Registers where UserName = 'Santo')
                  

                    --------------------------
                    --Seed Property Types
                    --------------------------
                    IF not exists(select name from PropertyTypes where Name = 'House')
                    insert into PropertyTypes(Name, LastUpdatedOn, LastUpdatedBy)
                    select 'House', GETDATE(),@UserID

                    IF not exists(select name from PropertyTypes where Name = 'Apartment')
                    insert into PropertyTypes(Name, LastUpdatedOn, LastUpdatedBy)
                    select 'Apartment', GETDATE(),@UserID

                    IF not exists(select name from PropertyTypes where Name = 'Duplex')
                    insert into PropertyTypes(Name, LastUpdatedOn, LastUpdatedBy)
                    select 'Duplex', GETDATE(),@UserID


                    --------------------------
                    --Seed Furnishing Types
                    --------------------------
                    IF not exists(select name from FurnishingTypes where Name = 'Fully')
                    insert into FurnishingTypes(Name, LastUpdatedOn, LastUpdatedBy)
                    select 'Fully', GETDATE(),@UserID

                    IF not exists(select name from FurnishingTypes where Name = 'Semi')
                    insert into FurnishingTypes(Name, LastUpdatedOn, LastUpdatedBy)
                    select 'Semi', GETDATE(),@UserID

                    IF not exists(select name from FurnishingTypes where Name = 'Unfurnished')
                    insert into FurnishingTypes(Name, LastUpdatedOn, LastUpdatedBy)
                    select 'Unfurnished', GETDATE(),@UserID

                    --------------------------
                    --Seed Cities
                    --------------------------
                    IF not exists(select top 1 id from Cities)
                    Insert into Cities(Name, LastUpdatedBy, LastUpdatedOn, Country)
                    select 'Chennai',@UserID,getdate(),'India'
                    union
                    select 'Bangalore',@UserID,getdate(),'India'
                    union
                    select 'Hyderabad',@UserID,getdate(),'India'
                    union
                    select 'Kozhikode',@UserID,getdate(),'India'

                union
            select 'Houston',@UserID,getdate(),'USA'

                union
            select 'NewYork',@UserID,getdate(),'USA'

                union
            select 'NewDelhi',@UserID,getdate(),'India'

            union
            select 'Karachi',@UserID,getdate(),'Pakistan'

--------------------------
                    --Seed Properties
                    --------------------------
                    --Seed property for sell
                    --1
                    IF not exists(select top 1 name from Properties where Name = 'Green City Avenue')
                    insert into Properties(SellRent, Name, PropertyTypeId, BHK, FurnishingTypeId, Price, BuiltArea, CarpetArea, Address,
                    Address2, CityId, FloorNo, TotalFloors, ReadyToMove, MainEntrance, Security, Gated, Maintenance, EstPossessionOn, Age, Description, PostedOn, PostedBy, LastUpdatedOn, LastUpdatedBy)
                    select
                    1, --Sell Rent
                    'Green City Avenue', --Name
                    (select Id from PropertyTypes where Name = 'House'), --Property Type ID
                    3, --BHK
                    (select Id from FurnishingTypes where Name = 'Fully'), --Furnishing Type ID
                    8500000, --Price
                    2100, --Built Area
                    450, --Carpet Area
                    '1st Raju Street, Thoraipakkam', --Address
                    'Near to AKDR Golf Tower', --Address2
                    (select Id from Cities where Name = 'Chennai'), --City ID
                    1, --Floor No
                    2, --Total Floors
                    1, --Ready to Move
                    'East', --Main Entrance
                    0, --Security
                    1, --Gated
                    500, --Maintenance
                    '2019-01-01', --Establishment or Posession on
                    1, --Age
                    'Well Maintained builder floor available for sale at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                    Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination.Feel free to call With Query.', --Description
                    GETDATE(), --Posted on
                    @UserID, --Posted by
                    GETDATE(), --Last Updated on
                    @UserID--Last Updated by
                    ------------------------------ -
                    --2
                    IF not exists(select top 1 name from Properties where Name = 'Chendhur Homes')
                    insert into Properties(SellRent, Name, PropertyTypeId, BHK, FurnishingTypeId, Price, BuiltArea, CarpetArea, Address,
                    Address2, CityId, FloorNo, TotalFloors, ReadyToMove, MainEntrance, Security, Gated, Maintenance, EstPossessionOn, Age, Description, PostedOn, PostedBy, LastUpdatedOn, LastUpdatedBy)
                    select
                    1, --Sell Rent
                    'Chendhur Homes', --Name
                    (select Id from PropertyTypes where Name = 'Duplex'), --Property Type ID
                    2, --BHK
                    (select Id from FurnishingTypes where Name = 'Fully'), --Furnishing Type ID
                    7000000, --Price
                    1650, --Built Area
                    500, --Carpet Area
                    'Mavoor Road', --Address
                    'Opposite to KFC', --Address2
                    (select Id from Cities where Name = 'Bangalore'), --City ID
                    5, --Floor No
                    6, --Total Floors
                    0, --Ready to Move
                    'South', --Main Entrance
                    1, --Security
                    1, --Gated
                    3500, --Maintenance
                    '2019-01-01', --Establishment or Posession on
                    3, --Age
                    'Well Maintained builder floor available for sale at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                    Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination.Feel free to call With Query.', --Description
                    GETDATE(), --Posted on
                    @UserID, --Posted by
                    GETDATE(), --Last Updated on
                    @UserID--Last Updated by
                    ----------------------------------
                    --3
                    IF not exists(select top 1 name from Properties where Name = 'Vennila Livings')
                    insert into Properties(SellRent, Name, PropertyTypeId, BHK, FurnishingTypeId, Price, BuiltArea, CarpetArea, Address,
                    Address2, CityId, FloorNo, TotalFloors, ReadyToMove, MainEntrance, Security, Gated, Maintenance, EstPossessionOn, Age, Description, PostedOn, PostedBy, LastUpdatedOn, LastUpdatedBy)
                    select
                    1, --Sell Rent
                    'Vennila Livings', --Name
                    (select Id from PropertyTypes where Name = 'House'), --Property Type ID
                    2, --BHK
                    (select Id from FurnishingTypes where Name = 'Fully'), --Furnishing Type ID
                    4500000, --Price
                    800, --Built Area
                    250, --Carpet Area
                    'Green Valley, Banjara Hills', --Address
                    'City Center', --Address2
                    (select Id from Cities where Name = 'Houston'), --City ID
                    1, --Floor No
                    2, --Total Floors
                    0, --Ready to Move
                    'South', --Main Entrance
                    0, --Security
                    0, --Gated
                    100, --Maintenance
                    '2019-01-01', --Establishment or Posession on
                    5, --Age
                    'Well Maintained builder floor available for sale at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                    Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination.Feel free to call With Query.', --Description
                    GETDATE(), --Posted on
                    @UserID, --Posted by
                    GETDATE(), --Last Updated on
                    @UserID--Last Updated by
                    -------------------------- -
                    --4
                    IF not exists(select top 1 name from Properties where Name = 'SaiRam Apartments')
                    insert into Properties(SellRent, Name, PropertyTypeId, BHK, FurnishingTypeId, Price, BuiltArea, CarpetArea, Address,
                    Address2, CityId, FloorNo, TotalFloors, ReadyToMove, MainEntrance, Security, Gated, Maintenance, EstPossessionOn, Age, Description, PostedOn, PostedBy, LastUpdatedOn, LastUpdatedBy)
                    select
                    1, --Sell Rent
                    'SaiRam Apartments', --Name
                    (select Id from PropertyTypes where Name = 'Apartment'), --Property Type ID
                    3, --BHK
                    (select Id from FurnishingTypes where Name = 'Semi'), --Furnishing Type ID
                    25000, --Price
                    1200, --Built Area
                    100, --Carpet Area
                    'Kundalahalli, AECS Layout', --Address
                    'Near to HDFC Bank', --Address2
                    (select Id from Cities where Name = 'Karachi'), --City ID
                    2, --Floor No
                    9, --Total Floors
                    1, --Ready to Move
                    'North', --Main Entrance
                    1, --Security
                    1, --Gated
                    1000, --Maintenance
                    '2019-01-01', --Establishment or Posession on
                    0, --Age
                    'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                    Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination.Feel free to call With Query.', --Description
                    GETDATE(), --Posted on
                    @UserID, --Posted by
                    GETDATE(), --Last Updated on
                    @UserID--Last Updated by
                    ------------------------------
                    --6
                    IF not exists(select top 1 name from Properties where Name = 'Pearl Castle')
                    insert into Properties(SellRent, Name, PropertyTypeId, BHK, FurnishingTypeId, Price, BuiltArea, CarpetArea, Address,
                    Address2, CityId, FloorNo, TotalFloors, ReadyToMove, MainEntrance, Security, Gated, Maintenance, EstPossessionOn, Age, Description, PostedOn, PostedBy, LastUpdatedOn, LastUpdatedBy)
                    select
                    1, --Sell Rent
                    'Pearl Castle', --Name
                    (select Id from PropertyTypes where Name = 'Duplex'), --Property Type ID
                    2, --BHK
                    (select Id from FurnishingTypes where Name = 'Semi'), --Furnishing Type ID
                    5000000, --Price
                    1150, --Built Area
                    300, --Carpet Area
                    'Pearlvine Garden, Electronic City', --Address
                    'Near to Eurofins IT Solutions', --Address2
                    (select Id from Cities where Name = 'Chennai'), --City ID
                    3, --Floor No
                    5, --Total Floors
                    1, --Ready to Move
                    'North', --Main Entrance
                    1, --Security
                    1, --Gated
                    1000, --Maintenance
                    '2019-01-01', --Establishment or Posession on
                    2, --Age
                    'Well Maintained builder floor available for sale at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                    Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination.Feel free to call With Query.', --Description
                    GETDATE(), --Posted on
                    @UserID, --Posted by
                    GETDATE(), --Last Updated on
                    @UserID--Last Updated by
                    --------------------------------------------
                    ------------------------------
                    --8
                    IF not exists(select top 1 name from Properties where Name = 'Casa Grand Living Homes')
                    insert into Properties(SellRent, Name, PropertyTypeId, BHK, FurnishingTypeId, Price, BuiltArea, CarpetArea, Address,
                    Address2, CityId, FloorNo, TotalFloors, ReadyToMove, MainEntrance, Security, Gated, Maintenance, EstPossessionOn, Age, Description, PostedOn, PostedBy, LastUpdatedOn, LastUpdatedBy)
                    select
                    1, --Sell Rent
                    'Casa Grand Living Homes', --Name
                    (select Id from PropertyTypes where Name = 'House'), --Property Type ID
                    3, --BHK
                    (select Id from FurnishingTypes where Name = 'Fully'), --Furnishing Type ID
                    12500000, --Price
                    2400, --Built Area
                    800, --Carpet Area
                    'Cheran Street, Perumbakkam', --Address
                    'Opposite to Saravana Stores', --Address2
                    (select Id from Cities where Name = 'NewYork'), --City ID
                    5, --Floor No
                    14, --Total Floors
                    1, --Ready to Move
                    'West', --Main Entrance
                    1, --Security
                    1, --Gated
                    4000, --Maintenance
                    '2019-01-01', --Establishment or Posession on
                    2, --Age
                    'Well Maintained builder floor available for sale at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                    Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination.Feel free to call With Query.', --Description
                    GETDATE(), --Posted on
                    @UserID, --Posted by
                    GETDATE(), --Last Updated on
                    @UserID--Last Updated by
                    --------------------------------------------
                    --9
                    IF not exists(select top 1 name from Properties where Name = 'Deepak Flats')
                    insert into Properties(SellRent, Name, PropertyTypeId, BHK, FurnishingTypeId, Price, BuiltArea, CarpetArea, Address,
                    Address2, CityId, FloorNo, TotalFloors, ReadyToMove, MainEntrance, Security, Gated, Maintenance, EstPossessionOn, Age, Description, PostedOn, PostedBy, LastUpdatedOn, LastUpdatedBy)
                    select
                    1, --Sell Rent
                    'Deepak Flats', --Name
                    (select Id from PropertyTypes where Name = 'Apartment'), --Property Type ID
                    3, --BHK
                    (select Id from FurnishingTypes where Name = 'Fully'), --Furnishing Type ID
                    3800000, --Price
                    800, --Built Area
                    150, --Carpet Area
                    'Nallagandla 2nd Street', --Address
                    'Near to Commissioner Office', --Address2
                    (select Id from Cities where Name = 'NewDelhi'), --City ID
                    1, --Floor No
                    4, --Total Floors
                    0, --Ready to Move
                    'North', --Main Entrance
                    1, --Security
                    1, --Gated
                    1500, --Maintenance
                    '2019-01-01', --Establishment or Posession on
                    2, --Age
                    'Well Maintained builder floor available for sale at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                    Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination.Feel free to call With Query.', --Description
                    GETDATE(), --Posted on
                    @UserID, --Posted by
                    GETDATE(), --Last Updated on
                    @UserID--Last Updated by
                    --------------------------------------------


                    -------------------------- -
                    --Seed property for rent
                    -------------------------- -
                    --1
                    IF not exists(select top 1 name from Properties where Name = 'Dhiya Nest')
                    insert into Properties(SellRent, Name, PropertyTypeId, BHK, FurnishingTypeId, Price, BuiltArea, CarpetArea, Address,
                    Address2, CityId, FloorNo, TotalFloors, ReadyToMove, MainEntrance, Security, Gated, Maintenance, EstPossessionOn, Age, Description, PostedOn, PostedBy, LastUpdatedOn, LastUpdatedBy)
                    select
                    2, --Sell Rent
                    'Dhiya Nest', --Name
                    (select Id from PropertyTypes where Name = 'Apartment'), --Property Type ID
                    2, --BHK
                    (select Id from FurnishingTypes where Name = 'Semi'), --Furnishing Type ID
                    10000, --Price
                    600, --Built Area
                    100, --Carpet Area
                    'Gandhi Street, Thiruvanmiyur', --Address
                    'Thiruvanmiyur Railway Station', --Address2
                    (select Id from Cities where Name = 'Bangalore'), --City ID
                    4, --Floor No
                    7, --Total Floors
                    1, --Ready to Move
                    'West', --Main Entrance
                    1, --Security
                    1, --Gated
                    2000, --Maintenance
                    '2019-01-01', --Establishment or Posession on
                    3, --Age
                    'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                    Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination.Feel free to call With Query.', --Description
                    GETDATE(), --Posted on
                    @UserID, --Posted by
                    GETDATE(), --Last Updated on
                    @UserID--Last Updated by
                    ------------------------------ -
                    --2
                    IF not exists(select top 1 name from Properties where Name = 'SaiRam Apartments')
                    insert into Properties(SellRent, Name, PropertyTypeId, BHK, FurnishingTypeId, Price, BuiltArea, CarpetArea, Address,
                    Address2, CityId, FloorNo, TotalFloors, ReadyToMove, MainEntrance, Security, Gated, Maintenance, EstPossessionOn, Age, Description, PostedOn, PostedBy, LastUpdatedOn, LastUpdatedBy)
                    select
                    1, --Sell Rent
                    'SaiRam Apartments', --Name
                    (select Id from PropertyTypes where Name = 'Apartment'), --Property Type ID
                    3, --BHK
                    (select Id from FurnishingTypes where Name = 'Semi'), --Furnishing Type ID
                    25000, --Price
                    1200, --Built Area
                    100, --Carpet Area
                    'Kundalahalli, AECS Layout', --Address
                    'Near to HDFC Bank', --Address2
                    (select Id from Cities where Name = 'Karachi'), --City ID
                    2, --Floor No
                    9, --Total Floors
                    1, --Ready to Move
                    'North', --Main Entrance
                    1, --Security
                    1, --Gated
                    1000, --Maintenance
                    '2019-01-01', --Establishment or Posession on
                    0, --Age
                    'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                    Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination.Feel free to call With Query.', --Description
                    GETDATE(), --Posted on
                    @UserID, --Posted by
                    GETDATE(), --Last Updated on
                    @UserID--Last Updated by
                    ------------------------------
                    --3
                    IF not exists(select top 1 name from Properties where Name = 'Priya Homes')
                    insert into Properties(SellRent, Name, PropertyTypeId, BHK, FurnishingTypeId, Price, BuiltArea, CarpetArea, Address,
                    Address2, CityId, FloorNo, TotalFloors, ReadyToMove, MainEntrance, Security, Gated, Maintenance, EstPossessionOn, Age, Description, PostedOn, PostedBy, LastUpdatedOn, LastUpdatedBy)
                    select
                    2, --Sell Rent
                    'Priya Homes', --Name
                    (select Id from PropertyTypes where Name = 'House'), --Property Type ID
                    2, --BHK
                    (select Id from FurnishingTypes where Name = 'Unfurnished'), --Furnishing Type ID
                    6000, --Price
                    500, --Built Area
                    100, --Carpet Area
                    'Walajah Street, Guindy', --Address
                    'Near to Anna University', --Address2
                    (select Id from Cities where Name = 'NewDelhi'), --City ID
                    1, --Floor No
                    1, --Total Floors
                    1, --Ready to Move
                    'East', --Main Entrance
                    0, --Security
                    0, --Gated
                    100, --Maintenance
                    '2019-01-01', --Establishment or Posession on
                    0, --Age
                    'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                    Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination.Feel free to call With Query.', --Description
                    GETDATE(), --Posted on
                    @UserID, --Posted by
                    GETDATE(), --Last Updated on
                    @UserID--Last Updated by
                    ---------------------------- -
 --Seed Property for Photos
                    ------------------------------
                    --1
                    IF not exists(select Id from Photos where PropertyId = 2)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668345032/pfmj9da7m5refpeul7o5.jpg', 1, 1, 'pfmj9da7m5refpeul7o5'
                    ------------------------------
                    --2
                    IF not exists(select Id from Photos where PropertyId = 29)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668345148/nadqwnuruwuut3fn999j.jpg', 1, 1, 'nadqwnuruwuut3fn999j'
                    ------------------------------ -
                    --3
                    IF not exists(select Id from Photos where PropertyId = 30)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668345927/ji6t1xlspbcynsqwypw9.jpg', 1, 1, 'ji6t1xlspbcynsqwypw9'
                    ------------------------------ -
                    --4
                    IF not exists(select Id from Photos where PropertyId = 31)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668345979/d9yvrqf3baeyq4gh8fju.jpg', 1, 1, 'd9yvrqf3baeyq4gh8fju'
                    --------------------------------
                    --5
                    IF not exists(select Id from Photos where PropertyId = 32)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668346020/i0fih6bcecmpunbblloe.jpg', 1, 1, 'i0fih6bcecmpunbblloe'
                    -------------------------------- -
                    --6
                    IF not exists(select Id from Photos where PropertyId = 33)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668346098/clsnyychlhzpksty9kgs.jpg', 1, 1, 'clsnyychlhzpksty9kgs'
                    ----------------------------------
                    --7
                    IF not exists(select Id from Photos where PropertyId = 34)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668346180/uxblezsnnkhh0wauz3fx.jpg', 1, 1, 'uxblezsnnkhh0wauz3fx'
                    ---------------------------------- -
                    --8
                    IF not exists(select Id from Photos where PropertyId = 35)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668346230/nm9xmnbhelb26iqxjshs.jpg', 1, 1, 'nm9xmnbhelb26iqxjshs'
                    ---------------------------------- -
                    --9
                    IF not exists(select Id from Photos where PropertyId = 36)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668346278/gkjwdm21rtvbmxytn7m0.jpg', 1, 1, 'gkjwdm21rtvbmxytn7m0'

--Seed Property for Photos
                    ------------------------------
                    --1
                    IF not exists(select Id from Photos where PropertyId = 1)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668345032/pfmj9da7m5refpeul7o5.jpg', 1, 1, 'pfmj9da7m5refpeul7o5'
                    ------------------------------
                    --2
                    IF not exists(select Id from Photos where PropertyId = 2)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668345148/nadqwnuruwuut3fn999j.jpg', 1, 2, 'nadqwnuruwuut3fn999j'
                    ------------------------------ -
                    --3
                    IF not exists(select Id from Photos where PropertyId = 3)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668345927/ji6t1xlspbcynsqwypw9.jpg', 1, 3, 'ji6t1xlspbcynsqwypw9'
                    ------------------------------ -
                    --4
                    IF not exists(select Id from Photos where PropertyId = 4)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668345979/d9yvrqf3baeyq4gh8fju.jpg', 1, 4, 'd9yvrqf3baeyq4gh8fju'
                    --------------------------------
                    --5
                    IF not exists(select Id from Photos where PropertyId = 5)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668346020/i0fih6bcecmpunbblloe.jpg', 1, 5, 'i0fih6bcecmpunbblloe'
                    -------------------------------- -
                    --6
                    IF not exists(select Id from Photos where PropertyId = 6)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668346098/clsnyychlhzpksty9kgs.jpg', 1, 6, 'clsnyychlhzpksty9kgs'
                    ----------------------------------
                    --7
                    IF not exists(select Id from Photos where PropertyId = 7)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668346180/uxblezsnnkhh0wauz3fx.jpg', 1, 7, 'uxblezsnnkhh0wauz3fx'
                    ---------------------------------- -
                    --8
                    IF not exists(select Id from Photos where PropertyId = 8)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668346230/nm9xmnbhelb26iqxjshs.jpg', 1, 8, 'nm9xmnbhelb26iqxjshs'
                    ---------------------------------- -
                    --9
                    IF not exists(select Id from Photos where PropertyId = 9)
                    insert into Photos(LastUpdatedOn, LastUpdatedBy, ImageUrl, IsPrimary, PropertyId, PublicId)
                    select GETDATE(), @UserID, 'https://res.cloudinary.com/drn3bc1nj/image/upload/v1668346278/gkjwdm21rtvbmxytn7m0.jpg', 1, 9, 'gkjwdm21rtvbmxytn7m0'

                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DECLARE @UserID as int
SET @UserID = (select ID from Db_Registers where UserName='Santo')
delete from Users where Username='Santo'
delete from PropertyTypes where LastUpdatedBy=@UserID
delete from FurnishingTypes where LastUpdatedBy=@UserID
delete from Cities where LastUpdatedBy=@UserID
delete from Properties where PostedBy=@UserId");
        }
    }
}

