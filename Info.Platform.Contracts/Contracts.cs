namespace Info.PlatformContracts
{
    public record PlatformCreated(int Id, string Name);

    public record PlatformUpdated(int Id, string Name);

    public record PlatformDeleted(int Id);
}