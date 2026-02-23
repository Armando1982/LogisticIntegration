namespace LogisticIntegration.Domain.SharedKernel;

/// <summary>
/// Represents an employee in the system.
/// This is a domain entity that enforces invariants through constructor validation.
/// </summary>
public class Employee
{
    public Guid Id { get; private set; }
    public string EmployeeNumber { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new Employee instance.
    /// </summary>
    /// <param name="id">Unique identifier for the employee</param>
    /// <param name="employeeNumber">Unique employee number</param>
    /// <param name="firstName">First name of the employee</param>
    /// <param name="lastName">Last name of the employee</param>
    /// <exception cref="ArgumentException">Thrown when employeeNumber, firstName, or lastName are empty or whitespace</exception>
    public Employee(Guid id, string employeeNumber, string firstName, string lastName)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        if (string.IsNullOrWhiteSpace(employeeNumber))
            throw new ArgumentException("Employee number cannot be empty or whitespace", nameof(employeeNumber));

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty or whitespace", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty or whitespace", nameof(lastName));

        Id = id;
        EmployeeNumber = employeeNumber;
        FirstName = firstName;
        LastName = lastName;
        IsActive = true;
    }

    /// <summary>
    /// Deactivates the employee.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }
}
