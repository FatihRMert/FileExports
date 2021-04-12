using System;
using System.Collections.Generic;
using ExelAndPdfShop.Api.Models;

namespace ExelAndPdfShop.Api.Utility
{
    public static class DataStorage
    {
        public static List<Employee> GetEmployees()
        {
            return new List<Employee>
            {
                new() {Id = 1, Name = "Fatih", Surname = "Mert", JoinDate = DateTime.Now},
                new() {Id = 2, Name = "Sönmez", Surname = "Korkmaz", JoinDate = DateTime.Now.AddDays(1)},
                new() {Id = 3, Name = "Sinirlenmez", Surname = "Yılmaz", JoinDate = DateTime.Now.AddDays(2)},
                new() {Id = 4, Name = "Kalkmaz", Surname = "Durmaz", JoinDate = DateTime.Now.AddDays(3)},
                new() {Id = 5, Name = "Gelmez", Surname = "Gitmez", JoinDate = DateTime.Now.AddDays(4)},
            };
        }
    }
}