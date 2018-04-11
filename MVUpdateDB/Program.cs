using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using NConsole;
using Scrypt;
using LiteDB;

namespace MVUpdateDB
{
    class MainClass
    {
        //LiteDB connection
        //static LiteDatabase db = new LiteDatabase(@"Users.db");

        static void Main(string[] args)
        {
            var processor = new CommandLineProcessor(new ConsoleHost());
            processor.RegisterCommand<InsertCommand>("insert");
            processor.Process(args);
        }
    }

    // <summary>
    /// Users class.
    /// </summary>
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public bool IsActive { get; set; }
    }

    [Description("Insert AvatarName and AvatarPassword")]
    public class InsertCommand : IConsoleCommand
    {
        [Description("The first value: AvatarName")]
        [Argument(Name = "FirstValue")]
        public string FirstValue { get; set; }

        [Description("The second value: AvatarPassword")]
        [Argument(Name = "SecondValue")]
        public string SecondValue { get; set; }

        public async Task<object> RunAsync(CommandLineProcessor processor, IConsoleHost host)
        {
             //Connect and create users collection for LiteDB.org
            //LiteDB connection
            LiteDatabase db = new LiteDatabase(@"Users.db");

             //Get users collection
            var col = db.GetCollection<Users>("users");

            //Create Scrypt Password
            ScryptEncoder encoder = new ScryptEncoder();
            string hashsedPassword = encoder.Encode(SecondValue);
 
            if (col.Count() == 0)
            {
               // Create your new customer instance
                var user = new Users
                {
                    UserName = FirstValue,
                    UserPassword = hashsedPassword,
                    IsActive = true
                };

                // Create unique index in Name field
                col.EnsureIndex(x => x.UserName, true);

                // Insert new customer document (Id will be auto-incremented)
                col.Insert(user);
            } else {

                // Create your new customer instance
                var user = new Users
                {
                    UserName = FirstValue,
                    UserPassword = hashsedPassword,
                    IsActive = true
                };

                // Insert new customer document (Id will be auto-incremented)
                col.Insert(user);
            }

            host.WriteMessage("AvatarName: " + FirstValue);
            host.WriteMessage("AvatarPassword: " + hashsedPassword);
            return null;
        }
    }
}
