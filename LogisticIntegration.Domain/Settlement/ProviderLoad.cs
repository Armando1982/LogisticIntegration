namespace LogisticIntegration.Domain.Settlement;

/// <summary>
/// Represents a load provided by a supplier/provider during a settlement.
/// Tracks the documentary weight and unit price for settlement calculations.
/// </summary>
public class ProviderLoad
{
    public Guid Id { get; private set; }
    public string ProviderCode { get; private set; }
    public string ProductCode { get; private set; }
    public decimal DocumentaryWeight { get; private set; }
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Creates a new ProviderLoad instance.
    /// </summary>
    /// <param name="id">Unique identifier for the provider load</param>
    /// <param name="providerCode">Code identifying the provider/supplier</param>
    /// <param name="productCode">Code identifying the product</param>
    /// <param name="documentaryWeight">The weight documented for this load in kilograms</param>
    /// <param name="unitPrice">The unit price of the product</param>
    /// <exception cref="ArgumentException">Thrown when id is empty, or providerCode/productCode are empty/whitespace</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when documentaryWeight or unitPrice are not greater than 0</exception>
    public ProviderLoad(Guid id, string providerCode, string productCode, decimal documentaryWeight, decimal unitPrice)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        if (string.IsNullOrWhiteSpace(providerCode))
            throw new ArgumentException("ProviderCode cannot be empty or whitespace", nameof(providerCode));

        if (string.IsNullOrWhiteSpace(productCode))
            throw new ArgumentException("ProductCode cannot be empty or whitespace", nameof(productCode));

        if (documentaryWeight <= 0)
            throw new ArgumentOutOfRangeException(nameof(documentaryWeight), "Documentary weight must be greater than 0");

        if (unitPrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price must be greater than 0");

        Id = id;
        ProviderCode = providerCode;
        ProductCode = productCode;
        DocumentaryWeight = documentaryWeight;
        UnitPrice = unitPrice;
    }
}
