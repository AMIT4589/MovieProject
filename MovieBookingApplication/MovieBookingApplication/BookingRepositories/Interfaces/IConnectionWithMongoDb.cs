﻿namespace MovieBookingApplication.BookingRepositories.interfaces
{
    public interface IConnectionWithMongoDb
    {
        List<string> CollectionName { get; set; }
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}
