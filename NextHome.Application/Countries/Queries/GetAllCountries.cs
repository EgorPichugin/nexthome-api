using MediatR;
using NextHome.Application.Countries.Responses;
using NextHome.Application.Countries.Services;

namespace NextHome.Application.Countries.Queries;

public record GetAllCountries : IRequest<List<CountryResponse>>;

public class GetAllCountriesHandler(ICsvReaderService csvReaderService) : IRequestHandler<GetAllCountries, List<CountryResponse>>
{
    private const string FolderName = "Data";
    private const string FileName = "data.csv";
    
    public Task<List<CountryResponse>> Handle(GetAllCountries request, CancellationToken cancellationToken)
    {
        using var csvReader = csvReaderService.ReadCsv(FolderName, FileName);
        var records = csvReader.GetRecords<CountryResponse>().ToList();
        return Task.FromResult(records);
    }
}