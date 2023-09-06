namespace AspNetCoreFromScratch;

public class MyController : ControllerBase
{
    [Route("api/post")]
    public MyDto Call(DtoRequest request)
    {
        Console.WriteLine("Inside Controller");
        return new MyDto(request.Name);
    }

    [Route("api/another")]
    public MyDto Another(DtoRequest request)
    {
        return new MyDto("Another " + request.Name);
    }
}

public record MyDto(string Name);
public record DtoRequest(string Name);