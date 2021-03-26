using Microsoft.AspNetCore.Components;
using MudBlazor;
using PatchNotes.Data;
using PatchNotes.Models;
using System.Threading.Tasks;

namespace PatchNotes.Dialogs
{
    public partial class CompanyDetailsDialog
    {
        bool isLoading = false;
        private bool success;
        private MudForm form;

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public string ContentText { get; set; }

        [Parameter] public Company Company { get; set; } = new Company();

        [Inject] CompanyService CompanyService { get; set; }

        [Inject] IDialogService DialogService { get; set; }

        async Task Submit()
        {
            try
            {
                isLoading = true;

                form.Validate();
                if (!form.IsValid)
                    return;

                await CompanyService.SaveAsync(Company);

                MudDialog.Close(DialogResult.Ok(true));
            }
            finally
            {
                isLoading = false;
            }
        }

        void Cancel() => MudDialog.Cancel();

        async void Delete()
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(ConfirmCancelDialog.ContentText), "Are you sure you want to delete this company? This process cannot be undone");
            parameters.Add(nameof(ConfirmCancelDialog.ButtonText), "Delete");
            parameters.Add(nameof(ConfirmCancelDialog.Color), Color.Error);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
            var dialog = DialogService.Show<ConfirmCancelDialog>("Delete", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await CompanyService.DeleteAsync(Company.ID);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
    }
}
