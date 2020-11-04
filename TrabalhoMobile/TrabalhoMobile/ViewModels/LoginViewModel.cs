using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrabalhoMobile.Services;
using TrabalhoMobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TrabalhoMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _Username;
        public string Username
        {
            set
            {
                this._Username = value;
                OnPropertyChanged();
            }
            get
            {
                return this._Username;
            }
        }
        private string _Password;
        public string Password
        {
            set
            {
                this._Password = value;
                OnPropertyChanged();
            }
            get
            {
                return this._Password;
            }
        }
        private bool _Result;
        public bool Result
        {
            set
            {
                this._IsBusy = value;
                OnPropertyChanged();
            }
            get
            {
                return this._IsBusy;
            }
        }
        private bool _IsBusy;
        public bool IsBusy
        {
            set
            {
                this._Result = value;
                OnPropertyChanged();
            }
            get
            {
                return this._Result;
            }
        }

        public Command LoginCommand { get; set; } // Comandos que estão relacionados aos botões login e registro
        public Command RegisterCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginCommandAsync());
            RegisterCommand = new Command(async () => await RegisterCommandAsync());
        }

        private async Task RegisterCommandAsync() // implementação de validação de registro para usuário ja cadastrado
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var userService = new UserService();

                Result = await userService.RegisterUser(Username, Password);
                if (Result)
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Usuário Registrado", "Ok");
                else
                    await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao registrar usuário", "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoginCommandAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true; // Validação de login

                var userService = new UserService();

                Result = await userService.LoginUser(Username, Password);
                if (Result)
                {
                    Preferences.Set("Username", Username);
                    await Application.Current.MainPage.Navigation.PushAsync(new ContatosView());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "usuário ou senha inválidos", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
