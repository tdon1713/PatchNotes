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

        void Delete()
        {
            MudDialog.Close(DialogResult.Ok(true));
        }
    }
}
