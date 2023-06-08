public class ProducerService
{
    private readonly IMediator _mediator;

    public ProducerService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void ProduceEmail(string subject)
    {
        _mediator.Send(new EmailMessage(subject));
    }
}