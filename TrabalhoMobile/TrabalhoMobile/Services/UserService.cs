using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoMobile.Models;

namespace TrabalhoMobile.Services
{
    public class UserService
    {
        FirebaseClient client;

        public UserService()
        {
            client = new FirebaseClient("https://xamarintrabalho-61861.firebaseio.com/"); //Definido a instância do firebase
        }                                                                                 //Apenas passando a URL

        public async Task<bool> IsUserExists(string name)
        {
            var user = (await client.Child("Users")
                .OnceAsync<User>())
                .Where(u => u.Object.Username == name)
                .FirstOrDefault();

            return (user != null);
        }

        public async Task<bool> RegisterUser(string name, string password)
        {
            if (await IsUserExists(name) == false) // Verificado se o nó("Tabela") no firebase existe, se não existe é criado um.
            {
                await client.Child("Users")
                    .PostAsync(new User()
                    {
                        Username = name,
                        Password = password
                    });
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> LoginUser(string name, string password)
        {
            var user = (await client.Child("Users")
                    .OnceAsync<User>())
                    .Where(u => u.Object.Username == name)
                     .Where(u => u.Object.Password == password)
                      .FirstOrDefault();
            return (user != null);
        }
    
    }
}
