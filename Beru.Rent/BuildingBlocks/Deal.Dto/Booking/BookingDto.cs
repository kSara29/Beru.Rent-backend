using System.Text.Json.Serialization;

namespace Deal.Dto.Booking;

[Serializable]
public record BookingDto(
    
    [property: JsonPropertyName("adId")]Guid AdId,
    [property: JsonPropertyName("tenantId")]Guid TenantId,
    [property: JsonPropertyName("dbeg")]DateTime Dbeg,
    [property: JsonPropertyName("dend")]DateTime Dend,
    [property: JsonPropertyName("cost")]decimal Cost,
    [property: JsonPropertyName("bookingState")]string BookingState
    );