using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TRPO_08_1.Models;

namespace TRPO_08_1.Data
{
    public static class RestarauntInitializationHandler
    {
        public static void Initialize()
        {
            Database.SetInitializer(new RestarauntContext.CreateInitializer());
            using (var restarauntContext = new RestarauntContext())
            {
                restarauntContext.Database.Initialize(true);
            }
        }
    }
}