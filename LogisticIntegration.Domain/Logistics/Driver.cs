namespace LogisticIntegration.Domain.Logistics;

/// <summary>
/// Represents a driver in the logistics domain.
/// A driver is associated with an employee but maintains a strict reference to the employee ID
/// without inheritance, allowing drivers to have unique logistics-specific attributes.
/// </summary>
public class Driver
{
    public Guid Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public string LicenseNumber { get; private set; }
    public DateTime LicenseExpirationDate { get; private set; }

    /// <summary>
    /// Creates a new Driver instance.
    /// </summary>
    /// <param name="id">Unique identifier for the driver</param>
    /// <param name="employeeId">Reference to the associated employee</param>
    /// <param name="licenseNumber">Driver's license number</param>
    /// <param name="licenseExpirationDate">Date when the driver's license expires</param>
    /// <exception cref="ArgumentException">Thrown when id or employeeId are empty, or licenseNumber is empty/whitespace</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when licenseExpirationDate is in the past</exception>
    public Driver(Guid id, Guid employeeId, string licenseNumber, DateTime licenseExpirationDate)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        if (employeeId == Guid.Empty)
            throw new ArgumentException("EmployeeId cannot be empty", nameof(employeeId));

        if (string.IsNullOrWhiteSpace(licenseNumber))
            throw new ArgumentException("License number cannot be empty or whitespace", nameof(licenseNumber));

        if (licenseExpirationDate < DateTime.UtcNow.Date)
            throw new ArgumentOutOfRangeException(nameof(licenseExpirationDate), "License expiration date cannot be in the past");

        Id = id;
        EmployeeId = employeeId;
        LicenseNumber = licenseNumber;
        LicenseExpirationDate = licenseExpirationDate;
    }

    /// <summary>
    /// Renews the driver's license with a new license number and expiration date.
    /// </summary>
    /// <param name="newLicenseNumber">The new license number</param>
    /// <param name="newExpirationDate">The new expiration date for the license</param>
    /// <exception cref="ArgumentException">Thrown when newLicenseNumber is empty or whitespace</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when newExpirationDate is in the past</exception>
    public void RenewLicense(string newLicenseNumber, DateTime newExpirationDate)
    {
        if (string.IsNullOrWhiteSpace(newLicenseNumber))
            throw new ArgumentException("License number cannot be empty or whitespace", nameof(newLicenseNumber));

        if (newExpirationDate < DateTime.UtcNow.Date)
            throw new ArgumentOutOfRangeException(nameof(newExpirationDate), "License expiration date cannot be in the past");

        LicenseNumber = newLicenseNumber;
        LicenseExpirationDate = newExpirationDate;
    }
}
