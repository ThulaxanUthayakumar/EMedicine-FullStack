namespace EMedicine.Api.DTOs;
public record RegisterRequest(string FirstName,string LastName,string Email,string Password,string? Phone);
public record LoginRequest(string Email,string Password);
public record CartRequest(int MedicineId,int Quantity);
public record StatusRequest(string Status);
public record ProfileRequest(string FirstName,string LastName,string? Phone,string? Address);

public record FundRequest(decimal Amount);
