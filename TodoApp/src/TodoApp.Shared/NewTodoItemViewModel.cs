using MADE.Data.Validation;
using MADE.Data.Validation.Validators;
using MvvmHelpers;
using System;

namespace TodoApp
{
    public class NewTodoItemViewModel : ObservableObject
    {
        private string title;
        private string description;
        private DateTimeOffset dueDate;
        private bool isTitleValid;
        private bool isDescriptionValid;
        private bool isViewModelValid;

        public NewTodoItemViewModel()
        {
            TitleValidators.Validated += ValidateTitle;
            DescriptionValidators.Validated += ValidateDescription;
        }


        public string Title
        {
            get => title;
            set
            {
                SetProperty(ref title, value);
            }
        }

        public string Description
        {
            get => description;
            set
            {
                SetProperty(ref description, value);
            }
        }

        public DateTimeOffset DueDate
        {
            get => dueDate;
            set
            {
                SetProperty(ref dueDate, value);
            }
        }

        public bool IsTitleValid
        {
            get => isTitleValid;
            set
            {
                SetProperty(ref isTitleValid, value);
            }
        }

        public bool IsDescriptionValid
        {
            get => isDescriptionValid;
            set
            {
                SetProperty(ref isDescriptionValid, value);
            }
        }

        public bool IsViewModelValid
        {
            get => isViewModelValid;
            set
            {
                SetProperty(ref isViewModelValid, value);
            }
        }

        public ValidatorCollection TitleValidators { get; } = new ValidatorCollection() { new RequiredValidator(), new MinLengthValidator(4) };
        public ValidatorCollection DescriptionValidators { get; } = new ValidatorCollection() { new RequiredValidator(), new MinLengthValidator(10) };

        private void ValidateTitle(object sender, InputValidatedEventArgs args)
        {
            IsTitleValid = !TitleValidators.IsInvalid;
            UpdateIsViewModelValid();
        }

        private void ValidateDescription(object sender, InputValidatedEventArgs args)
        {
            IsDescriptionValid = !DescriptionValidators.IsInvalid;
            UpdateIsViewModelValid();
        }

        private void UpdateIsViewModelValid()
        {
            IsViewModelValid = IsTitleValid && IsDescriptionValid;
        }
    }
}
