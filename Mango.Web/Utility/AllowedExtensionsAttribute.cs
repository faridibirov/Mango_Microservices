using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Utility;

public  class AllowedExtensionsAttribute: ValidationAttribute
{
    private readonly string[] _allowedExtensions;

    public AllowedExtensionsAttribute(string[] allowedExtensions)
    {
        _allowedExtensions = allowedExtensions;
    }

    protected override ValidationResult? IsValid (object? value, ValidationContext validationContext)
    {
        var file = value as IFormFile;

        if (file != null)
        {
            var extention = Path.GetExtension(file.FileName);
            if(!_allowedExtensions.Contains(extention.ToLower()))
            {
                return new ValidationResult("This photo extention is not allowed!");
            }
        }

        return ValidationResult.Success;
    }
}
