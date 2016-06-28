using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.GettingStarted.Models
{
    using Catel.Data;
    public class Person : ModelBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName
        {
            get { return GetValue<string>(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        /// <summary>
        /// Register the FirstName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FirstNameProperty = RegisterProperty("FirstName", typeof(string), null);

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName
        {
            get { return GetValue<string>(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        /// <summary>
        /// Register the LastName property so it is known in the class.
        /// </summary>
        public static readonly PropertyData LastNameProperty = RegisterProperty("LastName", typeof(string), null);
        #endregion

        #region Methods

        public override string ToString()
        {
            string fullName = string.Empty;
            if (!string.IsNullOrEmpty(FirstName))
            {
                fullName += FirstName;
            }
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrWhiteSpace(LastName))
            {
                fullName += " ";
            }
            if (!string.IsNullOrWhiteSpace(LastName))
            {
                fullName += LastName;
            }
            return fullName;
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                validationResults.Add(FieldValidationResult.CreateError(FirstNameProperty, "The first name is required"));
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                validationResults.Add(FieldValidationResult.CreateError(LastNameProperty, "The last name is required"));
            }
        }

        #endregion
    }
}
