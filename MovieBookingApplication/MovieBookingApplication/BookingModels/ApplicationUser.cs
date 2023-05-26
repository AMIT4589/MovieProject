﻿using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieBookingApplication.BookingModels
{
    [BsonIgnoreExtraElements]
    public class ApplicationUser : MongoUser
    {

    }
}
