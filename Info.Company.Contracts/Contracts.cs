namespace Info.CompanyContracts
{
    public record CompanyCreated(int Id, string Name);

    public record CompanyUpdated(int Id, string Name);

    public record CompanyDeleted(int Id);
}