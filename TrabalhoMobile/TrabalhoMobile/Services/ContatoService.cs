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
    public class ContatoService
    {
        FirebaseClient firebase = new FirebaseClient("https://xamarintrabalho-61861.firebaseio.com/");

        public async Task AddContato(int contatoid, string nome, string email)
        {
            await firebase.Child("Contatos")
                .PostAsync(new Contato() { ContatoId = contatoid, Nome = nome, Email = email });
        }

        public async Task<List<Contato>> GetContatos()
        {
            return (await firebase
                .Child("Contatos")
                .OnceAsync<Contato>()).Select(item => new Contato
                {
                    Nome = item.Object.Nome,
                    Email = item.Object.Email,
                    ContatoId = item.Object.ContatoId
                }).ToList();
        }

        public async Task<Contato> GetContato(int contatoid)
        {
            try
            {
                var contato = (await firebase
                    .Child("Contatos")
                    .OnceAsync<Contato>())
                    .Where(a => a.Object.ContatoId == contatoid).FirstOrDefault();

                return await firebase.Child("Contatos")
                    .Child(contato.Key).OnceSingleAsync<Contato>();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task UpdateContato(int contatoId, string nome, string email)
        {
            try
            {
                var toUpdateContato = (await firebase
                    .Child("Contatos")
                    .OnceAsync<Contato>())
                    .Where(a => a.Object.ContatoId == contatoId).FirstOrDefault();

                await firebase
                    .Child("Contatos")
                    .Child(toUpdateContato.Key)
                    .PutAsync(new Contato()
                    { ContatoId = contatoId, Nome = nome, Email = email });
            }
            catch (System.Exception )
            {
                throw;
            }
        }

        public async Task DeleteContato(int contatoId)
        {
            try
            {
                var toDeleteContato = (await firebase
                    .Child("Contatos")
                    .OnceAsync<Contato>())
                    .Where(a => a.Object.ContatoId == contatoId).FirstOrDefault();

                await firebase.Child("Contatos")
                    .Child(toDeleteContato.Key)
                    .DeleteAsync();

            }
            catch (System.Exception )
            {
                throw;
            }
        }
    }
}
