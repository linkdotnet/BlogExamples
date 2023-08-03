namespace FSharpCombined.Domain

type Product = { Name: string; Price: decimal }
type OrderLine = { Product: Product; Quantity: int }

type Discount = 
    | Percentage of float
    | FixedAmount of decimal

type Order(orderLines: OrderLine list, discount: Discount option) =
    member this.CalculateTotalPrice() =
        let subtotal = List.sumBy (fun ol -> ol.Product.Price * (decimal ol.Quantity)) orderLines
        let totalDiscount =
            discount
            |> Option.map (fun d -> 
                match d with
                | Percentage p -> subtotal * (decimal p / 100M)
                | FixedAmount f -> f)
            |> Option.defaultValue 0M
        subtotal - totalDiscount