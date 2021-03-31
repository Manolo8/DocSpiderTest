using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Shared.Utils.Other;

namespace DocSpiderTest.Properties {
    public class ValidationMetadataProvider : IValidationMetadataProvider {
        private readonly ValidationMessages _messages = new ValidationMessages();

        public void CreateValidationMetadata(ValidationMetadataProviderContext context) {
            if (context.Attributes == null) return;

            foreach (var contextParameterAttribute in context.Attributes) {
                if (!(contextParameterAttribute is ValidationAttribute attribute))
                    continue;

                if (attribute.ErrorMessage != null)
                    continue;

                var message = _messages.Get(attribute.GetType());

                if (message != null)
                    attribute.ErrorMessage = message;
            }
        }
    }
}