using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public List<User> GetUserList()
        {
            List<User> userList = new List<User>()
            {
                new User()
                {
                    UserId=1,
                    FirstName="Adem",
                    LastName="Olguner",
                    Email="ademolguner@gmail.com"
                 }
                //, new User()
                //{
                //    UserId=1,
                //    FirstName="Barış",
                //    LastName="Boy",
                //    Email="baris.boy@ericssonmsp.com"
                // } ,
                //new User()
                //{
                //    UserId=1,
                //    FirstName="Vacip",
                //    LastName="Derici",
                //    Email="vacip.derici@ericssonmsp.com"
                // } ,
                //new User()
                //{
                //    UserId=1,
                //    FirstName="Talha",
                //    LastName="Seçkin",
                //    Email="taha.seckin@ericssonmsp.com"
                // }
            };

            return userList;
        }

        //public static List<User> GetUserListJson()
        //{
        //    var userLists = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"/./../userlistconfig.json"));
        //    return userLists;
        //}


    }
}
