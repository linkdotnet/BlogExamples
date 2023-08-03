using FSharpCombined.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FSharp.Collections;

namespace FSharpCombined.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private static readonly List<Order> Orders = new();

    [HttpPost]
    public ActionResult<Order> CreateOrder([FromBody] List<OrderLine> orderLines, [FromBody] Discount discount)
    {
        var fsharpOrderLines = ListModule.OfSeq(orderLines);
        var order = new Order(fsharpOrderLines, discount);
        Orders.Add(order);
        return CreatedAtAction(nameof(GetOrderById), new { id = Orders.Count - 1 }, order);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Order> GetOrderById(int id)
    {
        if (id < 0 || id >= Orders.Count)
        {
            return NotFound();
        }

        return Orders[id];
    }

    [HttpGet("{id:int}/total")]
    public ActionResult<decimal> GetOrderTotalPrice(int id)
    {
        if (id < 0 || id >= Orders.Count)
        {
            return NotFound();
        }

        return Orders[id].CalculateTotalPrice();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetAllOrders()
    {
        return Orders;
    }
}