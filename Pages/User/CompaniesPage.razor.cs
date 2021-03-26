using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using PatchNotes.Data;
using PatchNotes.Dialogs;
using PatchNotes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatchNotes.Pages.User
{
    public partial class CompaniesPage
    {
        bool isDialogLoading = true;
        bool isDataLoading = true;
        IEnumerable<Company> _companies = new List<Company>();

        [Inject]
        CompanyService CompanyService { get; set; }

        [Inject]
        IDialogService DialogService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadCompaniesAsync();
        }

        private async Task ViewCompanyDetails(MouseEventArgs e, Company company = null)
        {
            var parameters = new DialogParameters
            {
                { "Company", company ?? new Company() }
            };

            var dialog = DialogService.Show<CompanyDetailsDialog>(company?.Name ?? "New Company", parameters);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await LoadCompaniesAsync();
            }
        }

        private async Task OnCompanyClicked(TableRowClickEventArgs<Company> e)
        {
            await ViewCompanyDetails(null, e.Item);
        }
        
        private async Task LoadCompaniesAsync()
        {
            isDataLoading = true;

            _companies = await CompanyService.GetCompaniesAsync();

            isDataLoading = false;
        }
    }
}
