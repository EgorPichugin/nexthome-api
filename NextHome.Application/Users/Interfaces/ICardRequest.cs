namespace NextHome.Application.Users.Interfaces;

/// <summary>
/// Card request contract.
/// </summary>
public interface ICardRequest
{
    /// <summary>
    /// The title of the card.
    /// </summary>
    public string Title { get; }
    
    /// <summary>
    /// The description of the card.
    /// </summary>
    public string Description { get; }    
}

/// <summary>
/// Create card request contract.
/// </summary>
public interface ICreateCardRequest : ICardRequest;

/// <summary>
/// Update card request contract.
/// </summary>
public interface IUpdateCardRequest: ICardRequest;