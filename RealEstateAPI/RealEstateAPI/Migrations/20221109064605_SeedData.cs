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


            SET @UserID = (select ID from Db_Registers where UserName='santo')

            --------------------------
            --Seed Property Types
            --------------------------
            IF not exists (select name from PropertyTypes where Name='House')
            insert into PropertyTypes(Name,LastUpdatedOn,LastUpdatedBy)
            select 'House', GETDATE(),@UserID

            IF not exists (select name from PropertyTypes where Name='Apartment')
            insert into PropertyTypes(Name,LastUpdatedOn,LastUpdatedBy)
            select 'Apartment', GETDATE(),@UserID

            IF not exists (select name from PropertyTypes where Name='Duplex')
            insert into PropertyTypes(Name,LastUpdatedOn,LastUpdatedBy)
            select 'Duplex', GETDATE(),@UserID


            --------------------------
            --Seed Furnishing Types
            --------------------------
            IF not exists (select name from FurnishingTypes where Name='Fully')
            insert into FurnishingTypes(Name, LastUpdatedOn, LastUpdatedBy)
            select 'Fully', GETDATE(),@UserID

            IF not exists (select name from FurnishingTypes where Name='Semi')
            insert into FurnishingTypes(Name, LastUpdatedOn, LastUpdatedBy)
            select 'Semi', GETDATE(),@UserID

            IF not exists (select name from FurnishingTypes where Name='Unfurnished')
            insert into FurnishingTypes(Name, LastUpdatedOn, LastUpdatedBy)
            select 'Unfurnished', GETDATE(),@UserID

            --------------------------
            --Seed Cities
            --------------------------
            IF not exists (select top 1 id from Cities)
            Insert into Cities(Name,LastUpdatedBy,LastUpdatedOn,Country)
            select 'New York',@UserID,getdate(),'USA'
            union
            select 'Houston',@UserID,getdate(),'USA'
            union
            select 'Los Angeles',@UserID,getdate(),'USA'
            union
            select 'New Delhi',@UserID,getdate(),'India'
            union
            select 'Bangalore',@UserID,getdate(),'India'

            --------------------------
            --Seed Properties
            --------------------------
            --Seed property for sell
            IF not exists (select top 1 name from Properties where Name='White House Demo')
            insert into Properties(SellRent,Name,PropertyTypeId,BHK,FurnishingTypeId,Price,BuiltArea,CarpetArea,Address,
            Address2,CityId,FloorNo,TotalFloors,ReadyToMove,MainEntrance,Security,Gated,Maintenance,EstPossessionOn,Age,Description,PostedOn,PostedBy,LastUpdatedOn,LastUpdatedBy)
            select 
            1, --Sell Rent
            'White House Demo', --Name
            (select Id from PropertyTypes where Name='Apartment'), --Property Type ID
            2, --BHK
            (select Id from FurnishingTypes where Name='Fully'), --Furnishing Type ID
            1800, --Price
            1400, --Built Area
            900, --Carpet Area
            '6 Street', --Address
            'Golf Course Road', -- Address2
            (select top 1 Id from Cities), -- City ID
            3, -- Floor No
            3, --Total Floors
            1, --Ready to Move
            'East', --Main Entrance
            0, --Security
            1, --Gated
            300, -- Maintenance
            '2019-01-01', -- Establishment or Posession on
            0, --Age
            'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
            Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination. Feel free to call With Query.', --Description
            GETDATE(), --Posted on
            @UserID, --Posted by
            GETDATE(), --Last Updated on
            @UserID --Last Updated by

            ---------------------------
            --Seed property for rent
            ---------------------------
            IF not exists (select top 1 name from Properties where Name='Birla House Demo')
            insert into Properties(SellRent,Name,PropertyTypeId,BHK,FurnishingTypeId,Price,BuiltArea,CarpetArea,Address,
            Address2,CityId,FloorNo,TotalFloors,ReadyToMove,MainEntrance,Security,Gated,Maintenance,EstPossessionOn,Age,Description,PostedOn,PostedBy,LastUpdatedOn,LastUpdatedBy)
            select 
            2, --Sell Rent
            'Birla House Demo', --Name
            (select Id from PropertyTypes where Name='Apartment'), --Property Type ID
            2, --BHK
            (select Id from FurnishingTypes where Name='Fully'), --Furnishing Type ID
            1800, --Price
            1400, --Built Area
            900, --Carpet Area
            '6 Street', --Address
            'Golf Course Road', -- Address2
            (select top 1 Id from Cities), -- City ID
            3, -- Floor No
            3, --Total Floors
            1, --Ready to Move
            'East', --Main Entrance
            0, --Security
            1, --Gated
            300, -- Maintenance
            '2019-01-01', -- Establishment or Posession on
            0, --Age
            'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
            Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination. Feel free to call With Query.', --Description
            GETDATE(), --Posted on
            @UserID, --Posted by
            GETDATE(), --Last Updated on
            @UserID --Last Updated by


");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DECLARE @UserID as int
SET @UserID = (select ID from Db_Registers where UserName='santo')
delete from Users where Username='santo'
delete from PropertyTypes where LastUpdatedBy=@UserID
delete from FurnishingTypes where LastUpdatedBy=@UserID
delete from Cities where LastUpdatedBy=@UserID
delete from Properties where PostedBy=@UserId");

        }
    }
}
