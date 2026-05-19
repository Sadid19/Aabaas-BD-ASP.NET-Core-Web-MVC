using System;
using System.ComponentModel.DataAnnotations;
using BLL.DTOs;

namespace BLL.Validations
{
    public class CheckInDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime checkIn)
            {
                if (checkIn.Date < DateTime.Today)
                {
                    return new ValidationResult("Check-in date cannot be in the past!");
                }

                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid check-in date.");
        }
    }

    public class CheckOutDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            BookingDTO instance = validationContext.ObjectInstance as BookingDTO;
            if (instance == null)
            {
                return new ValidationResult("Validation error.");
            }

            if (value is DateTime checkOut)
            {
                if (checkOut.Date <= instance.CheckIn.Date)
                {
                    return new ValidationResult("Check-out must be after check-in date.");
                }

                if ((checkOut - instance.CheckIn).TotalDays > 30)
                {
                    return new ValidationResult("Booking cannot exceed 30 nights.");
                }

                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid check-out date.");
        }
    }
}
