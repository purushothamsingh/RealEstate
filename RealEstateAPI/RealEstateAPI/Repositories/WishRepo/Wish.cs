using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.DomainModels.WishDto;
using RealEstateAPI.Migrations;
using RealEstateAPI.Models;
using RealEstateAPI.Models.WishModule;
using RealEstateAPI.Repositories.LoginRepo;

namespace RealEstateAPI.Repositories.WishRepo
{
    public class Wish : IWishRepo
    {
        private readonly ApplicationDbContext db;
        
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(Auth));
        public Wish(ApplicationDbContext _db )
        {
            db = _db;
            
        }

        public async Task<Response> AddedWishAsync(WishedDto wishedData)
        {
            wished wish = new wished();

            var userPresent = await db.Wishes
                .AnyAsync(x => x.PropertyId == wishedData.Id && x.db_RegisterId == wishedData.db_RegisterId);

            if(userPresent==true)
            {
                return new Response("", StatusCodes.Status406NotAcceptable, "", "Already Added");
            }
            else
            {
                wish.SellRent = wishedData.SellRent;
                wish.PropertyId = wishedData.Id;
                wish.Price = wishedData.Price;
                wish.City = wishedData.City;
                wish.ReadyToMove = wishedData.ReadyToMove;
                wish.db_RegisterId = wishedData.db_RegisterId;
                wish.BuiltArea = wishedData.BuiltArea;
                wish.photo = "";
                wish.BHK = wishedData.BHK;
                wish.FurnishingType = wishedData.FurnishingType;
                wish.Name = wishedData.Name;
                wish.PropertyType = wishedData.PropertyType;

                db.Wishes.AddAsync(wish);
                db.SaveChanges();
                return new Response("added", StatusCodes.Status201Created, wishedData, "");
            }


            
        }

        public async Task<Response> GetwishedList(int id)
        {
            var userWishedData = await db.Wishes.Where(x => x.db_RegisterId == id).Select(x=>x).ToListAsync();

            return new Response("found", StatusCodes.Status302Found, userWishedData, "");
        }
    }
}
