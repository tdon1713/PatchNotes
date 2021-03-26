using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using PatchNotes.Token;
using System;
using PatchNotes.Utility;

namespace PatchNotes.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private ISessionStorageService SessionStorage { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }

        private bool _isLoggedIn = false;
        private bool _drawerOpen = true;
        bool isLoggingIn = false;

        private string _username;
        private string _password;
        private string _error;
        private bool success;

        private MudForm form;

        protected override void OnInitialized()
        {
            base.OnInitialized();
#if DEBUG
            _username = "admin";
            _password = "admin";
#endif
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //TODO: if the user navigates to  "/" or "", then the item should be removed and login should be shown.

            try
            {
                if (!firstRender)
                    return;

                var isTokenAvailable = await SessionStorage.ContainKeyAsync(Constants.SessionStorageNames.Token);
                if (isTokenAvailable)
                {
                    var token = await SessionStorage.GetItemAsync<string>(Constants.SessionStorageNames.Token);
                    if (!string.IsNullOrEmpty(token))
                    {
                        if (!TokenManager.IsTokenValid(token))
                        {
                            await SessionStorage.RemoveItemAsync(Constants.SessionStorageNames.Token);
                            _isLoggedIn = false;
                            return;
                        }

                        _isLoggedIn = true;
                    }
                }
            }
            finally
            {
                StateHasChanged();
            }
        }

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        private async Task Login(MouseEventArgs e)
        {
            isLoggingIn = true;
            await Task.Delay(500);
            _error = string.Empty;

            try
            {
                form.Validate();
                if (!form.IsValid)
                    return;

                if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
                {
                    _error = "Username or Password is Invalid";
                    return;
                }

                bool adminLogin = false;
#if DEBUG
                if (_username.Equals("admin", StringComparison.InvariantCultureIgnoreCase) &&
                    _password.Equals("admin", StringComparison.InvariantCultureIgnoreCase))
                {
                    adminLogin = true;
                }
#endif
                if (!adminLogin)
                {
                    using PrincipalContext pc = new PrincipalContext(ContextType.Machine);
                    bool isCredentialValid = pc.ValidateCredentials(_username, _password);
                    if (!isCredentialValid)
                    {
                        _error = "Username or Password is Invalid";
                        return;
                    }

                    if (pc.ContextType == ContextType.Domain)
                    {
                        var user = UserPrincipal.FindByIdentity(pc, _username);
                        if (user == null)
                        {
                            _error = "User has not been setup";
                            return;
                        }

                        // Check permissions here
                    }
                }

                var token = TokenManager.GenerateTDSUser(_username);
                await SessionStorage.SetItemAsync(Constants.SessionStorageNames.Token, token);

                NavManager.NavigateTo("/User/Dashboard", forceLoad: true);
            }
            finally
            {
                isLoggingIn = false;
            }
        }
    }
}
