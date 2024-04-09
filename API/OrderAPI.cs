using hip_hop_pizza.Models;
using HipHopPizza.Dtos;
using Microsoft.EntityFrameworkCore;
namespace HipHopPizza.API
{
    public class OrderAPI
    {
        public static void Map(WebApplication app)
        {
            // Get all the Orders
            app.MapGet("/orders", (HipHopPizzaDbContext db) =>
            {
                return db.Orders.ToList();
            });

            // Get a Single Order
            app.MapGet("/orders/{orderId}", (HipHopPizzaDbContext db, int orderId) =>
            {
                return db.Orders.FirstOrDefault(o => o.Id == orderId);
            });

            // Create an Order
            app.MapPost("/orders", (HipHopPizzaDbContext db, Order newOrder) =>
            {
                db.Orders.Add(newOrder);
                db.SaveChanges();
                return Results.Created($"/orders/{newOrder.Id}", newOrder);
            });

            // Delete an Order
            app.MapDelete("/orders", (HipHopPizzaDbContext db, int id) =>
            {
                var orderToDelete = db.Orders.FirstOrDefault(o => o.Id == id);
                if (orderToDelete == null)
                {
                    return Results.NotFound("Sorry this Order couldn't be found.");
                }
                else
                {
                    db.Orders.Remove(orderToDelete);

                    db.SaveChanges();
                }
                return Results.Ok();
            });

            // Update an Order
            app.MapPut("/orders", (HipHopPizzaDbContext db, int orderId, OrderDto updateOrder) =>
            {
                var orderToUpdate = db.Orders.Single(o => o.Id == orderId);
                orderToUpdate.Name= updateOrder.Name;
                orderToUpdate.Closed = updateOrder.Closed;
                orderToUpdate.Phone = updateOrder.Phone;
                orderToUpdate.Email = updateOrder.Email;
                orderToUpdate.Type = updateOrder.Type;
                orderToUpdate.PaymentType = updateOrder.PaymentType;
                orderToUpdate.Total = updateOrder.Total;
                orderToUpdate.Tip = updateOrder.Tip;
                
                db.SaveChanges();
                return Results.Created($"/orders/{orderToUpdate.Id}", updateOrder);
            });
        }
    }
}