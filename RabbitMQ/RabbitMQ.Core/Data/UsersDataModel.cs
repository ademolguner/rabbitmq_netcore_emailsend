using RabbitMQ.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Data
{
    public class UsersDataModel : IDataModel<User>
    {
        public IEnumerable<User> GetData()
        {
            return new List<User>
            {
                new User
                {
                    UserId=1,
                    FirstName="Adem",
                    LastName="Olguner",
                    Email="ademolguner@gmail.com"
                 }
                //, new User
                //{
                //    UserId=1,
                //    FirstName="Barış",
                //    LastName="Boy",
                //    Email="baris.boy@ericssonmsp.com"
                // } ,
                //new User
                //{
                //    UserId=1,
                //    FirstName="Vacip",
                //    LastName="Derici",
                //    Email="vacip.derici@ericssonmsp.com"
                // } ,
                //new User
                //{
                //    UserId=1,
                //    FirstName="Talha",
                //    LastName="Seçkin",
                //    Email="taha.seckin@ericssonmsp.com"
                // }
            };
        }

    }
}
