using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoMobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrabalhoMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContatosView : ContentPage
    {
        ContatoService contatoService = new ContatoService();
        public ContatosView()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            var contatos = await contatoService.GetContatos();
            listaContatos.ItemsSource = contatos;
        }

        private async void btnIncluir_Clicked(object sender, EventArgs e)
        {
            await contatoService.AddContato(Convert.ToInt32(edtId.Text), edtNome.Text, edtEmail.Text);
            edtId.Text = string.Empty;
            edtNome.Text = string.Empty;
            edtEmail.Text = string.Empty;

            await DisplayAlert("Sucesso", "Contato incluído com sucesso", "Ok");
        }

        private async Task ExibeContatos()
        {
            var contatos = await contatoService.GetContatos();
            listaContatos.ItemsSource = contatos;
        }

        private async void btnExibir_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(edtId.Text))
            {
                await DisplayAlert("Erro", "ID do contato inválido", "Ok");
            }
            else
            {
                try
                {
                    var contato = await contatoService.GetContato(Convert.ToInt32(edtId.Text));

                    if (contato != null)
                    {
                        edtId.Text = contato.ContatoId.ToString();
                        edtNome.Text = contato.Nome;
                        edtEmail.Text = contato.Email;
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Não existe contato com esse ID", "Ok");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", "Contato não encontrado : " + ex.Message, "Ok");
                }
            }
        }

        private async void btnAtualizar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(edtId.Text))
            {
                await DisplayAlert("Erro", "ID do contato inválido", "Ok");
            }
            else
            {
                try
                {
                    await contatoService.UpdateContato(Convert.ToInt32(edtId.Text), edtNome.Text, edtEmail.Text);

                    edtId.Text = string.Empty;
                    edtNome.Text = string.Empty;
                    edtEmail.Text = string.Empty;

                    await DisplayAlert("Sucesso", "Contato atualizado com sucesso", "Ok");

                    await ExibeContatos();

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", "Contato não atualizado : " + ex.Message, "Ok");
                }
            }
        }

        private async void btnDeletar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(edtId.Text))
            {
                await DisplayAlert("Erro", "ID do contato inválido", "Ok");
            }
            else
            {
                try
                {
                    await contatoService.DeleteContato(Convert.ToInt32(edtId.Text));

                    edtId.Text = string.Empty;

                    await DisplayAlert("Sucesso", "Contato deletado com sucesso", "Ok");

                    await ExibeContatos();

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", "Contato não deletado : " + ex.Message, "Ok");
                }
            }
        }
    }
}