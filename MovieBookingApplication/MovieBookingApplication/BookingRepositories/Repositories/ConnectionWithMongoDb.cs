﻿using MovieBookingApplication.BookingRepositories.interfaces;

namespace MovieBookingApplication.BookingRepositories.Repositories
{
    public class ConnectionWithMongoDb : IConnectionWithMongoDb
    {
        public List<string> CollectionName { get; set; }
        public string DatabaseName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
    }
}
